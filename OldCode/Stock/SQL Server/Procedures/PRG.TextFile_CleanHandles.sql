alter procedure PRG.TextFile_CleanHandles
as
begin
	declare @Handle int
	while 1=1
	begin
		select @handle = null
		select top 1 @Handle = Handle from PRG.TextFile_ListHandles()
		if @handle is not null
			exec PRG.TextFile_CloseFile @Handle
		else
			break;
	end
end