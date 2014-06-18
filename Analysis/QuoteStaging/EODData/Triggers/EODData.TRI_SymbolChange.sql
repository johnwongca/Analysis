create trigger EODData.TRI_SymbolChange on EODData.vSymbolChange
instead of insert
AS 
BEGIN 
	set nocount on

	insert into EODData.SymbolChange(Date, FromExchange, FromSymbol, ToExchange, ToSymbol)
	select cast(s.Date as date), s.FromExchange, s.FromSymbol, s.ToExchange, s.ToSymbol
	from inserted s
	where not exists(select * 
					from EODData.SymbolChange t
					where t.Date = cast(s.Date as date)
						and t.FromExchange = s.FromExchange
						and t.FromSymbol = s.FromSymbol
						and t.ToExchange = s.ToExchange 
						and t.ToSymbol = s.ToSymbol
					)
	option(loop join);
END