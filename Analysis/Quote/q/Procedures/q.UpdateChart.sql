create procedure q.UpdateChart
(
	@ChartName varchar(128),
	@ChartDefinition xml
)
as
begin
	set nocount on
	update q.Chart
		set ChartDefinition = @ChartDefinition
	where ChartName = @ChartName
	if @@rowcount = 0
		insert into q.Chart(ChartName, ChartDefinition)
			values(@ChartName, @ChartDefinition)
end