create procedure EODData.GetTask @Timeout int = 60 -- 60 seconds
as
begin
	set nocount on
	if @@trancount = 0
		throw 50000, 'EODData.GetTask must be run within a transaction', 1
	declare @i int = 1, @Max int, @ret int = -1, @Res nvarchar(200), @TaskID int, @TimeoutDate datetime = dateadd(second, @Timeout, getdate())
	select @Max = cast(Value as int) from EODData.Setting where Name = 'MaxConcurrentExecution'
	while getdate() < @TimeoutDate
	begin
		if @@trancount = 0
			begin transaction
		select @i = 1
		while @i <= @Max
		begin
			select @Res = cast(@i as varchar(20))+ '-EODData.GetTask'
			exec @ret = sp_getapplock @Resource =  @Res, @LockMode =  'Exclusive',  @LockOwner =  'Transaction' ,  @LockTimeout = 0
			if @ret >=0
				break;
			select @i = @i + 1
		end
	
		select top 1 @TaskID = TaskID
		from EODData.Task with(readcommittedlock, readpast, updlock, rowlock)
		where NextStartDate <=getdate() and IsRegularPool = 0
		order by IsRegularPool, NextStartDate
		if @@rowcount > 0
			goto ___Found___
		select top 1 @TaskID = TaskID
		from EODData.Task with(readcommittedlock, readpast, updlock, rowlock)
		where NextStartDate <=getdate() and IsRegularPool = 1
		order by IsRegularPool, NextStartDate
		if @@rowcount > 0
			goto ___Found___
		commit
		waitfor delay '00:00:00.100'
		continue
___Found___:
		select PoolID, TaskID, MethodName, Exchange, IntervalID, DateFrom
		from EODData.Task with(readcommittedlock, readpast, updlock, rowlock)
		where @TaskID = TaskID
		break
	end
	if @@trancount = 0
		begin transaction
end