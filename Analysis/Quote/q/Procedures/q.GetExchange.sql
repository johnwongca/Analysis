create procedure q.GetExchange
as
begin
	select Exchange, ExchangeName, Country, Currency, TimeZone from q.Exchange order by Exchange
end