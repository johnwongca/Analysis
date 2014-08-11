create function q.GetSymbolExchange(@SymbolID int)
returns varchar(10)
as
begin
	return (
				select e.Exchange
				from q.Exchange e
					inner join q.Symbol s on s.ExchangeID = s.ExchangeID
				where s.SymbolID = @SymbolID
			)
end
