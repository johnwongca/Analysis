create procedure q.GetChart
as
begin
	set nocount on
	select ChartName, ChartDefinition 
	from q.Chart 
	order by ChartName
end