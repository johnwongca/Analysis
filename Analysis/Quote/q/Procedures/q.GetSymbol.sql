create procedure q.GetSymbol
as
begin
	select SymbolID, Exchange, Symbol, SymbolName, LongName, DayPriceLastUpdate, MinutePriceLastUpdate, DayPriceFirstUpdate, MinutePriceFirstUpdate
	from q.Symbol s
	where MinutePriceLastUpdate > dateadd(day, -7, getdate())
		or DayPriceLastUpdate  > dateadd(day, -7, getdate())
	order by Exchange, Symbol
end