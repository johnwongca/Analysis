alter function IMP.GetFileContent
(
	@FileName varchar(Max)
) returns varchar(max)
as
begin
	declare @handle int, @ret varchar(max);
	select @handle = PRG.TextFile_OpenFile(@FileName)
	select @ret = PRG.TextFile_GetText(@handle)
	select @handle = PRG.TextFile_CloseFile(@handle)
	return @ret;
end