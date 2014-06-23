CREATE PROCEDURE [EODData].[SetSessionStatus]
(
	@ManagementThreadID int,
	@TaskSessionID smallint,
	@BulkCopySessionID smallint,
	@TaskID int,
	@PoolID smallint,
	@MethodName varchar(50),
	@IntervalID tinyint,
	@Exchange varchar(10),
	@DateFrom date,
	@Status varchar(50),
	@Error varchar(max),
	@Rows int
)
AS 
begin
	set nocount on
	;merge EODData.SessionStatus as t
	using (select @ManagementThreadID as ManagementThreadID) s on t.ManagementThreadID = s.ManagementThreadID
	when matched then
		update set TaskSessionID = @TaskSessionID,
					BulkCopySessionID = @BulkCopySessionID,
					TaskID = @TaskID,
					PoolID = @PoolID,
					MethodName = @MethodName,
					IntervalID = @IntervalID,
					Exchange = @Exchange,
					DateFrom = @DateFrom,
					Status = @Status,
					StatusDate = getdate(),
					Error = @Error,
					Rows = @Rows
	when not matched then
		insert (ManagementThreadID, BulkCopySessionID, TaskSessionID, TaskID, PoolID, MethodName, IntervalID, Exchange, DateFrom, Status, StatusDate, Error, Rows)
			values(@ManagementThreadID, @BulkCopySessionID, @TaskSessionID, @TaskID, @PoolID, @MethodName, @IntervalID, @Exchange, @DateFrom, @Status, getdate(), @Error, @Rows)
	;

end