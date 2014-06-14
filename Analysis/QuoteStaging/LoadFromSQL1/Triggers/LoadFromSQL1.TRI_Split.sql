CREATE TRIGGER [LoadFromSQL1].[TRI_Split] ON [LoadFromSQL1].[Split]
instead of delete, insert, update
as
begin
	set nocount on
	;merge [$(Quote)].q.Split as t
	using inserted as s on s.SymbolID= t.SymbolID and s.Date = t.Date
	when not matched then
		insert (SymbolID, Date, Ratio)
		 values (s.SymbolID, s.Date, s.Ratio)
	when matched and (s.Ratio <> t.Ratio) then
		update set t.Ratio = s.Ratio
	;
end
