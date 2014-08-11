create procedure q.RetrieveExchangeSymbol(@ExchangeID int)
as
begin
	select * 
	from q.Symbol
	where ExchangeID = @ExchangeID
end
