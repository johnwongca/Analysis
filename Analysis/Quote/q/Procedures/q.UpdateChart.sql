create procedure q.UpdateChart
(
	@ChartName varchar(128),
	@AlgorithmName varchar(256),
	@SymbolID int,
	@IntervalType varchar(20),
	@Interval int,
	@ChartDefinition xml
)
as
begin
	set nocount on
	update q.Chart
		set AlgorithmName = @AlgorithmName, 
			SymbolID = @SymbolID,
			IntervalType = @IntervalType,
			Interval = Interval,
			ChartDefinition = @ChartDefinition
	where ChartName = @ChartName
	if @@rowcount = 0
		insert into q.Chart(ChartName, AlgorithmName, SymbolID, IntervalType, Interval, ChartDefinition)
			values(@ChartName, @AlgorithmName, @SymbolID, @IntervalType, @Interval, @ChartDefinition)
end