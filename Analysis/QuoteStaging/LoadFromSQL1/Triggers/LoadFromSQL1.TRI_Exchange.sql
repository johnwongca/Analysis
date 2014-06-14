create trigger LoadFromSQL1.TRI_Exchange ON LoadFromSQL1.Exchange
instead of delete, insert, update
as
begin
	set nocount on
	;merge [$(Quote)].q.Exchange as t
	using inserted as s on s.Exchange = t.Exchange
	when not matched then
		insert (Exchange, ExchangeName, Country, Currency, TimeZone)
		 values (s.Exchange, s.ExchangeName, s.Country, s.Currency, s.TimeZone)
	when matched and (
								isnull(s.ExchangeName, '')<>isnull(t.ExchangeName, '')
							or isnull(s.Country, '')<>isnull(t.Country, '')
							or isnull(s.Currency, '')<>isnull(t.Currency, '')
							or isnull(s.TimeZone, '')<>isnull(t.TimeZone, '')
					) then
		update set t.ExchangeName = s.ExchangeName, t.Country = s.Country, t.Currency = s.Currency, t.TimeZone = s.TimeZone
	;
end
