alter procedure IMP.EOD_Symbol
(
	@SourceFileID int,
	@raiserror bit = 0,
	@AutoArchive bit = 1
)
as
begin
	declare @handle int,@Body varchar(max), @r int, @proc varchar(256), @ExchangeName nvarchar(30), @ExchangeID int, @Imported bit
	select @Body = s.Body, @ExchangeName = s.FileName, @Imported = Imported
	from SourceFile s 
		inner join Location l on l.ID = s.LocationID 
		inner join Format f on F.ID = l.FormatID 
	where s.ID = @SourceFileID 
		and isnull(Body,'') <> ''  
		and f.ProcedureName = 'IMP.EOD_Symbol' 
	if @AutoArchive = 1 and @Imported = 1
	begin
		exec IMP.ArchiveSourceFile @SourceFileID
	end
	if @Body is null
	begin
		if @raiserror = 1
			raiserror('Could not find text body for SouceFileID %d or wrong importing map.', 16, 1, @SourceFileID);
		return;
	end
	
	begin try
		begin transaction
		select @ExchangeName = left(@ExchangeName, charindex('.', @ExchangeName)-1)
		select @ExchangeID = ID from STK.Exchange where Name = @ExchangeName
		if @ExchangeID is null
			raiserror('Could not find exchange name %s!', 16, 1, @ExchangeName)
		select @handle = PRG.TextFile_OpenString(@Body);
		select @r = PRG.TextFile_SetDelimiter(@handle, '	');
		create table #t (id int, c0 varchar(max), c1 varchar(max))
		insert into #t
			exec PRG.TextFile_GetTable @handle, 2
		exec PRG.TextFile_CloseFile @handle;
		select @handle = null
		declare @c0 nvarchar(30), @c1 nvarchar(128)
		declare c cursor local for
			select left(upper(ltrim(rtrim(c0))), 30), left(rtrim(ltrim(c1)), 128) from #t
		open c
		fetch next from c into @c0, @c1
		while @@fetch_status = 0
		begin
			if @c0 = 'SYMBOL'
				goto _NEXT_
			update STK.Symbol
				set Description = @c1
			where Name = @c0 and ExchangeID = @ExchangeID;
			if @@rowcount = 0
			begin
				insert into STK.Symbol(ExchangeID, Name, Description)
					values(@ExchangeID, @c0, @c1)
			end
_NEXT_:
			fetch next from c into @c0, @c1
		end
		close c
		deallocate c
		exec IMP.FinishImport @SourceFileID
		if @AutoArchive = 1
			exec IMP.ArchiveSourceFile @SourceFileID
		commit transaction
	end try
	begin catch
		rollback transaction
		update SourceFile
			set Error = PRG.GetErrorMessage()
		where ID = @SourceFileID;
		if @handle is not null
			select @r = PRG.TextFile_CloseFile(@handle);
	end catch
end