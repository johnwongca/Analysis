create trigger EODData.TRI_Symbol on EODData.vSymbol
instead of insert
AS 
BEGIN 
	set nocount on
	;merge EODData.Symbol t
	using inserted s on t.Exchange = s.Exchange and t.Symbol = s.Symbol
	when matched and (
							isnull(t.Name, '') <> isnull (s.Name, '')
						or isnull(t.LongName, '') <> isnull(s.LongName, '')
						or isnull(t.Date, '1960-01-01') <> isnull(s.Date, '1960-01-01')
						) then
		upDate set Name = s.Name,
			LongName = s.LongName,
			Date = s.Date
	when not matched then
		insert (Exchange, Symbol, Name, LongName, Date)
			values(s.Exchange, s.Symbol, s.Name, s.LongName, s.Date)
	option(loop join);
END