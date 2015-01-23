


CREATE view [trans].[InventoryCurrent]
as
	select InventoryID, SymbolID, Exchange, Symbol, Position, IsShort, Rate,Cost, sum(Cost) over()  TotalCost
	from trans.Inventory 
	where CloseDate is null