create trigger LoadFromSQL1.TRI_Symbol on LoadFromSQL1.Symbol
instead of delete, insert, update
as
begin
	set nocount on
	set identity_insert [$(Quote)].q.Symbol on
	;merge [$(Quote)].q.Symbol t
	using inserted s on s.SymbolID = t.SymbolID
	when not matched then
		insert(SymbolID, Exchange, Symbol, SymbolName, LongName)
			values(s.SymbolID, s.Exchange, s.Symbol, s.SymbolName, s.SymbolLongName)
	when matched  and (
								t.Exchange<> s.Exchange
							or t.Symbol<> s.Symbol
							or t.SymbolName<> s.SymbolName
							or isnull(t.LongName,'')<> isnull(s.SymbolLongName, '')
						)
	then
		update set t.Exchange = s.Exchange,
					t.Symbol = s.Symbol,
					t.SymbolName = s.SymbolName,
					t.LongName = s.SymbolLongName
	;
	set identity_insert [$(Quote)].q.Symbol off	
end
