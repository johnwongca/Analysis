CREATE function q.GetExchange(@ExchangeID int)
returns varchar(10)
as
begin
	return (select Exchange from q.Exchange where ExchangeID = @ExchangeID)
end