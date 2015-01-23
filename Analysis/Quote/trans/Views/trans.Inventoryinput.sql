


CREATE view [trans].[Inventoryinput] 
as 
select i.Exchange, i.Symbol, 
	id.Date, id.Quantity, IsSell, id.Price, id.Commission
from trans.Inventory i
	inner join trans.InventoryDetail id on i.InventoryID = id.InventoryID
where i.CloseDate is null
GO
CREATE trigger [trans].[TRI_Inventoryinput] on [trans].[Inventoryinput]
instead of insert, delete, update
as
begin
	if @@rowcount <> 1
	begin
		raiserror('Only one record can be updated at a time', 16, 1)
		rollback
		return
	end
	set nocount on
	
	
	insert into trans.Inventory(SymbolID, Exchange, Symbol)
		select s.SymbolID, s.Exchange, s.Symbol
		from inserted i
			inner join q.Symbol s on s.Symbol = i.Symbol and s.Exchange = i.Exchange
		where not exists(select * from trans.Inventory v where v.CloseDate is null and v.SymbolID = s.SymbolID)
	-- recalculate inventory
	delete id
	from trans.InventoryDetail id
		inner join trans.Inventory v on v.InventoryID = id.InventoryID
		inner join deleted d on d.Exchange = v.Exchange and d.Symbol = v.Symbol and d.Date = id.Date
	where v.CloseDate is null

	insert into trans.InventoryDetail(InventoryID, Date, IsSell, Quantity, Price, Commission)
		select v.InventoryID, i.Date, i.IsSell, i.Quantity, i.Price, i.Commission
		from trans.Inventory v 
			inner join inserted i on i.Exchange = v.Exchange and i.Symbol = v.Symbol
			inner join q.Symbol s on s.Exchange = i.Exchange and s.Symbol = i.Symbol
	declare @InventoryID int
	declare c cursor local static for
		select v.InventoryID
		from (
				select isnull(i.Exchange, d.Exchange) Exchange, isnull(i.Symbol, d.Symbol) Symbol
				from inserted i
					full outer join deleted d on i.Symbol = d.Symbol and i.Exchange = d.Exchange
			)a
				inner join trans.Inventory v on v.Symbol = a.Symbol and a.Exchange = a.Exchange
		where v.CloseDate is null
	open c
	fetch next from c into @InventoryID
	while @@fetch_status = 0
	begin
		exec trans.CalculateInventory @InventoryID, 0
		fetch next from c into @InventoryID
	end
	close c
	deallocate c
	return
end