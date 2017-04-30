CREATE procedure [q].[GetExchangeSymbol]
as
begin
	select Exchange, ExchangeName, Country, Currency, TimeZone from q.Exchange order by Exchange
	--select SymbolID, Exchange, Symbol, SymbolName, LongName, DayPriceLastUpdate, MinutePriceLastUpdate, DayPriceFirstUpdate, MinutePriceFirstUpdate
	--from q.Symbol
	--order by Exchange, Symbol
	select s.SymbolID, s.Exchange, s.Symbol, s.SymbolName, s.LongName, s.DayPriceLastUpdate, s.MinutePriceLastUpdate, s.DayPriceFirstUpdate, s.MinutePriceFirstUpdate,
		f.Dividend, f.DividendDate, f.DividendYield, f.DPS, f.EBITDA, f.MarketCap, f.EPS, f.PtS, f.NTA, f.PE, f.PEG, f.PtB, f.Shares, f.Yield
	from q.Symbol s
		inner join q.Fundamental f on s.SymbolID = f.SymbolID and f.ExpiryDate is null and f.MarketCap> 500000000
	where s.MinutePriceLastUpdate > dateadd(day, -7, getdate())
		or s.DayPriceLastUpdate  > dateadd(day, -7, getdate())
	order by s.Exchange, s.Symbol
end