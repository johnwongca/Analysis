CREATE procedure [chart].[CursorFetch](@CursorName varchar(128), @StartLocation int, @NumberOfRows int)
as
begin
	declare @SQL nvarchar(max), @Algorithm xml, @CursorSize int
	select	@NumberOfRows = isnull(@NumberOfRows, 0),
			@StartLocation = isnull(@StartLocation, 0)
	select	@CursorName = CursorName, 
			@Algorithm = Algorithm 
	from chart.Cache where CursorName = @CursorName
	if @@rowcount = 0
	begin
		raiserror('Could not find cursor %s', 16, 1, @CursorName)
		return;
	end
	exec chart.MonitorEnter @CursorName;
	exec chart.CreateCursor @Algorithm, null
	exec @CursorSize = chart.GetCursorSize @CursorName
	if @CursorSize < @StartLocation + @NumberOfRows - 1
		select @StartLocation = @CursorSize  - @NumberOfRows + 1
	if(@StartLocation < 1)
		select @StartLocation = 1
	select @SQL = 'select * from tempdb.dbo.' + quotename(@CursorName) + 'where ___ID___ >= @StartLocation and ___ID___ < @StartLocation + @NumberOfRows order by ___ID___'
	exec sp_executesql @SQL, N'@StartLocation int, @NumberOfRows int', @StartLocation, @NumberOfRows
	exec chart.MonitorExit @CursorName;

	return @StartLocation
end
