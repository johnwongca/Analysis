create view trans.vInventory
as
	select	
		s.SymbolID, s.Exchange, s.Symbol,
		i.Date, i.Quantity, i.Price,
		i.Total, i.Commission, i.Position,
		i.Rate, i.Cost,sum(i.Total)over() Cash
  from trans.Inventory i
		inner join quote.q.Symbol s on s.SymbolID = i.SymbolID
--  order by s.Symbol, Date desc