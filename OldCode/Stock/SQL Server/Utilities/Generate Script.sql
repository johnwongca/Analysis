--alter table
declare @i int, @j int, @k int, @s varchar(max), @name varchar(128)
select @i = 0, @s = '', @k = -1
declare c cursor local for
	select name, TypeID from DIM.Formula 
	where TypeID > 300
	order by ID
open c
fetch next from c into @name, @j
while @@fetch_status = 0
begin
	select @s = 'alter table DIM.Fact_Base add '+@Name + ' float not null default(0) '
	print @s
	fetch next from c into @name, @j
end
close c
deallocate c
--Last Values
declare @i int, @j int, @k int, @s varchar(max), @name varchar(128)
select @i = 0, @s = '', @k = -1
declare c cursor local for
	select name, TypeID from DIM.Formula 
	where TypeID >= 0 
	order by ID
open c
fetch next from c into @name, @j
while @@fetch_status = 0
begin
	if @k <> @j
	begin
		if len(@s)>0 
			select @s = substring(@s, 1, len(@s)-1)
		print @s
		select @k = @j, @s = 'declare '
	end
	select @s = @s + '@Last'+@Name + ' float, '
	fetch next from c into @name, @j
end
close c
deallocate c
if len(@s)>0 
	select @s = substring(@s, 1, len(@s)-1)
print @s
go
--Current Values
declare @i int, @j int, @k int, @s varchar(max), @name varchar(128)
select @i = 0, @s = '', @k = -1
declare c cursor local for
	select name, TypeID from DIM.Formula 
	where TypeID > 300
	order by ID
open c
fetch next from c into @name, @j
while @@fetch_status = 0
begin
	if @k <> @j
	begin
		if len(@s)>0 
			select @s = substring(@s, 1, len(@s)-1)
		print @s
		select @k = @j, @s = 'declare '
	end
	select @s = @s + '@'+@Name + ' float, '
	fetch next from c into @name, @j
end
close c
deallocate c
if len(@s)>0 
	select @s = substring(@s, 1, len(@s)-1)
print @s
go
print ''
declare @i int, @j int, @k int, @s varchar(max), @name varchar(128)
select @i = 0, @s = 'declare ', @k = -1
declare c cursor local for
	select name 
	from sys.columns 
	where object_id('A.Daily') = object_id and name not in ('SymbolID', 'Seq')
	order by column_id
open c
fetch next from c into @name
while @@fetch_status = 0
begin
	if @Name like '%Diff%'
	begin
		select @s = @s + '@A0_' + replace(@Name, 'Diff', 'Gain') + ' float, '
		select @s = @s + '@A0_' + replace(@Name, 'Diff', 'Loss') + ' float, '
	end
	else
		select @s = @s + '@A0_'+@Name + ' float, '
	fetch next from c into @name
end
close c
deallocate c
if len(@s)>0 
	select @s = substring(@s, 1, len(@s)-1)
print @s
select @s = replace(@s,'@A0', '@A1')
print @s
select @s = replace(@s,'@A1', '@A2')
print @s
go
--Read Last Values
declare @i int, @j int, @k int, @s varchar(max), @name varchar(512)
select @i = 0, @s = 'select ', @k = -1
declare c cursor local for
	select name, TypeID, ID from DIM.Formula 
	where TypeID > 300
	order by ID
open c
fetch next from c into @name, @j, @i
while @@fetch_status = 0
begin
	if @k <> @j
	begin
		if len(@s)>0 
			if substring(@s, len(@s)-1, 1 ) = ','
				select @s = substring(@s, 1, len(@s)-1)
		print @s
		select @k = @j, @s = ' '
	end
	select @s = @s + '@Last'+@Name + ' =  isnull('+@Name + ', 0) , '
	if len(@s) > 7800
	begin
		print @s
		select @s = ''
	end
	fetch next from c into @name, @j, @i
end
close c
deallocate c
if len(@s)>0 
	select @s = substring(@s, 1, len(@s)-1)
print @s
print 'from (select 1 as x) x
		left outer join DIM.Fact_Base f on	f.SymbolID = @SymbolID 
										and f.Seq = @Seq -1
										and f.PeriodID = @PeriodID'
go
--read A0
print ''
declare @i int, @j int, @k int, @s varchar(max), @name varchar(128)
select @i = 0, @s = 'select  ', @k = -1
declare c cursor local for
	select name 
	from sys.columns 
	where object_id('A.Daily') = object_id and name not in ('SymbolID', 'Seq')
	order by column_id
open c
fetch next from c into @name
while @@fetch_status = 0
begin
	if @Name like '%Diff%'
	begin
		select @s = @s + '@A0_' + replace(@Name, 'Diff', 'Gain') + ' = isnull((case when '+@Name+' >=0 then '+@Name+' else 0 end), 0), '
		select @s = @s + '@A0_' + replace(@Name, 'Diff', 'Loss') + ' = isnull((case when '+@Name+' <=0 then -'+@Name+' else 0 end), 0), '
	end
	else if @Name = 'Date'
		select @s = @s + '@A0_'+@Name + ' = isnull(cast('+@Name+' as float),0), '
	else
		select @s = @s + '@A0_'+@Name + ' = isnull('+@Name+',0), '
	fetch next from c into @name
end
close c
deallocate c
if len(@s)>0 
	select @s = substring(@s, 1, len(@s)-1)
print @s
print 'from (select 1 as x) x
		left outer join A.Daily b with (nolock) on b.SymbolID = @SymbolID and b.Seq = @Seq - @PeriodID --+ 1'

select @s = replace(replace(@s, '@A0', '@A1'), 'select ', 'select top 1 ')
print @s
print 'from A.Daily with(nolock)
	where SymbolID = @SymbolID
		and Seq >= @Seq - @PeriodID + 1
	order by SymbolID, Seq'
select @s = replace(replace(@s, '@A1', '@A2'), 'Top 1 ', '')
print @s
print 'from A.Daily with(nolock)
	where SymbolID = @SymbolID
		and Seq = @Seq'
go
/*Calculate */
declare @i int, @j int, @k int, @s varchar(max), @name varchar(128), @desc varchar(256)
select @i = 0, @s = '', @k = -1
declare c cursor local for
	select name, TypeID, description from DIM.Formula 
	where TypeID >= 200
	order by ID
open c
fetch next from c into @name, @j, @desc
while @@fetch_status = 0
begin
	select @s = '--'+@desc+''
	print @s
	select @s = 'select @'+@Name + ' = '
	print @s
	fetch next from c into @name, @j, @desc
end
close c
deallocate c
go
--write return

declare @i int, @j int, @k int, @s varchar(max), @name varchar(128), @desc varchar(256)
select @i = 0, @s = '', @k = -1
declare c cursor local for
	select name, ID, description from DIM.Formula 
	where TypeID >= 0 
	order by ID
open c
fetch next from c into @name, @j, @desc
while @@fetch_status = 0
begin
	select @s = '--'+@desc+''
	--print @s
	select @s = 'insert into @ret(ID, Value, Description) values('+cast(@j as varchar(20))+', @'+@Name + ','''+replace(@desc, '''', '''''')+''')'
	print @s
	fetch next from c into @name, @j, @desc
end
close c
deallocate c
go
--insert back
declare @i int, @j int, @k int, @s varchar(max), @name varchar(128)
select @i = 0, @s = 'insert into DIM.Fact_Base(', @k = -1
declare c cursor local for
	select name, TypeID, ID from DIM.Formula 
	where TypeID >= 0 
	order by ID
open c
fetch next from c into @name, @j, @i
while @@fetch_status = 0
begin
	if @k <> @j
	begin
		if len(@s)>0 
			if substring(@s, len(@s)-1, 1 ) = ','
				select @s = substring(@s, 1, len(@s)-1)
		print @s
		select @k = @j, @s = ' '
	end
	select @s = @s + @Name + ', '
	fetch next from c into @name, @j, @i
end
close c
deallocate c
if len(@s)>0 
	select @s = substring(@s, 1, len(@s)-1)
print @s+'
)'
go
declare @i int, @j int, @k int, @s varchar(max), @name varchar(128)
select @i = 0, @s = 'Values(', @k = -1
declare c cursor local for
	select name, TypeID, ID from DIM.Formula 
	where TypeID >= 0 
	order by ID
open c
fetch next from c into @name, @j, @i
while @@fetch_status = 0
begin
	if @k <> @j
	begin
		if len(@s)>0 
			if substring(@s, len(@s)-1, 1 ) = ','
				select @s = substring(@s, 1, len(@s)-1)
		print @s
		select @k = @j, @s = ' '
	end
	select @s = @s + '@' + @Name + ', '
	fetch next from c into @name, @j, @i
end
close c
deallocate c
if len(@s)>0 
	select @s = substring(@s, 1, len(@s)-1)
print @s+'
)'
go