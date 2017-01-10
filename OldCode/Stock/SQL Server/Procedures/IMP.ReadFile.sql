alter procedure IMP.ReadFile
(
	@LocationID int
)
as
begin
	if exists(select 1 from Location where ID = @LocationID and Active = 0)
		return;
	declare @Path varchar(max), @Filter nvarchar(max), @FilterLogicReversed bit, @msg varchar(max)
	select 
			@Path = IMP.GetPath(@LocationID),
			@Filter = rtrim(ltrim(isnull(Filter, ''))),
			@FilterLogicReversed = FilterLogicReversed 
	from Location 
	where ID = @LocationID;
	if @Filter<>''
		select @Filter = 'select @FilterReturn = case when '+@Filter+' then 1 else -1 end';
	declare @FullName nvarchar(max), @FileName nvarchar(256), @Size bigint, @Type nvarchar(10), @FileDate datetime
	declare @FilterReturn int, @Content varchar(max), @ID int
	begin try
		declare c cursor local static for
			select [FullName], rtrim([FileName]), [Size], [Type], [FileDate]
			from PRG.TextFile_GetDirectory(@Path)
			where [Type] = 'File' and [Size] >0
	end try
	begin catch
		return;
	end catch
	open c
	fetch next from c into @FullName, @FileName, @Size, @Type, @FileDate
	while @@fetch_status = 0
	begin
		if (@Filter<>'')
		begin
			exec sp_executesql @Filter, N'@FileName nvarchar(256), @FilterReturn int output', @FileName = @FileName, @FilterReturn = @FilterReturn output
			if (@FilterReturn = 1 and @FilterLogicReversed = 1)
				goto _NEXT_
			if (@FilterReturn = 1 and @FilterLogicReversed = 0)
				goto _NEXT_
		end
		if exists(select 1 from SourceFileArchive where LocationID = @LocationID and FileName = @FileName and FileSize = @Size and FileDate = @FileDate)
			goto _NEXT_
		begin try
			select @ID = null
			select @ID = ID , @msg = case when Body is null then 'null' else 'not null' end
			from SourceFile 
			where FileName = @FileName 
				and FileSize = @Size 
				and FileDate = @FileDate
				and LocationID = @LocationID
			if @ID is null
			begin
				insert into SourceFile(
										LocationID, FileName, FileDate, 
										FileSize, ImportDate)
					values(
										@LocationID, @FileName, @FileDate, 
										@Size, null
							);
				select @ID = scope_identity();
			end
			else
			begin
				if @msg = 'not null'
					goto _NEXT_
			end
			select @Content = IMP.GetFileContent(@FullName)
			update SourceFile set Body = @Content where ID = @ID;
		end try
		begin catch
			select @msg = PRG.GetErrorMessage();
			if @ID is not null
				update SourceFile set Error = @msg where ID = @ID
			else
			begin
				declare @Param varchar(max)
				select @Param = 'LocationID ='+ cast(@LocationID as varchar(20))+'; FullName='+@FullName
				exec WriteError 'IMP.ReadFile', @Param, @msg
			end
		end catch
_NEXT_:		
		fetch next from c into @FullName, @FileName, @Size, @Type, @FileDate
	end
	close c
	deallocate c
end;
