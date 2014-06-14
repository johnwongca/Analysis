create trigger LoadFromSQL1.TRI_QuoteMinute on LoadFromSQL1.QuoteMinute
instead of delete, insert, update
as
begin
	set nocount on
	;merge [$(Quote)].q.QuoteMinute as t
		using inserted as s on s.SymbolID= t.SymbolID and s.Date = t.Date and s.Time = t.Time
		when not matched then
			insert (SymbolID, Date, [Open],	[High] ,[Low],[Close] ,[Volume], Time)
			 values (s.SymbolID, s.Date, s.[Open],	s.[High] ,s.[Low],s.[Close] ,s.[Volume], s.Time)
		when matched and (t.[Open]<> s.[Open] or t.High <> s.High or t.Low <> s.Low or t.[Close]<> s.[Close] or t.Volume <> s.Volume) then
			update set t.[Open]=s.[Open], t.High = s.High , t.Low = s.Low , t.[Close] = s.[Close] , t.Volume = s.Volume
		;
end
