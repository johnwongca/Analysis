alter procedure IMP.EOD_Daily
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
		and f.ProcedureName = 'IMP.EOD_Daily' 
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
		select @ExchangeName = left(@ExchangeName, charindex('_', @ExchangeName)-1)
		select @ExchangeID = ID from STK.Exchange where Name = @ExchangeName
		if @ExchangeID is null
			raiserror('Could not find exchange name %s!', 16, 1, @ExchangeName)
		select @handle = PRG.TextFile_OpenString(@Body);
		select @r = PRG.TextFile_SetDelimiter(@handle, ',');
		create table #t (
							id int, c0 varchar(max), c1 varchar(max),
							c2 varchar(max), c3 varchar(max), c4 varchar(max),
							c5 varchar(max), c6 varchar(max), c7 varchar(max)
						)
		insert into #t
			exec PRG.TextFile_GetTable @handle, 8
		exec PRG.TextFile_CloseFile @handle;
		select @handle = null
		declare @c0 nvarchar(30), @c1 nvarchar(30), @c2 nvarchar(30), @c3 nvarchar(30), @c4 nvarchar(30), @c5 nvarchar(30), @c6 nvarchar(30), @c7 nvarchar(30)
		declare @ID int, @SymbolID int, @Date smalldatetime, @High money, @Low money, @Opening money, @Closing money, @Volume money, @Interest money
		declare c cursor local for
			select 
					left(upper(ltrim(rtrim(c0))), 30), --Symbol
					left(upper(ltrim(rtrim(c1))), 30), --Date
					left(upper(ltrim(rtrim(c2))), 30), --open
					left(upper(ltrim(rtrim(c3))), 30), --high
					left(upper(ltrim(rtrim(c4))), 30), --low
					left(upper(ltrim(rtrim(c5))), 30), --close
					left(upper(ltrim(rtrim(c6))), 30), --volume
					left(upper(ltrim(rtrim(c7))), 30)  --OpenInterest
			from #t
		open c
		fetch next from c into @c0, @c1, @c2, @c3, @c4, @c5, @c6, @c7
		while @@fetch_status = 0
		begin
			select	@SymbolID = ID, 
					@Date = cast(@c1 as smalldatetime), 
					@Opening = cast(@c2 as money),
					@High = case when cast(@c3 as money) < cast(@c4 as money) then cast(@c4 as money) else cast(@c3 as money) end,
					@Low = case when cast(@c3 as money) > cast(@c4 as money) then cast(@c4 as money) else cast(@c3 as money) end,
					@Closing = cast(@c5 as money),
					@Volume = cast(@c6 as money),
					@Interest = case when rtrim(isnull(@c7,'')) = '' then 0 else cast(@c7 as money) end,
					@ID = null
			from STK.Symbol
			where ExchangeID = @ExchangeID and Name = @c0
			if @@rowcount = 0
			begin
				insert into STK.Symbol(ExchangeID, Name, Description)
					values (@ExchangeID, @c0, 'Unknown - From Import')
				--raiserror('Could not find Symbol %s.', 16, 1, @c0)
				select	@SymbolID = ID, 
						@Date = cast(@c1 as smalldatetime), 
						@Opening = cast(@c2 as money),
						@High = case when cast(@c3 as money) < cast(@c4 as money) then cast(@c4 as money) else cast(@c3 as money) end,
						@Low = case when cast(@c3 as money) > cast(@c4 as money) then cast(@c4 as money) else cast(@c3 as money) end,
						@Closing = cast(@c5 as money),
						@Volume = cast(@c6 as money),
						@Interest = case when rtrim(isnull(@c7,'')) = '' then 0 else cast(@c7 as money) end,
						@ID = null
				from STK.Symbol
				where ExchangeID = @ExchangeID and Name = @c0
				if @@rowcount = 0
				begin
					raiserror('Could not find Symbol %s.', 16, 1, @c0)
				end 
			end		
			update STK.Daily
				set Opening = @Opening,
					High = @High,
					Low = @Low,
					Closing = @Closing,
					Volume = @Volume,
					Interest = @Interest
			where Date = @Date
				and SymbolID = @SymbolID
			if @@rowcount = 0
			begin
				select @ID = max(ID) from STK.Daily where Date = @Date
				select @ID = isnull(@ID, 0)+1
				insert into STK.Daily(Date, ID, SymbolID, Opening, High, Low, Closing, Volume, Interest)
					values(@Date, @ID, @SymbolID, @Opening, @High, @Low, @Closing, @Volume, @Interest)
			end
_NEXT_:
			fetch next from c into @c0, @c1, @c2, @c3, @c4, @c5, @c6, @c7
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