﻿CREATE PROCEDURE [EODData].[GetFundamental](@Exchange varchar(10), @PoolID smallint = @@spid)
AS
begin
	set nocount on
	insert into EODData.Task(
								PoolID, MethodName, IntervalID, 
								Exchange, Symbol, DateFrom, 
								DateTo
							)
		select	@PoolID PoolID, 'GetFundamentals' MethodName, null IntervalID, 
				@Exchange Exchange, null Symbol, null DateFrom, 
				null DateTo
	declare @TaskID int = scope_identity()
	exec EODData.WaitForResult @PoolID, @TaskID
end
