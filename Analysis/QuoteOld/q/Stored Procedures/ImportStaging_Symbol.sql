CREATE procedure [q].[ImportStaging_Symbol]
(
	@FromVersion binary(8) = 0x00, 
	@ToVersion binary(8) = 0x000000000FFE6410
)
as
begin
	merge q.Symbol t
	using (
			select b.ExchangeID, a.SymbolID as Symbol, isnull(a.Name, 'Unkown') SymbolName, a.LongName
			from EODData.staging.Symbol a
					left outer join q.Exchange b on a.ExchangeID = b.Exchange
			where a.___DataVersion___ > @FromVersion
				and a.___DataVersion___ <= @ToVersion
			) s on t.ExchangeID = s.ExchangeID and t.Symbol = s.Symbol
	when Matched and (t.SymbolName <> s.SymbolName or isnull(t.LongName,'') <> isnull(s.LongName, '')) then
		update
			set t.SymbolName = s.SymbolName,
				t.LongName = s.LongName
	when not matched then
		insert (ExchangeID, Symbol, SymbolName, LongName)
			values(s.ExchangeID, s.Symbol, s.SymbolName, s.LongName)
	;
	return @@rowcount
end
