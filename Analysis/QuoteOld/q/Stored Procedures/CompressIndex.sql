CREATE procedure [q].[CompressIndex]
as
begin
	declare @SQL nvarchar(max)
	
	select @SQL = (select 'create index ' + c.name + ' on q.'+ QUOTENAME(OBJECT_NAME(c.object_id))+'(' + c.name + ') on [QuoteData];'
	from sys.columns c
	where c.name = '___DataHash___' and OBJECT_SCHEMA_NAME(c.object_id) = 'q'
		and not exists(select 1 from sys.indexes i where i.object_id = c.object_id and name = c.name) for xml path(''))
	exec (@SQL)
	
	select @SQL = (select 'create index ' + c.name + ' on q.'+ QUOTENAME(OBJECT_NAME(c.object_id))+'(' + c.name + ') on [QuoteData];'
	from sys.columns c
	where c.name = '___DataVersion___' and OBJECT_SCHEMA_NAME(c.object_id) = 'q'
		and not exists(select 1 from sys.indexes i where i.object_id = c.object_id and name = c.name) for xml path(''))
	exec (@SQL)
	
	select @SQL = (select 'alter index '+ quotename(i.name) + ' on q.' + quotename(object_name(p.object_id)) + ' rebuild with(data_compression = page);'
	from sys.partitions p 
		inner join sys.indexes i on p.object_id = i.object_id and p.index_id = i.index_id
	where data_compression = 0
		and object_schema_name(p.object_id) = 'q'
	for xml path(''))

	exec(@SQL)
end