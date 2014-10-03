create procedure q.ClearOldData(@YearsBack int = 3)
as
begin
	declare @SymbolIDs table(SymbolID int primary key)
	insert into @SymbolIDs(SymbolID)
		select SymbolID
		from q.Symbol s
		where 
		(
		DayPriceLastUpdate is null and MinutePriceLastUpdate is null or
				DayPriceLastUpdate is not null  and DayPriceLastUpdate < dateadd(year, -@YearsBack, getdate())

			and MinutePriceLastUpdate is not null and MinutePriceLastUpdate < dateadd(year, -@YearsBack, getdate())
			)
		and not exists(select * from q.SymbolChange c where c.FromSymbolID = s.SymbolID and c.Date > dateadd(year, @YearsBack, getdate()))

	--select count(*) from q.QuoteMinute where SymbolID in (select SymbolID from @SymbolIDs)
	delete q.QuoteMinute where SymbolID in (select SymbolID from @SymbolIDs)
	--select count(*) from q.Quote where SymbolID in (select SymbolID from @SymbolIDs)
	delete q.Quote where SymbolID in (select SymbolID from @SymbolIDs)
	--select count(*) from q.Split where SymbolID in (select SymbolID from @SymbolIDs)
	delete q.Split where SymbolID in (select SymbolID from @SymbolIDs)
	--select count(*) from q.SymbolChange where FromSymbolID in (select SymbolID from @SymbolIDs)
	delete q.SymbolChange where FromSymbolID in (select SymbolID from @SymbolIDs)
	--select count(*) from q.Symbol where SymbolID in (select SymbolID from @SymbolIDs)
	delete q.Symbol where SymbolID in (select SymbolID from @SymbolIDs)
	--select * from q.Fundamental where SymbolID in (select SymbolID from @SymbolIDs)
	delete q.Fundamental where SymbolID in (select SymbolID from @SymbolIDs)
end