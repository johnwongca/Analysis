create procedure chart.MonitorExit(@SyncObject nvarchar(255))
as
begin
	begin try
		exec sp_releaseapplock @Resource = @SyncObject, @LockOwner = 'Session'
	end try
	begin catch
		-- I don't care exceptions here
	end catch
end 
