CREATE PROCEDURE [EODData].[WaitForResult]
(
	@PoolID smallint,
	@TaskID int
)
AS
begin
	set nocount on
	if @PoolID = 0
		return;
	while not exists(select * from EODData.Task with(nolock) where PoolID = @PoolID and TaskID = @TaskID)
	begin
		waitfor delay '00:00:00.100'
	end
	declare @Error varchar(max)
	select @Error = Error from EODData.TaskCompleted where TaskID = TaskID
	if @Error is not null
		throw 50000, @Error, 1
end