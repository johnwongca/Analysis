alter procedure IMP.EOD_Change
(
	@SourceFileID int,
	@raiserror bit = 0,
	@AutoArchive bit = 1
)
as
begin
	declare @handle int,@Body varchar(max), @r int, @proc varchar(256), @Imported bit
	select @Body = s.Body, @Imported = Imported
	from SourceFile s 
		inner join Location l on l.ID = s.LocationID 
		inner join Format f on F.ID = l.FormatID 
	where s.ID = @SourceFileID 
		and isnull(Body,'') <> ''  
		and f.ProcedureName = 'IMP.EOD_Change' 
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
		select @handle = PRG.TextFile_OpenString(@Body);
		select @r = PRG.TextFile_SetDelimiter(@handle, ',');
		create table #t (id int, c0 varchar(max), c1 varchar(max), c2 varchar(max), c3 varchar(max))
		insert into #t
			exec PRG.TextFile_GetTable @handle, 4
		exec PRG.TextFile_CloseFile @handle;
		--select * from #t
		select @handle = null
		declare @c0 nvarchar(30), @c1 nvarchar(30), @c2 nvarchar(30), @c3 nvarchar(30)
		declare @ExchangeID int, @Date smalldatetime
		declare c cursor local for
			select 
					left(upper(ltrim(rtrim(c0))), 30), 
					left(rtrim(ltrim(c1)), 30),
					left(rtrim(ltrim(c2)), 30),
					left(rtrim(ltrim(c3)), 30)
			from #t
		open c
		fetch next from c into @c0, @c1, @c2, @c3
		while @@fetch_status = 0
		begin
			if @c0 = 'DATE'
				goto _NEXT_
			select @ExchangeID = ID, @Date = cast(@c0 as smalldatetime)
			from STK.Exchange
			where Name = @c1
			if @@rowcount = 0
				raiserror('Count not find Exchange %s.', 16, 1, @c1)
			if exists(
						select 1
						from STK.SymbolHistory h
						where h.ExchangeID = @ExchangeID
							and h.OldName = @c2
							and h.NewName = @c3
							and h.Date = @Date
					)
				goto _NEXT_
			insert into STK.SymbolHistory (ExchangeID, OldName, NewName, Date)
					values(@ExchangeID, @c2, @c3, @Date)
_NEXT_:
			fetch next from c into  @c0, @c1, @c2, @c3
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