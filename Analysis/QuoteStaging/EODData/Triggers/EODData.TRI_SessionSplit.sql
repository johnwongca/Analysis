create trigger EODData.TRI_SessionSplit on EODData.SessionSplit
instead of insert
AS 
BEGIN 
	set nocount on
	;merge EODData.SessionSplit t
	using inserted s on t.Exchange = s.Exchange and t.Symbol = s.Symbol and t.Date = s.Date
	when matched and t.Ratio <> s.Ratio then
		update set Ratio = s.Ratio
	when not matched then 
		insert (Exchange, Symbol, Date, Ratio)
			values(s.Exchange, s.Symbol, s.Date, s.Ratio)
	;
END