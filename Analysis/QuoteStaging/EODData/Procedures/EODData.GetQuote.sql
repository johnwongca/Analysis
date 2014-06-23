CREATE PROCEDURE [EODData].[GetQuote](@Exchange varchar(10), @Interval tinyint, @Date date, @PoolID smallint = @@spid)
AS
begin
	set nocount on
	insert into EODData.Task(
								PoolID, MethodName, IntervalID, 
								Exchange, Symbol, DateFrom, 
								DateTo
							)
		select	@PoolID PoolID, 'GetQuotes' MethodName, @Interval IntervalID, 
				@Exchange Exchange, null Symbol, @Date DateFrom, 
				null DateTo
	declare @TaskID int = scope_identity()
	exec EODData.WaitForResult @PoolID, @TaskID
end