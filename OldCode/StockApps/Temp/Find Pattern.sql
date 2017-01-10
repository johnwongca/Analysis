use Stock

declare @ExchangeID int, @SymbolID int, @SymbolName Varchar(512), @ExchangeName varchar(10)
if object_id('tempdb..#Result') is not null
	drop table #Result
create table #Result (ID int identity(1,1) primary key, SymbolID int, Date datetime, Type varchar(100))
declare @MaxResult int, @CurrentResultCount int
select @CurrentResultCount = 0, @MaxResult = 20;

declare @High0 money, @Opening0 money, @Closing0 money, @Low0 money, @Volume0 money, @Date0 datetime
declare @High1 money, @Opening1 money, @Closing1 money, @Low1 money, @Volume1 money, @Date1 datetime
declare @High2 money, @Opening2 money, @Closing2 money, @Low2 money, @Volume2 money, @Date2 datetime
declare @High3 money, @Opening3 money, @Closing3 money, @Low3 money, @Volume3 money, @Date3 datetime
declare @High4 money, @Opening4 money, @Closing4 money, @Low4 money, @Volume4 money, @Date4 datetime
declare @High5 money, @Opening5 money, @Closing5 money, @Low5 money, @Volume5 money, @Date5 datetime
print 'Analyzing ....'
declare c cursor local static for
	select a.ID, a.ExchangeID, b.Name, a.Name + ' - ' + A.Description
	from A.Symbol a
		inner join A.Exchange b on a.ExchangeID = b.ID
	where b.Name = 'NASDAQ'
		--and SymbolID not in (11869, )
open c
fetch next from c into @SymbolID, @ExchangeID, @ExchangeName, @SymbolName
while @@fetch_status = 0
begin
	if @CurrentResultCount = @MaxResult
		goto __Quit
	print @ExchangeName + ': ' + @SymbolName
	declare c1 cursor local static for
		select High, Opening, Closing, Low, Volume, Date 
		from A.Sync_GetSecurity(@SymbolID, '2006-12-01')
		order by Date Desc
	open c1
	fetch next from c1 into @High5, @Opening5, @Closing5, @Low5, @Volume5, @Date5
	if @@fetch_status != 0 goto _CloseCursor
	fetch next from c1 into @High4, @Opening4, @Closing4, @Low4, @Volume4, @Date4
	if @@fetch_status != 0 goto _CloseCursor	
	fetch next from c1 into @High3, @Opening3, @Closing3, @Low3, @Volume3, @Date3
	if @@fetch_status != 0 goto _CloseCursor
	fetch next from c1 into @High2, @Opening2, @Closing2, @Low2, @Volume2, @Date2
	if @@fetch_status != 0 goto _CloseCursor
	fetch next from c1 into @High1, @Opening1, @Closing1, @Low1, @Volume1, @Date1
	if @@fetch_status != 0 goto _CloseCursor
	fetch next from c1 into @High0, @Opening0, @Closing0, @Low0, @Volume0, @Date0
	if @@fetch_status != 0 goto _CloseCursor
	while(1=1)
	begin

/*
		--Abandoned Baby - UP
		if		@Opening3 < @Closing3 
				and @Opening3 > 0
				and (@Closing3 - @Opening3)/@Opening3 > 0.01
					
			and @High4 <> @Low4 and @Low4 < @Opening4 and @Low4 < @Closing4 and @High4 > @Opening4 and @High4 > @Closing4
			and @Low4 > @High3 and @Low4 > @High5 
			and @Opening4 between @Closing4*0.9995 and @Closing4 * 1.0005

			and @Opening5 > @Closing5 
			and @Opening5 > 0
			and (@Opening5 - @Closing5)/@Closing5 > 0.01
		begin
			print '	Found "Abandoned Baby - Top"...'
			insert into #Result(SymbolID, Date, Type)
				values(@SymbolID, @Date3, 'Abandoned Baby - Top')
			select @CurrentResultCount = @CurrentResultCount +1
			if @CurrentResultCount = @MaxResult goto _CloseCursor
		end
		if @CurrentResultCount = @MaxResult 
			goto _CloseCursor


		--Abandoned Baby - DOWN
		if		@Opening3 > @Closing3 
				and @Closing3 > 0
				and (@Opening3 - @Closing3 )/@Closing3 > 0.01
					
			and @High4 <> @Low4 and @Low4 < @Opening4 and @Low4 < @Closing4 and @High4 > @Opening4 and @High4 > @Closing4
			and @High4 < @Low3 and @High4 < @Low5 
			and @Opening4 between @Closing4*0.9995 and @Closing4 * 1.0005

			and @Opening5 < @Closing5 
			and @Opening5 > 0
			and (@Closing5 - @Opening5)/@Opening5 > 0.01
		begin
			print '	Found "Abandoned Baby - Bottom"...'
			insert into #Result(SymbolID, Date, Type)
				values(@SymbolID, @Date3, 'Abandoned Baby - Bottom')
			select @CurrentResultCount = @CurrentResultCount +1
			if @CurrentResultCount = @MaxResult goto _CloseCursor
		end
		if @CurrentResultCount = @MaxResult 
			goto _CloseCursor

*/
		--Belt-holdLine up
		if	@Opening5 = @Low5 and @Closing5 > @Opening5 * 1.1 and @High5 > @Closing5 * 1.1
		begin
			print '	Found "Belt-holdLine up"...'
			insert into #Result(SymbolID, Date, Type)
				values(@SymbolID, @Date3, 'Belt-holdLine - Up')
			select @CurrentResultCount = @CurrentResultCount +1
			if @CurrentResultCount = @MaxResult goto _CloseCursor
		end
		if @CurrentResultCount = @MaxResult 
			goto _CloseCursor
/*
		--Belt-holdLine Down
		if	@Opening5 = @High5 and @Opening5 > @Closing5 * 1.1 and @Low5 < @Closing5 * 1.1
		begin
			print '	Found "Belt-holdLine Down"...'
			insert into #Result(SymbolID, Date, Type)
				values(@SymbolID, @Date3, 'Belt-holdLine - Down')
			select @CurrentResultCount = @CurrentResultCount +1
			if @CurrentResultCount = @MaxResult goto _CloseCursor
		end
		if @CurrentResultCount = @MaxResult 
			goto _CloseCursor

*/







		select @High5 = @High4, @Opening5 = @Opening4, @Closing5 = @Closing4, @Low5 = @Low4, @Volume5 = @Volume4, @Date5 = @Date4
		select @High4 = @High3, @Opening4 = @Opening3, @Closing4 = @Closing3, @Low4 = @Low3, @Volume4 = @Volume3, @Date4 = @Date3
		select @High3 = @High2, @Opening3 = @Opening2, @Closing3 = @Closing2, @Low3 = @Low2, @Volume3 = @Volume2, @Date3 = @Date2
		select @High2 = @High1, @Opening2 = @Opening1, @Closing2 = @Closing1, @Low2 = @Low1, @Volume2 = @Volume1, @Date2 = @Date1
		select @High1 = @High0, @Opening1 = @Opening0, @Closing1 = @Closing0, @Low1 = @Low0, @Volume1 = @Volume0, @Date1 = @Date0
		fetch next from c1 into @High0, @Opening0, @Closing0, @Low0, @Volume0, @Date0
		if @@fetch_status != 0 goto _CloseCursor
	end
_CloseCursor:
	close c1
	deallocate c1

	if @CurrentResultCount = @MaxResult
		goto __Quit
	fetch next from c into @SymbolID, @ExchangeID, @ExchangeName, @SymbolName
end

__Quit:
close c
deallocate c
print 'Analyzing ....done'
select a.SymbolID, a.Type, a.Date, c.Name as ExchangeName, b.Name as Symbol, b.Description
from #Result a
	inner join A.Symbol b on a.SymbolID = b.ID
	inner join A.Exchange c on b.ExchangeID = c.ID
order by a.Type asc, a.Date desc