CREATE procedure q.GetWatchList
as
begin
	set nocount on
	select s.Symbol, q1.Date PriceDate, cast(q1.[Close] as money) Price, cast(q2.[Close]  as money) PreviousPrice, cast(q1.[Close] - q2.[Close] as money) [Difference] ,s.Exchange, s.SymbolName, w.SymbolID, w.CreationDate
	from q.WatchList w
		 inner join q.Symbol s on s.SymbolID = w.SymbolID
		 cross apply (
							select top 1 q.[Close], q.Date
							from  q.Quote q
							where q.SymbolID = w.SymbolID
							order by q.Date desc
						) q1
		cross apply (
						select [Close]
						from(
							select q.[Close],  row_number() over(order by q.Date desc) RowNumber
							from  q.Quote q
							where q.SymbolID = w.SymbolID
							) q1
						where q1.RowNumber = 2
						) q2
end