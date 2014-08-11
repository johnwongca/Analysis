create procedure q.RetrieveSymbol(@ExchangeID int)
as
begin
	select a.*
	from q.Symbol a
	where a.ExchangeID = @ExchangeID
end
