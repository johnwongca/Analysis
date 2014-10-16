create procedure q.CursorRemove(@CursorName varchar(128))
as
begin
	declare @SQL nvarchar(max), @CursorSize int
	if object_id('tempdb.dbo.'+quotename(@CursorName)) is not null
	begin
		select @SQL = 'drop table tempdb.dbo.'+quotename(@CursorName)
		exec(@SQL)
	end
end
go
