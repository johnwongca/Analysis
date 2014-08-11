CREATE procedure chart.SaveChart
(
	@ChartID int output,
	@ChartName varchar(128),
	@SymbolID int,
	@SourceIntervalID int,
	@DisplayStartID bigint,
	@DisplayRange int,
	@Algorithm xml,
	@ChartDefinition xml,
	@Action varchar(20) -- Save, SaveAsUserChart, SaveAsTemplate
)
as
begin
	set nocount on

	If (@Action = 'Save')
	begin
		update chart.Chart
			set ChartName = @ChartName, 
				SymbolID = @SymbolID, 
				SourceIntervalID = @SourceIntervalID, 
				DisplayStartID = @DisplayStartID, 
				DisplayRange = @DisplayRange, 
				Algorithm = @Algorithm, 
				ChartDefinition = @ChartDefinition
		where ChartID = @ChartID
		return
	end

	Declare @ChartUserID int, @ParentChartID int

	insert into chart.ChartUser(LoginName)
		select rtrim(cast(system_user as varchar(128)))
		where not exists(select 1 from chart.ChartUser where LoginName = rtrim(cast(system_user as varchar(128))))

	select @ChartUserID = ChartUserID
	from chart.ChartUser
	where LoginName = rtrim(cast(system_user as varchar(128)))

	select @ParentChartID = isnull(ParentChartID, ChartID)
	from chart.Chart
	where ChartID = @ChartID

	if(@Action = 'SaveAsTemplate')
		select @ChartID = null, @ChartUserID = -1;
	else
		select @ChartID = @ParentChartID
	insert into chart.Chart(ParentChartID, ChartUserID, ChartName, SymbolID, SourceIntervalID, DisplayStartID, DisplayRange, Algorithm, ChartDefinition)
		values(@ChartID, @ChartUserID, @ChartName, @SymbolID, @SourceIntervalID, @DisplayStartID, @DisplayRange, @Algorithm, @ChartDefinition)
	select @ChartID = scope_identity()
end