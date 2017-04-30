create procedure q.AddToWatchList @SymbolID int
as
begin
	set nocount on
	insert into q.WatchList(SymbolID) values(@SymbolID)
end