if object_id('A.Daily_EMA') is null
	exec('create view A.Daily_EMA as select 1 as one')
go
declare @t varchar(100), @i int, @SQL varchar(max)
select @SQL = 'Alter View A.Daily_EMA
as
	select 
		d.SymbolID, d.Seq, d.Date, d.Opening, d.High, d.Low, d.Closing, d.Volume, d.Interest
';
select @i = 1
while @i <= 100
begin 
	select @SQL = @SQL + 
			'		,A.Cal_Average(d.SymbolID, d.Seq, null, ''Exponential'', ' + 
			cast(@i as varchar(10))+ ') EMA_' + right('000'+ cast(@i as varchar(10)), 3)+ '
',
		@i = @i +1
end
select @SQL = @SQL +'
	from A.Daily d
	--where SymbolID = 12773
'
exec(@SQL)