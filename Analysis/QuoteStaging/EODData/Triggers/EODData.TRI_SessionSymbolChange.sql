create trigger EODData.TRI_SessionSymbolChange on EODData.SessionSymbolChange
instead of insert
AS 
BEGIN 
	set nocount on

	insert into EODData.SessionSymbolChange(Date, FromExchange, FromSymbol, ToExchange, ToSymbol)
	select s.Date, s.FromExchange, s.FromSymbol, s.ToExchange, s.ToSymbol
	from inserted s
	where not exists(select * 
					from EODData.SessionSymbolChange t
					where t.Date = s.Date
						and t.FromExchange = s.FromExchange
						and t.FromSymbol = s.FromSymbol
						and t.ToExchange = s.ToExchange 
						and t.ToSymbol = s.ToSymbol
					)
		
END