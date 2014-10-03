create procedure q.GetSymbol
as
begin
	select SymbolID, Exchange, Symbol, SymbolName, LongName, DayPriceLastUpdate, MinutePriceLastUpdate, DayPriceFirstUpdate, MinutePriceFirstUpdate
	from q.Symbol
	order by Exchange, Symbol
end