create procedure q.GetChart
as
begin
	set nocount on
	select ChartName, AlgorithmName, SymbolID, IntervalType, Interval, ChartDefinition 
	from q.Chart 
	order by ChartName
end