create procedure q.CursorSize(@CursorName varchar(128))
as
begin
	declare @SQL nvarchar(max), @CursorSize int
	if object_id('tempdb.dbo.'+quotename(@CursorName)) is null
		return -1;

	select @SQL = 'select @CursorSize = max(___RowNumber___) from tempdb.dbo.'+quotename(@CursorName)
	exec sp_executesql @SQL, N'@CursorSize int output', @CursorSize output
	return @CursorSize
end
go
