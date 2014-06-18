create procedure EODData.GetCountry(@PoolID smallint = @@spid)
as
begin
	set nocount on
	insert into EODData.Task(
								PoolID, MethodName, IntervalID, 
								Exchange, Symbol, DateFrom, 
								DateTo
							)
		select	@PoolID PoolID, 'GetCountries' MethodName, null IntervalID, 
				null Exchange, null Symbol, null DateFrom, 
				null DateTo
	declare @TaskID int = scope_identity()
	exec EODData.WaitForResult @PoolID, @TaskID
end