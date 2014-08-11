CREATE procedure [q].[ImportStaging_Exchange]
(
	@FromVersion binary(8) = 0x00, 
	@ToVersion binary(8) = 0x000000000FFE6410
)
as
begin
	merge q.Exchange t
	using (
			select ExchangeID, Name, CountryID, Currency, TimeZone
			from EODData.staging.Exchange
			where ___DataVersion___ > @FromVersion
				and ___DataVersion___ <= @ToVersion
			) s on t.Exchange = s.ExchangeID
	when Matched and (t.ExchangeName <> s.Name or isnull(t.CountryID,'') != isnull(s.CountryID, '') or isnull(t.Currency, '') <> isnull(s.Currency, '' ) or isnull(t.TimeZone, '') <> isnull(s.TimeZone, '')) then
		update
			set t.ExchangeName = s.Name,
				t.CountryID = s.CountryID,
				t.Currency = s.Currency,
				t.TimeZone = s.TimeZone
	when not matched then
		insert (Exchange, ExchangeName, CountryID, Currency, TimeZone)
			values(s.ExchangeID, s.Name, s.CountryID, s.Currency, s.TimeZone)
	;
	return @@rowcount
end
