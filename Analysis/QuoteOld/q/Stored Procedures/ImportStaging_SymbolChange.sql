CREATE procedure q.ImportStaging_SymbolChange
(
	@FromVersion binary(8) = 0x00, 
	@ToVersion binary(8) = 0xFF0000001001372E
)
as
begin
	merge q.SymbolChange t
	using (
			select 
					a.date, b.SymbolID as FromSymbolID, c.SymbolID as ToSymbolID
			from EODData.staging.SymbolChange a
					inner join q.ExchangeSymbol b with (noexpand) on a.FromExchangeID = b.Exchange and a.FromSymbolID = b.Symbol
					inner join q.ExchangeSymbol c with (noexpand) on a.ToExchangeID = c.Exchange and a.ToSymbolID = c.Symbol
			where a.___DataVersion___ > @FromVersion
				and a.___DataVersion___ <= @ToVersion
			) s on t.FromSymbolID = s.FromSymbolID and t.Date = s.Date and t.ToSymbolID = s.ToSymbolID
	when not matched then
		insert (FromSymbolID, ToSymbolID, Date)
			values(FromSymbolID, ToSymbolID, Date)
	;
	return @@rowcount
end
