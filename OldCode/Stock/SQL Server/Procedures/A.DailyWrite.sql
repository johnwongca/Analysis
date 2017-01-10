Alter procedure A.DailyWrite
(
	@SymbolID	int,
	@Date		datetime,
	@Opening	Float,
	@High		Float,
	@Low		Float,
	@Closing	Float,
	@Volume		Float,
	@Interest	Float,
	@UpdateIfExists bit = 0
)
as
begin
	declare @Seq int, @d1 datetime, @NeedUpdate bit
	select
			@Seq = max(Seq),
			@d1 = max(Date)
	from A.Daily 
	where SymbolID = @SymbolID
	if @d1 = @Date or exists(select 1 from A.Daily where SymbolID = @SymbolID and Date = @Date)--if it is the same record
	begin
		if @UpdateIfExists = 1 
		begin
			update d1
				set d1.Opening = @Opening, 
					d1.High = @High, 
					d1.Low = @Low, 
					d1.Closing = @Closing, 
					d1.Volume = @Volume, 
					d1.Interest = @Interest,
					d1.DiffClosing = @Closing - isnull(d0.Closing, 0),
					d1.DiffVolume =  @Volume - isnull(d0.Volume, 0),
					d1.WeightedClosing = (@Closing * 2 + @High + @Low) / 4,
					d1.DiffWeightedClosing = ((@Closing * 2 + @High + @Low) / 4) - isnull(d0.WeightedClosing, 0),
					d1.TypicalClosing	= (@Closing + @High + @Low) / 3,
					d1.DiffTypicalClosing = (@Closing + @High + @Low) / 3 - isnull(d0.TypicalClosing, 0),

					d1.VolumeAdjustedClosing = @Closing * (@Volume/1000),
					d1.DiffVolumeAdjustedClosing = @Closing * (@Volume/1000) - isnull(d0.DiffVolumeAdjustedClosing, 0),
					d1.VolumeAdjustedWeightedClosing = (@Closing * 2 + @High + @Low) * (@Volume/1000) / 4,
					d1.DiffVolumeAdjustedWeightedClosing = (@Closing * 2 + @High + @Low) * (@Volume/1000) / 4 - isnull(d0.VolumeAdjustedWeightedClosing, 0),
					d1.VolumeAdjustedTypicalClosing = (@Closing + @High + @Low) * (@Volume/1000) / 3,
					d1.DiffVolumeAdjustedTypicalClosing = (@Closing + @High + @Low) * (@Volume/1000) / 3 - isnull(d0.VolumeAdjustedTypicalClosing, 0)

			from A.Daily d1
				left outer join A.Daily d0 on d0.SymbolID = @SymbolID and d0.Date = @Date - 1
			where d1.SymbolID = @SymbolID
				and d1.Date = @Date
			select @NeedUpdate = 1
		end
	end
	else if @d1 < @Date
	begin
		select @Seq = @Seq +1
	end
	else if @d1 > @Date
	begin
		select @Seq = min(Seq)
		from A.Daily
		where SymbolID = @SymbolID
			and Date > @Date
		update A.Daily
			set Seq = Seq + 1
		where SymbolID = @SymbolID
			and Seq >= @Seq
		select @NeedUpdate = 1
	end
	else if @d1 is null -- if it is the first record
	begin
		select @Seq = 1
	end

	insert into A.Daily(
							SymbolID, Seq, Date, 
							Opening, High, Low, 
							Closing, Volume, Interest, 
							DiffClosing, DiffVolume,
							WeightedClosing, DiffWeightedClosing,
							TypicalClosing, DiffTypicalClosing,
							VolumeAdjustedClosing, DiffVolumeAdjustedClosing, 
							VolumeAdjustedWeightedClosing, DiffVolumeAdjustedWeightedClosing, 
							VolumeAdjustedTypicalClosing, DiffVolumeAdjustedTypicalClosing
						)
		select				@SymbolID, @Seq, @Date, 
							@Opening, @High, @Low, 
							@Closing, @Volume, @Interest,
							@Closing - isnull(d0.Closing, 0), @Volume - isnull(d0.Volume, 0),
							(@Closing * 2 + @High + @Low) / 4, (@Closing * 2 + @High + @Low) / 4 - isnull(d0.WeightedClosing, 0),
							(@Closing + @High + @Low) / 3, (@Closing + @High + @Low) / 3 - isnull(d0.TypicalClosing, 0),

							@Closing * (@Volume / 1000), @Closing * (@Volume / 1000) - isnull(d0.VolumeAdjustedClosing, 0), 
							(@Closing * 2 + @High + @Low)* (@Volume / 1000) / 4, (@Closing * 2 + @High + @Low)* (@Volume / 1000) / 4 - isnull(d0.VolumeAdjustedWeightedClosing, 0),
							(@Closing + @High + @Low) * (@Volume / 1000) / 3, (@Closing + @High + @Low) * (@Volume / 1000) / 3 - isnull(d0.VolumeAdjustedTypicalClosing, 0)
		from (select 1 one) x
			left outer join A.Daily d0 on d0.SymbolID = @SymbolID and d0.Date = @Date - 1
	
	if @NeedUpdate = 1
		update d1
			set d1.DiffClosing = d1.Closing - isnull(d0.Closing, 0),
				d1.DiffVolume =  d1.Volume - isnull(d0.Volume, 0),
				d1.DiffWeightedClosing = d1.WeightedClosing - isnull(d0.WeightedClosing, 0),
				d1.DiffTypicalClosing = d1.TypicalClosing - isnull(d0.TypicalClosing, 0),
				d1.DiffVolumeAdjustedClosing = d1.VolumeAdjustedClosing - isnull(d0.VolumeAdjustedClosing, 0), 
				d1.DiffVolumeAdjustedWeightedClosing = d1.VolumeAdjustedWeightedClosing - isnull(d0.VolumeAdjustedWeightedClosing, 0), 
				d1.DiffVolumeAdjustedTypicalClosing = d1.VolumeAdjustedTypicalClosing - isnull(d0.VolumeAdjustedTypicalClosing, 0)
		from A.Daily d1
			left outer join A.Daily d0 on d0.SymbolID = @SymbolID and d0.Date = @Date
		where d1.SymbolID = @SymbolID
			and d1.Date = @Date + 1
end
