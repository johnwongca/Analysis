CREATE procedure [q].[ImportStaging_Split]
(
	@FromVersion binary(8) = 0x00, 
	@ToVersion binary(8) = 0xFF0000001001372E
)
as
begin
	merge q.Split t
	using (
			select 
					b.SymbolID, a.Date, a.Ratio
			from EODData.staging.Split a
					inner join q.ExchangeSymbol b with (noexpand) on a.ExchangeID = b.Exchange and a.SymbolID = b.Symbol
			where a.___DataVersion___ > @FromVersion
				and a.___DataVersion___ <= @ToVersion
			) s on t.SymbolID = s.SymbolID and t.Date = s.Date
	when Matched and (t.Ratio <> s.Ratio) then
		update
			set t.Ratio = s.Ratio
	when not matched then
		insert (SymbolID, Date, Ratio)
			values(s.SymbolID, s.Date, s.Ratio)
	;
	return @@rowcount
end
