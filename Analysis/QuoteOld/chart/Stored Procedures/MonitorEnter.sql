create procedure chart.MonitorEnter (@SyncObject nvarchar(255))
as
begin
	declare @ret int
	exec @ret = sp_getapplock @Resource = @SyncObject, @LockMode = 'Exclusive', @LockOwner = 'Session'
	return @ret
end
