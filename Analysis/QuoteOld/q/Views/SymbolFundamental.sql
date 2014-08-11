

CREATE view [q].[SymbolFundamental] 
as
	with x as
	(
		select f.SymbolID, MAX(f.Date) Date
		from q.Fundamental f
		group by f.SymbolID
	)
	select	/*e.ExchangeID, e.Exchange,*/ f1.SymbolID, 
			/*s.Symbol,*/ f1.Date, f1.Sector, 
			f1.Industry, f1.Dividend, f1.DividendDate, 
			f1.DividendYield, f1.DSP, f1.EBITDA, 
			f1.MarketCap, f1.EPS, f1.PtS, 
			f1.NTA, f1.PE, f1.PEG, 
			f1.PtB, f1.Shares, f1.Yield
	from q.Fundamental f1
		--inner join q.Symbol s on s.SymbolID = f1.SymbolID
		--inner join q.Exchange e on e.ExchangeID = s.ExchangeID
	where  exists(select 1 from x where f1.SymbolID = x.SymbolID and f1.Date = x.Date)


