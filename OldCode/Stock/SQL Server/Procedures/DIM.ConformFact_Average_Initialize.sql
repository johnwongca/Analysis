alter procedure DIM.ConformFact_Average_Initialize
as
begin
	set nocount on
	declare @ret int, @SymbolID int, @Seq int, @i int, @msg varchar(max)
	declare @PeriodFrom smallint, @PeriodTo smallint, @j smallint
	declare @Date datetime, @Opening float, @High float, @Low float, @Closing float, @Volume float
	
/*
	truncate table DIM.Pending_Daily
	truncate table DIM.Fact_Base
	dbcc dbreindex('DIM.Pending_Daily') with no_infomsgs
	dbcc dbreindex('DIM.Fact_Base') with no_infomsgs
*/
	select @PeriodFrom = min(ID), @PeriodTo = max(ID) from DIM.Period
	declare c cursor local static  for
		select top 120 SymbolID, max(Seq)
		from A.Daily
		group by SymbolID
	open c
	fetch next from c into @SymbolID, @Seq
	while @@fetch_status = 0
	begin
		select @msg = 'Start	' + convert(varchar(50), getdate(), 121) 
					+ ' @SymbolID = ' + Cast(@SymbolID as varchar(20)) + '	'
					+ ' @Seq = ' + Cast(@Seq as varchar(20))
		print @msg
		print ''
		select @i = @Seq 
		select @j = @PeriodFrom
		while @j <= @PeriodTo
		begin
			/*select @msg = '	Start	' + convert(varchar(50), getdate(), 121) 
						+ ' @SymbolID = ' + Cast(@SymbolID as varchar(20)) + '	'
						+ ' @Seq = ' + Cast(@Seq as varchar(20)) + ' '
						+ ' @Period = ' + cast(@j as varchar(20))
			print @msg*/
			select @Seq = 1
			while(@Seq <= @i)
			begin
				exec DIM.Fact_BaseWrite @SymbolID, @Seq, @j
				select @Seq = @Seq + 1
			end
			/*select @msg = '	End	' + convert(varchar(50), getdate(), 121) 
						+ ' @SymbolID = ' + Cast(@SymbolID as varchar(20)) + '	'
						+ ' @Seq = ' + Cast(@Seq as varchar(20)) + ' '
						+ ' @Period = ' + cast(@j as varchar(20))
			print @msg
			print ''*/
			select @j = @j +1
		end
		select @msg = 'End	' + convert(varchar(50), getdate(), 121) 
						+ ' @SymbolID = ' + Cast(@SymbolID as varchar(20)) + '	'
						+ ' @Seq = ' + Cast(@Seq as varchar(20))
		print @msg
_NEXT_STOCK_:
		fetch next from c into @SymbolID, @Seq
	end
	close c
	deallocate c
end

