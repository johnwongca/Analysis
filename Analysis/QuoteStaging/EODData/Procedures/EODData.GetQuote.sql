CREATE PROCEDURE [EODData].[GetQuote](@Exchange varchar(10),@Date date, @PoolID smallint = @@spid)
AS
begin
	set nocount on
	insert into EODData.Task(
								PoolID, MethodName, IntervalID, 
								Exchange, Symbol, DateFrom, 
								DateTo
							)
		select	@PoolID PoolID, 'GetQuotes' MethodName, null IntervalID, 
				@Exchange Exchange, null Symbol, @Date DateFrom, 
				null DateTo
	declare @TaskID int = scope_identity()
	exec EODData.WaitForResult @PoolID, @TaskID
end