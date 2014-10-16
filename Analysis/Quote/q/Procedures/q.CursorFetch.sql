create procedure q.CursorFetch(@CursorName varchar(128), @StartLocation int = 1, @NumberOfRows int = 10)
as
begin
	declare @SQL nvarchar(max), @CursorSize int
	select	@NumberOfRows = isnull(@NumberOfRows, 1),
			@StartLocation = isnull(@StartLocation, 1)
	
	if object_id('tempdb.dbo.'+quotename(@CursorName)) is null
		return -1;

	select @SQL = 'select @CursorSize = max(___RowNumber___) from tempdb.dbo.'+quotename(@CursorName)
	exec sp_executesql @SQL, N'@CursorSize int output', @CursorSize output
	select @CursorSize = isnull(@CursorSize, 0);
	if @StartLocation <=0
		select @StartLocation = 200000000
	if @CursorSize < @StartLocation + @NumberOfRows - 1
		select @StartLocation = @CursorSize  - @NumberOfRows + 1
	if(@StartLocation < 1)
		select @StartLocation = 1
	select @SQL = 'select * from tempdb.dbo.' + quotename(@CursorName) + 'where ___RowNumber___ >= @StartLocation and ___RowNumber___ < @StartLocation + @NumberOfRows order by ___RowNumber___'
	exec sp_executesql @SQL, N'@StartLocation int, @NumberOfRows int', @StartLocation, @NumberOfRows
	return @StartLocation
end
go
