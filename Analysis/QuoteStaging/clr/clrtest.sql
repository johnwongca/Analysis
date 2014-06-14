declare @Dummy int
select @Dummy = clr.SessionParameterClear(@@spid)
select @Dummy = clr.SessionParameterSetValue(@@spid, '@___ConnectionString___ varchar(1000)','Server=SQL1;Database=quote;trusted_connection=true;pooling=true')
--select * from clr.ExecuteSQLAll(@@spid, 'Text', 'select top 10 * from q.quote')
exec clr.ExecuteSQL @@spid, 'Text','select top 10 * from q.quote'


--create table test1 (QuoteID bigint, SymbolID int, Date date, [Open] float, [Close] float, Low float, [high] float, Volume bigint)

select clr.BulkCopy(@@spid, 'Text', 'select top 10 * from q.quote', 'test1', 1500, 1)