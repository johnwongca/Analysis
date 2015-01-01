

CREATE view [trans].[Inventoryinput] 
as 
select s.Exchange, s.Symbol, 
	Date, Quantity, Price, Commission
from trans.Inventory i
	left join q.Symbol s on s.SymbolID = i.SymbolID
GO
CREATE trigger [trans].[TRI_Inventoryinput] on [trans].[Inventoryinput]
instead of insert, delete, update
as
begin
	if @@rowcount = 0
		return
	set nocount on
	delete b
	from trans.Inventory b
		inner join q.Symbol s on s.SymbolID = b.SymbolID
		inner join deleted d on d.Exchange = s.Exchange and d.Symbol = s.Symbol 
	where d.Date = b.Date
	insert into trans.Inventory(SymbolID, Date, Quantity, Price, Commission, Position, Rate, Cost, Total)
		select s.SymbolID, i.Date, i.Quantity, i.Price, i.Commission, 0, 0, 0, 0
		from inserted i
			inner join q.Symbol s on s.Symbol = i.Symbol and s.Exchange = i.Exchange
	-- recalculate inventory
	declare @SymbolID int
	declare c cursor local static for
		select s.SymbolID
		from inserted a
			inner join q.Symbol s on a.Exchange = s.Exchange and a.Symbol = s.Symbol 
		union 
		select s.SymbolID
		from deleted a
			inner join q.Symbol s on a.Exchange = s.Exchange and a.Symbol = s.Symbol 
	open c
	fetch next from c into @SymbolID
	while @@fetch_status = 0
	begin
		exec trans.CalculateInventory @SymbolID
		fetch next from c into @SymbolID
	end
	close c
	deallocate c
	return
end