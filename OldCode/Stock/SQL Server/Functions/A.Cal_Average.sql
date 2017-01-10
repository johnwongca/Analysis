alter function A.Cal_Average
(
	@SymbolID int,
	@Seq int = null,
	@Date datetime = null,
	@Type varchar(20), --None, Simple, Exponential, Weighted
	@Units int,
	@Expression varchar(max) = 'Closing',
	@TableName varchar(128) = 'A.Daily'
) returns float
as
begin
	--http://en.wikipedia.org/wiki/Moving_average
	
	if @Seq is null
	begin
		select @Seq = Seq
		from A.Daily
		where SymbolID = @SymbolID
			and Date = @Date
		if @Seq is null
			return null;
	end
	declare @sSymbolID varchar(100), @sSeq varchar(100), @sUnits varchar(100), @sSeqStart varchar(100)
	select 
			@Expression = '(' + @Expression + ')', 
			@sSymbolID = cast(@SymbolID as varchar(100)),
			@sSeq = cast(@Seq as varchar(100)),
			@sUnits = cast(@Units as varchar(100)),
			@sSeqStart = cast( @Seq - @Units + 1  as varchar(100))
	if @Type = 'None' 
		return A.RunSQLFloat('	select ' + @Expression + '
								from ' + @TableName + ' (nolock)
								where SymbolID = ' + @sSymbolID + '
									and Seq = ' + @sSeq
							);
	if @Type = 'Simple' 
		return A.RunSQLFloat('	select avg(' + @Expression + ')
								from ' + @TableName + ' (nolock)
								where SymbolID = ' + @sSymbolID + '
									and Seq between ' + @sSeqStart + ' and ' + @sSeq
							);
	if @Type = 'Weighted'
		return A.RunSQLFloat('	select 
										case when ' + @sUnits + ' = 0 then 0
											else
												sum(' + @Expression + ' * (Seq - ' + @sSeq + '+ ' + @sUnits + '))/(' + @sUnits + ' * (' + @sUnits + '+1) /2)
										end
								from ' + @TableName + ' (nolock)
								where SymbolID = ' + @sSymbolID + '
									and Seq between ' + @sSeqStart + ' and ' + @sSeq
							);
	else if @Type = 'Exponential'
		return A.RunSQLFloat('	declare	@a float;
								select @a = cast(1 as float) - cast(2 as float)/cast((' + @sUnits + ' + 1 ) as float);
								if @a <= 0
								begin
									select @a
									return
								end
								select sum(cast(' + @Expression + ' as float)* power(@a, ' + @sSeq + ' - Seq)) / sum(power(@a, ' + @sSeq + ' - Seq)) 
								from ' + @TableName + ' (nolock)
								where SymbolID = ' + @sSymbolID + '
									and Seq <= ' + @sSeq + '
								--and Seq between ' + @sSeq + '-' + @sUnits + ' - 365 and ' + @sSeq
							);

	return null;
end
