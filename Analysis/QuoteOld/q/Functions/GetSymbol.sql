create function q.GetSymbol(@SymbolID int)
returns varchar(10)
as
begin
	return (
				select s.Symbol
				from q.Symbol s 
				where s.Symbol = @SymbolID
			)
end
