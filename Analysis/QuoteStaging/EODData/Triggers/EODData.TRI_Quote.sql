create trigger EODData.TRI_Quote on EODData.vQuote
instead of insert
AS 
BEGIN 
	set nocount on
	; with s0 as
	(	
		select Exchange, Symbol, IntervalID, Date, [Open], [Close], High, Low, Volume, Ask, Bid, OpenInterest , row_number() over( partition by Exchange, Symbol, IntervalID, Date order by Exchange, Symbol, IntervalID, Date) RowNumber
		from inserted
	)
	merge EODData.Quote t
	using (select * from s0 where RowNumber = 1) s on t.Symbol = s.Symbol and t.Date = s.Date and t.Exchange = s.Exchange and t.IntervalID = s.IntervalID
	when matched and (
							t.[Open] <> s.[Open]
						or t.[Close] <> s.[Close]
						or t.[High] <> s.High
						or t.[Low] <> s.Low
						or t.[Volume] <> s.Volume
						or t.[Ask] <> s.Ask
						or t.[Bid] <> s.Bid
						or t.[OpenInterest] <> s.OpenInterest
						) then
		update set [Open] = s.[Open], [Close] = s.[Close], [High] = s.High,
					[Low] = s.Low, [Volume] = s.Volume, [Ask] = s.Ask,
					[Bid] = s.Bid, [OpenInterest] = s.OpenInterest
	when not matched then
		insert ([Exchange], [Symbol], IntervalID, [Date], [Open], [Close], [High], [Low], [Volume], [Ask], [Bid], [OpenInterest])
			values(s.Exchange, s.Symbol, s.IntervalID, s.Date, s.[Open], s.[Close], s.High, s.Low, s.Volume, s.Ask, s.Bid, s.OpenInterest)
	option(loop join);
END