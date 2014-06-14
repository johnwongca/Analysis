create trigger LoadFromSQL1.TRI_SymbolChange on LoadFromSQL1.SymbolChange
instead of delete, insert, update
as
begin
	set nocount on
	;merge [$(Quote)].[q].[SymbolChange] as t
	using inserted s on  s.Date = t.Date and s.FromSymbolID = t.FromSymbolID and s.ToSymbolID = t.ToSymbolID
	when not matched then
		insert(Date, FromSymbolID, ToSymbolID)
			values(s.Date, s.FromSymbolID, s.ToSymbolID)
	when not matched by source then
		delete;
end
