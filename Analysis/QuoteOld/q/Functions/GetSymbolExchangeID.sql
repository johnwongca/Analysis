create function q.GetSymbolExchangeID(@SymbolID int)
returns int
as
begin
	return (
				select e.ExchangeID
				from q.Exchange e
					inner join q.Symbol s on s.ExchangeID = s.ExchangeID
				where s.SymbolID = @SymbolID
			)
end
