alter function IMP.GetPath
(
	@LocationID int
) returns varchar(max)
as
begin
	declare @SourceFolder varchar(max), @FileName varchar(128), @Type varchar(10), @Address varchar(max), @Port int, @UserName varchar(max), @Password varchar(max)
	declare @ret varchar(max)
	select	
			@SourceFolder = l.SourceFolder,
			@FileName = isnull(l.FileName,''),
			@Type = upper(a.Type),
			@Address = a.Address,
			@Port = a.Port,
			@UserName = a.UserName,
			@Password = a.Password
	from Location l
		inner join [Address] a on l.AddressID = a.ID
	where l.ID = @LocationID
	If(@Type = 'FTP')
	begin
		select @ret = isnull(
								ltrim(rtrim(@UserName)) + 
								isnull(':' + @Password,'') + 
								'@', ''
							)	+	
						rtrim(ltrim(@Address))+'/' + 
						ltrim(rtrim(isnull(@SourceFolder,''))) + '/';
		select @ret = replace (@ret, '//', '/')
		select @ret = replace (@ret, '//', '/')
		select @ret = 'ftp://'+replace (@ret, '//', '/') + @FileName
	end
	else
	begin
		select @ret = @Address + '\' + ltrim(rtrim(isnull(@SourceFolder,''))) + '\'
		select @ret = replace (@ret, '\\', '\')
		select @ret = replace (@ret, '\\', '\') + @FileName
		if(left(@ret,1) = '\')
			select @ret = '\' + @ret
	end
	return @ret
end