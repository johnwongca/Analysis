create procedure q.RemoveFromWatchList @SymbolID int
as
begin
	set nocount on
	delete q.WatchList where SymbolID = @SymbolID
end