create procedure q.RetrieveSymbolSplit(@SymbolID int)
as
begin
	select a.*
	from q.Split a
	where a.SymbolID = @SymbolID
end
