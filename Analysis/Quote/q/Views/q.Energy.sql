create view q.Energy
as
select s.Symbol, n.Name, f.*
from q.Symbol s
	inner join q.Fundamental f on f.SymbolID = s.SymbolID and f.ExpiryDate is null
	inner join q.Name n on n.NameID = f.Industry
where s.Exchange = 'NASDAQ'
	--and s.Symbol = 'line'
	and Sector = 26
	and f.MarketCap > 500000000
--order by 2,1	




--select * from q.Name where NameID in (26, 60, 61, 690, 40, 693, 45, 54, 24, 54)