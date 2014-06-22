CREATE PROCEDURE [EODData].[SetSessionStatus]
(
	@ManagementThreadID int,
	@TaskSessionID smallint,
	@TaskID int,
	@PoolID smallint,
	@MethodName varchar(50),
	@IntervalID tinyint,
	@Exchange varchar(10),
	@DateFrom date,
	@Status varchar(50),
	@Error varchar(max)
)
AS 
begin
	set nocount on
	;merge EODData.SessionStatus as t
	using (select @ManagementThreadID as ManagementThreadID) s on t.ManagementThreadID = s.ManagementThreadID
	when matched then
		update set TaskSessionID = @TaskSessionID,
					TaskID = @TaskID,
					PoolID = @PoolID,
					MethodName = @MethodName,
					IntervalID = @IntervalID,
					Exchange = @Exchange,
					DateFrom = @DateFrom,
					Status = @Status,
					StatusDate = getdate(),
					Error = @Error
	when not matched then
		insert (ManagementThreadID, TaskSessionID, TaskID, PoolID, MethodName, IntervalID, Exchange, DateFrom, Status, StatusDate, Error)
			values(@ManagementThreadID, @TaskSessionID, @TaskID, @PoolID, @MethodName, @IntervalID, @Exchange, @DateFrom, @Status, getdate(), @Error)
	;

end