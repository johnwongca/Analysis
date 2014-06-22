CREATE PROCEDURE [EODData].[WaitForResult]
(
	@PoolID smallint,
	@TaskID int
)
AS
begin
	set nocount on
	if @PoolID = 0 
	begin
		if @TaskID is not null
			return;
		while not exists(select * from EODData.Task with(nolock) where PoolID = @PoolID)
		begin
			waitfor delay '00:00:00.100'
		end
		return;
	end
	while not exists(select * from EODData.Task with(nolock) where PoolID = @PoolID and TaskID = @TaskID)
	begin
		waitfor delay '00:00:00.100'
	end
	declare @Error varchar(max)
	select @Error = Error from EODData.TaskCompleted where TaskID = TaskID
	if @Error is not null
		throw 50000, @Error, 1
end