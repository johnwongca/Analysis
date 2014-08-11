create view Chart.ChartList
as
with AllChartUsers as
(
	select ChartUserID 
	from chart.ChartUser 
	where LoginName in (cast(system_user as varchar(128)), 'Default')
),
ExceptChartIDs as
(
	select ParentChartID
	from chart.Chart c
		inner join chart.ChartUser u on u.ChartUserID = c.ChartUserID
	where c.ParentChartID is not null
		and u.LoginName in (cast(system_user as varchar(128)))
)
	select	c.ChartID, c.ChartUserID, c.ChartName, 
			e.ExchangeID, e.Exchange, c.SymbolID, 
			s.Symbol, c.SourceIntervalID, c.DisplayStartID,
			c.DisplayRange, c.Algorithm, c.ChartDefinition
	from chart.Chart c 
		inner join q.Symbol s on s.SymbolID = c.SymbolID
		inner join q.Exchange e on e.ExchangeID = s.ExchangeID
		inner join AllChartUsers u on u.ChartUserID = c.ChartUserID
	where not exists(select * from ExceptChartIDs ex where ex.ParentChartID = c.ChartID)