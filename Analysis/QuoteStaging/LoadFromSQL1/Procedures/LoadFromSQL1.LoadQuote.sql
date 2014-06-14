create procedure LoadFromSQL1.LoadQuote
as
begin
	set nocount on
	declare @Dummy int
	select @Dummy = clr.SessionParameterClear(@@spid)
	select @Dummy = clr.SessionParameterSetValue(@@spid, '@___ConnectionString___ varchar(1000)','Server=SQL1;Database=quote;trusted_connection=true;pooling=true')
	select @Dummy = clr.BulkCopy(@@spid, 'Text', '
		select Exchange, ExchangeName, CountryID as Country, Currency, Timezone 
		from q.exchange', 'LoadFromSQL1.Exchange', 3000, 1)
	select @Dummy = clr.BulkCopy(@@spid, 'Text', '
		select SymbolID, Exchange, Symbol, SymbolName , SymbolLongName 
		from q.ExchangeSymbol', 'LoadFromSQL1.Symbol', 3000, 1)
	select @Dummy = clr.BulkCopy(@@spid, 'Text', '
		select * from q.Split', 'LoadFromSQL1.Split', 3000, 1)
	select @Dummy = clr.BulkCopy(@@spid, 'Text', '
		select * from q.SymbolChange', 'LoadFromSQL1.SymbolChange', 0, 1)
	select @Dummy = clr.BulkCopy(@@spid, 'Text', '
		select * 
		from q.Quote 
		where Date> cast( dateadd(day, -60, getdate()) as date)
		', 'LoadFromSQL1.Quote', 3000, 1)
	select @Dummy = clr.BulkCopy(@@spid, 'Text', '
		select q.SymbolID, q.Date, qd.Time, qd.[Open],qd.[High],qd.[Low],qd.[Close] ,qd.[Volume]
		from q.Quote  q
			inner join q.QuoteDetail qd on qd.QuoteID = q.QuoteID
		where qd.Type = 1
			and q.Date> cast( dateadd(day, -60, getdate()) as date)', 'LoadFromSQL1.QuoteMinute', 3000, 1)
end
