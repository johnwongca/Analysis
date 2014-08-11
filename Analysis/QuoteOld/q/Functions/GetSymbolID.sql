create function q.GetSymbolID(@Exchange varchar(10), @Symbol varchar(10))
returns int
as
begin
	return (
				select s.SymbolID
				from q.Exchange e
					inner join q.Symbol s on s.ExchangeID = s.ExchangeID
				where e.Exchange = @Exchange 
					and s.Symbol = @Symbol
			)
end