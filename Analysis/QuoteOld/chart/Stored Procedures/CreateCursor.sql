CREATE procedure [chart].[CreateCursor] (@Algorithm xml, @CursorName varchar(128) = null output, @ForceCreation bit = 0)
as
begin
	set nocount on
	select @CursorName = chart.GetCursorName(@Algorithm)
	declare @CreationDate datetime = null, @SQL nvarchar(max)
	exec chart.MonitorEnter @CursorName;
	select @CreationDate = create_date from tempdb.sys.tables where name = @CursorName
	if @CreationDate is null  -- object does not exists
	begin
		exec dbo.CLRGetBasicChartTableTarget @Algorithm
	end
	else if @ForceCreation = 1 or @CreationDate is not null and datediff(minute, @CreationDate, getdate()) > 300 -- exists but cache is too old, created 5 hours ago
	begin
		exec('drop table [tempdb].dbo.' + @CursorName)
		exec dbo.CLRGetBasicChartTableTarget @Algorithm
	end
	insert into chart.Cache(CursorName, Algorithm)
		select @CursorName, @Algorithm
		where not exists(select * from chart.Cache where CursorName = @CursorName)
	exec chart.MonitorExit @CursorName;
end
