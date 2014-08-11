create procedure [chart].[GetCursorSize](@CursorName varchar(128))
as
begin
	declare @r int, @SQL nvarchar(max)
	select @SQL = 'select @r = max(___ID___) from tempdb.dbo.'+ @CursorName
	exec sp_executesql @SQL, N'@r int output', @r output
	return @r
end 
