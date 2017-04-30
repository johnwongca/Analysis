



CREATE view [trans].[InventorySummary]
as
	select sum(GrossEarn) GrossEarn, SUM(Commission) Commission, sum(NetEarn) NetEarn
	from trans.Inventory 
	where CloseDate is not null