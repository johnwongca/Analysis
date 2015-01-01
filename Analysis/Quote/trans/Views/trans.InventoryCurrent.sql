
create view [trans].[InventoryCurrent]
as
	select SymbolID, Exchange, Symbol,
		Date, Quantity, Price,
		Total, Commission TotalCommission, Position,
		Rate, Cost
	from (
	select	
		s.SymbolID, s.Exchange, s.Symbol,
		i.Date, i.Quantity, i.Price,
		i.Total, sum(i.Commission) over() Commission, i.Position,
		i.Rate, i.Cost,row_number()over(partition by s.SymbolID order by i.Date desc) RowNumber
  from trans.Inventory i
		inner join quote.q.Symbol s on s.SymbolID = i.SymbolID
	) d
	where d.RowNumber = 1
--  order by s.Symbol, Date desc