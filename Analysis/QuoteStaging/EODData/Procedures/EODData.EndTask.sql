create procedure [EODData].[EndTask](@PoolID smallint, @TaskID int, @Error varchar(max))
as
begin
	set nocount on
	if @PoolID = 0
	begin
		if @Error is null
			goto ___Done___;
		update EODData.Task		
			set Retries = Retries + 1,
				LastCompletionDate = getdate(),
				Error = @Error
			output inserted.PoolID, inserted.TaskID, inserted.MethodName, 
				inserted.IntervalID, inserted.Exchange, inserted.Symbol, 
				inserted.DateFrom, inserted.DateTo, inserted.EnlistDate, 
				inserted.Retries, inserted.Error
				into EODData.TaskCompleted(PoolID, TaskID, MethodName, IntervalID, Exchange, Symbol, DateFrom, DateTo, EnlistDate, Retries, Error)
		where TaskID = @TaskID
		return
	end
___Done___:
	delete EODData.Task
		output deleted.PoolID, deleted.TaskID, deleted.MethodName, 
				deleted.IntervalID, deleted.Exchange, deleted.Symbol, 
				deleted.DateFrom, deleted.DateTo, deleted.EnlistDate, 
				deleted.Retries, @Error
		into EODData.TaskCompleted(PoolID, TaskID, MethodName, IntervalID, Exchange, Symbol, DateFrom, DateTo, EnlistDate, Retries, Error)
	where TaskID = @TaskID
end