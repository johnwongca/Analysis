alter procedure ETL.ConvertTextToTalbe
(
	@Text			varchar(max),
	@ColumnCount	int = 1,
	@Delimiter		varchar(128) = '',
	@IgnoreEmpties	bit = 0
)
as
begin
	set nocount on
	create table #ReturnTable (ID int primary key)
	if @ColumnCount < 1
		select @ColumnCount = 1
	declare @i bigint, @s varchar(max), @l bigint, @current bigint, @id int, @colid int, @j bigint
	declare @i1 bigint, @i2 bigint, @c1 int, @current1 bigint, @s1 varchar(max), @dl int, @sql nvarchar(max)
	select @i = 1, @dl = len(@Delimiter)
	while @i <=@ColumnCount
	begin
		select @s = '
						alter table #ReturnTable add C'+cast(@i as varchar(100))+' varchar(max) default('''')
					'
		exec(@s)
		select @i =@i + 1
	end
	--populate lines
	declare @1 varchar(1), @2 varchar(1), @3 varchar(2)
	select @j = 1, @i = 1, @l = len(@Text), @id = 1, @colid = 1
	while(@i<=@l)
	begin
		select @j = @i, @current = 0
		while (@j <= @l)
		begin
			select @3 = substring(@Text, @j, 2)
			select @1 = substring(@3, 1, 1), @2 = substring(@3, 2, 1)
			if @1 = char(13) and @2 = char(10)
			begin
				select @current = @j + 1
				break;
			end
			else if @1 = char(10) or @1 = char(13) and @2 <> char(10) or @1 = char(10) and @2 not in (char(10), char(13))
			begin
				select @current = @j
				break;
			end
			select @j = @j +1
		end
		if @current = 0
			select @current = @l
		select @s = substring(@Text, @i, @current - @i + 1)
		select @s = replace(replace(@s, char(13), ''), char(10), '')
		if @IgnoreEmpties = 1 or @s <> ''
		begin
			if @dl = 0
			begin
				insert into #ReturnTable(ID, C1) values(@id, @s)
			end
			else
			begin
				insert into #ReturnTable(ID) values(@id)
				select @i1 = 1, @c1 = 1, @i2 = len(@s), @current1 = 0
				while (@i1 <= @i2 and @c1 <= @ColumnCount)
				begin
					select @current1 = charindex(@Delimiter, @s, @i1)
					if @current1 = 0
						select @current1 = @i2
					select @s1 = replace(substring(@s, @i1, @current1 - @i1 +1), @Delimiter, '')
					select @sql = 'update #ReturnTable set C'+ cast(@c1 as varchar(10))+ ' = @value where ID = @ID'
					exec sp_executesql @stmt = @sql, @params = N'@value varchar(max), @ID int', @value = @s1, @ID = @id
					select @c1 = @c1 + 1, @i1 = @current1 + @dl 
				end
			end
			select @id = @id+1
		end
		select @i = @current + 1
	end
	select * from #ReturnTable
	return
end
go
declare @str nvarchar(max);declare @d datetime
select @d = getdate()
select @str = body from imp.SourceFileArchive where ID = 55
select datediff(millisecond, @d, getdate())
select @d = getdate()
exec ETL.ConvertTextToTalbe @str, 7,',' 
select datediff(millisecond, @d, getdate())


