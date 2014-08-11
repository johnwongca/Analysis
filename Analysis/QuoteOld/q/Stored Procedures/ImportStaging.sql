CREATE procedure q.ImportStaging
as
begin
	set xact_abort on
	declare @FromVersion binary(8), @ToVersion binary(8), @Proc sysname = 'EODData..sp_executesql'
	select @FromVersion = LastLoadedVersion from q.LastLoadedVersion
	if @@ROWCOUNT = 0
	begin
		insert into q.LastLoadedVersion default values
		select @FromVersion = LastLoadedVersion from q.LastLoadedVersion
	end
	exec @Proc N'select @MaxVersion = @@DBTS', N'@MaxVersion binary(8) output', @ToVersion output
	
	exec q.ImportStaging_Exchange @FromVersion, @ToVersion
	exec q.ImportStaging_Symbol @FromVersion, @ToVersion
	exec q.ImportStaging_Fundamental @FromVersion, @ToVersion
	exec q.ImportStaging_Split @FromVersion, @ToVersion
	exec q.ImportStaging_SymbolChange @FromVersion, @ToVersion
	exec q.ImportStaging_Quote @FromVersion, @ToVersion
	update q.LastLoadedVersion set LastLoadedVersion = @ToVersion
	return
end
