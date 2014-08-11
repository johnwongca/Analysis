create procedure [EODData].[UpdateQuote]
as
begin
	set nocount on
	;merge [$(Quote)].q.Exchange t
	using EODData.Exchange s on t.Exchange = s.Exchange
	when not matched then
		insert (Exchange, ExchangeName, Country, Currency, TimeZone)
			values(s.Exchange, s.Name, s.Country, s.Currency, s.TimeZone)
	when matched and (
							t.ExchangeName<> s.Name
						or isnull(t.Country,'') <> isnull(s.Country, '')
						or isnull(t.Currency,'') <> isnull(s.Currency, '')
						or isnull(t.TimeZone, '') <> isnull(s.TimeZone, '')
					) then
		update set t.ExchangeName = s.Name,
					t.Country = s.Country,
					t.Currency = s.Currency,
					t.TimeZone = s.TimeZone
	;

	;merge [$(Quote)].q.Symbol t
	using EODData.Symbol s on s.Exchange = t.Exchange and s.Symbol = t.Symbol
	when not matched then
		insert(Exchange, Symbol, SymbolName, LongName)
			values(s.Exchange, s.Symbol, s.Name, s.LongName)
	when matched  and (
								t.SymbolName<> s.Name
							or isnull(t.LongName,'')<> isnull(s.LongName, '')
						)
	then
		update set 	t.SymbolName = s.Name,
					t.LongName = s.LongName
	;

	insert into EODData.Symbol(Exchange, Symbol, Name, LongName, Date)
		select Exchange, Symbol, Name, Description, Date
		from EODData.Fundamental i
		where not exists(select * from [$(Quote)].q.Symbol s where s.Exchange = i.Exchange and s.Symbol = i.Symbol)

	;merge [$(Quote)].q.Split as t
	using (
			select i.Date, s.SymbolID, i.Ratio
			from EODData.Split i
				inner join [$(Quote)].q.Symbol s on s.Exchange = i.Exchange and s.Symbol = i.Symbol
		)as s on s.SymbolID= t.SymbolID and s.Date = t.Date
	when not matched then
		insert (SymbolID, Date, Ratio)
		 values (s.SymbolID, s.Date, s.Ratio)
	when matched and (s.Ratio <> t.Ratio) then
		update set t.Ratio = s.Ratio;

	;merge [$(Quote)].q.SymbolChange as t
	using (	select sf.SymbolID FromSymbolID, st.SymbolID ToSymbolID, i.Date
			from EODData.SymbolChange i
				inner join [$(Quote)].q.Symbol sf on sf.Exchange = i.FromExchange and sf.Symbol = i.FromSymbol
				inner join [$(Quote)].q.Symbol st on st.Exchange = i.ToExchange and st.Symbol = i.ToSymbol
		) s on  s.Date = t.Date and s.FromSymbolID = t.FromSymbolID and s.ToSymbolID = t.ToSymbolID
	when not matched then
		insert(Date, FromSymbolID, ToSymbolID)
			values(s.Date, s.FromSymbolID, s.ToSymbolID)
	when not matched by source then
		delete;

	insert into [$(Quote)].q.Name(Name)
	select Sector
	from EODData.Fundamental 
	where Sector is not null
	union 
	select Industry
	from EODData.Fundamental
	where Sector is not null
	
	
	declare @t table (SymbolID int NOT NULL, EffectiveDate date NOT NULL, ExpiryDate date NULL, Sector int NULL, Industry int NULL, Dividend float NULL, DividendDate datetime NULL, DividendYield float NULL, DPS float NULL, EBITDA float NULL, MarketCap bigint NULL, EPS float NULL, PtS float NULL, NTA float NULL, PE float NULL, PEG float NULL, PtB float NULL, Shares bigint NULL, Yield float NULL)
	declare @Date Date
	declare cFundamental cursor local static for
		select distinct Date from EODData.Fundamental order by 1
	open cFundamental
	fetch next from cFundamental into @Date
	while @@fetch_status = 0
	begin
		delete @t
		insert into @t(SymbolID, EffectiveDate, ExpiryDate, Sector, Industry, Dividend, DividendDate, DividendYield, DPS, EBITDA, MarketCap, EPS, PtS, NTA, PE, PEG, PtB, Shares, Yield)
			select SymbolID, EffectiveDate, ExpiryDate, Sector, Industry, Dividend, DividendDate, DividendYield, DPS, EBITDA, MarketCap, EPS, PtS, NTA, PE, PEG, PtB, Shares, Yield
			from (
				merge [$(Quote)].q.Fundamental as t
				using (
							select	s.SymbolID, cast(i.Date as Date) Date, i.Dividend, 
									i.DividendDate, i.DividendYield, i.DPS, 
									i.EBITDA, i.EPS, Ind.NameID Industry, 
									Sec.NameID Sector, i.MarketCap, i.NTA, 
									i.PE, i.PEG, i.PtB, 
									i.PtS, i.Shares, i.Yield
							from EODData.Fundamental i
								inner join [$(Quote)].q.Symbol s on s.Exchange = i.Exchange and s.Symbol = i.Symbol
								left outer join [$(Quote)].q.Name sec on sec.Name = i.Sector
								left outer join [$(Quote)].q.Name Ind on Ind.Name = i.Industry
							where i.Date = @Date
							) as s on t.SymbolID = s.SymbolID and t.ExpiryDate is null
				when matched and (
											--t.EffectiveDate<> s.Date
										--or 
											isnull(t.Sector, 0) <> isnull(s.Sector, 0)
										or isnull(t.Industry,0) <> isnull(s.Industry, 0)
										or isnull(t.Dividend, 0) <> isnull(s.Dividend,0) 
										or isnull(t.DividendDate, '1960-01-01') <> isnull(s.DividendDate, '1960-01-01')
										or isnull(t.DividendYield,0)<> isnull(s.DividendYield,0)
										or isnull(t.DPS,0) <> isnull(s.DPS,0)
										or isnull(t.EBITDA,0) <> isnull(s.EBITDA,0) 
										or isnull(t.MarketCap, 0) <> isnull(s.MarketCap,0)
										or isnull(t.EPS, 0)<> isnull(s.EPS,0)
										or isnull(t.PtS, 0) <> isnull(s.PtS,0) 
										or isnull(t.NTA,0) <> isnull(s.NTA,0)
										or isnull(t.PE,0) <> isnull(s.PE,0)
										or isnull(t.PEG, 0) <> isnull(s.PEG,0)
										or isnull(t.PtB, 0 )<>  isnull(s.PtB, 0 )
										or isnull(t.Shares,0) <> isnull(s.Shares,0)
										or isnull(t.Yield,0) <> isnull(s.Yield,0)
									)then
					update set t.EffectiveDate = s.Date
							,t.Sector = s.Sector
							,t.Industry = s.Industry
							,t.Dividend = s.Dividend 
							,t.DividendDate = s.DividendDate
							,t.DividendYield = s.DividendYield
							,t.DPS = s.DPS
							,t.EBITDA = s.EBITDA 
							,t.MarketCap = s.MarketCap
							,t.EPS = s.EPS
							,t.PtS = s.PtS 
							,t.NTA = s.NTA
							,t.PE = s.PE
							,t.PEG = s.PEG
							,t.PtB = s.PtB
							,t.Shares = s.Shares
							,t.Yield = s.Yield
				when not matched then
					insert (SymbolID, EffectiveDate, ExpiryDate, Sector, Industry, Dividend, DividendDate, DividendYield, DPS, EBITDA, MarketCap, EPS, PtS, NTA, PE, PEG, PtB, Shares, Yield)
						values(s.SymbolID, s.Date, null, s.Sector, s.Industry, s.Dividend, s.DividendDate, s.DividendYield, s.DPS, s.EBITDA, s.MarketCap, s.EPS, s.PtS, s.NTA, s.PE, s.PEG, s.PtB, s.Shares, s.Yield)
				output deleted.SymbolID, deleted.EffectiveDate, deleted.ExpiryDate, deleted.Sector, deleted.Industry, deleted.Dividend, deleted.DividendDate, deleted.DividendYield, deleted.DPS, deleted.EBITDA, deleted.MarketCap, deleted.EPS, deleted.PtS, deleted.NTA, deleted.PE, deleted.PEG, deleted.PtB, deleted.Shares, deleted.Yield
			)ExpiredRecords
		where SymbolID is not null
		--update t1
		--	set t1.ExpiryDate = ExpiryDate.Date
		--from [$(Quote)].q.Fundamental t1
		--	inner join @t s1 on s1.SymbolID = t1.SymbolID and s1.EffectiveDate = t1.EffectiveDate
		--	cross apply (select MAX(EffectiveDate) Date from [$(Quote)].q.Fundamental where SymbolID = s1.SymbolID group by SymbolID) ExpiryDate
		--where t1.EffectiveDate < ExpiryDate.Date
		--	and t1.ExpiryDate is null
		insert into [$(Quote)].q.Fundamental(SymbolID, EffectiveDate, ExpiryDate, Sector, Industry, Dividend, DividendDate, DividendYield, DPS, EBITDA, MarketCap, EPS, PtS, NTA, PE, PEG, PtB, Shares, Yield)
			select SymbolID, EffectiveDate, @Date, Sector, Industry, Dividend, DividendDate, DividendYield, DPS, EBITDA, MarketCap, EPS, PtS, NTA, PE, PEG, PtB, Shares, Yield
			from @t t
			where not exists(select * from [$(Quote)].q.Fundamental t1 where t1.SymbolID = t.SymbolID and t1.EffectiveDate = t.EffectiveDate)
		fetch next from cFundamental into @Date
	end
	close cFundamental
	deallocate cFundamental

	;merge [$(Quote)].q.Quote as t
	using (
			select s.SymbolID, cast(i.Date as Date) Date, i.[Open], i.[High], i.[Low], i.[Close], i.[Volume]
			from (select * from EODData.Quote where IntervalID = 6)  i
				inner join [$(Quote)].q.Symbol s on s.Exchange = i.Exchange and s.Symbol = i.Symbol
		)as s on s.SymbolID= t.SymbolID and s.Date = t.Date
	when not matched then
		insert (SymbolID, Date, [Open],	[High] ,[Low],[Close] ,[Volume])
			values (s.SymbolID, s.Date, s.[Open],	s.[High] ,s.[Low],s.[Close] ,s.[Volume])
	when matched and (t.[Open]<> s.[Open] or t.High <> s.High or t.Low <> s.Low or t.[Close]<> s.[Close] or t.Volume <> s.Volume) then
		update set t.[Open]=s.[Open], t.High = s.High , t.Low = s.Low , t.[Close] = s.[Close] , t.Volume = s.Volume
	;
	;merge [$(Quote)].q.QuoteMinute as t
	using (
			select s.SymbolID, cast(i.Date as Date) Date, cast(i.Date as Time) Time, i.[Open], i.[High], i.[Low], i.[Close], i.[Volume]
			from (select * from EODData.Quote where IntervalID = 0)  i
				inner join [$(Quote)].q.Symbol s on s.Exchange = i.Exchange and s.Symbol = i.Symbol
			where i.IntervalID = 0
		)as s on s.SymbolID= t.SymbolID and s.Date = t.Date and s.Time = t.Time
	when not matched then
		insert (SymbolID, Date, [Open],	[High] ,[Low],[Close] ,[Volume], Time)
			values (s.SymbolID, s.Date, s.[Open],	s.[High] ,s.[Low],s.[Close] ,s.[Volume], s.Time)
	when matched and (t.[Open]<> s.[Open] or t.High <> s.High or t.Low <> s.Low or t.[Close]<> s.[Close] or t.Volume <> s.Volume) then
		update set t.[Open]=s.[Open], t.High = s.High , t.Low = s.Low , t.[Close] = s.[Close] , t.Volume = s.Volume
	;
	;with s as
	(
		select Exchange, Symbol, 
			MAX(case when IntervalID = 0 then Date end) MinutePriceLastUpdate, 
			MAX(case when IntervalID = 6 then Date end) DayPriceLastUpdate,
			MIN(case when IntervalID = 0 then Date end) MinutePriceFirstUpdate, 
			MIN(case when IntervalID = 6 then Date end) DayPriceFirstUpdate  
		from [EODData].[Quote] group by Exchange, Symbol, IntervalID
	)
	update t
		set t.DayPriceLastUpdate = case when isnull(t.DayPriceLastUpdate, '1900-01-01')< s.DayPriceLastUpdate then s.DayPriceLastUpdate else t.DayPriceLastUpdate end,
			t.MinutePriceLastUpdate = case when isnull(t.MinutePriceLastUpdate, '1900-01-01') < s.MinutePriceLastUpdate then s.MinutePriceLastUpdate else t.MinutePriceLastUpdate end,
			t.DayPriceFirstUpdate = case when t.DayPriceFirstUpdate is null then s.DayPriceFirstUpdate else t.DayPriceFirstUpdate end,
			t.MinutePriceFirstUpdate = case when t.MinutePriceFirstUpdate is null then s.MinutePriceFirstUpdate else t.MinutePriceFirstUpdate end
	from [$(Quote)].q.Symbol t
		inner join s on t.Exchange = s.Exchange and t.Symbol = s.Symbol
	where isnull(t.DayPriceLastUpdate, '1900-01-01')< s.DayPriceLastUpdate
		or isnull(t.MinutePriceLastUpdate, '1900-01-01') < s.MinutePriceLastUpdate
		or t.MinutePriceFirstUpdate is null
		or t.DayPriceFirstUpdate is null
end
go