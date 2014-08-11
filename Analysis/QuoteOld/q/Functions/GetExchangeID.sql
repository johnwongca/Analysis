create function q.GetExchangeID(@Exchange varchar(10))
returns int
as
begin
	return (select ExchangeID from q.Exchange where Exchange = @Exchange)
end