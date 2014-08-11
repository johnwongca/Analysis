create procedure [rpt].[GetExchange]
as
begin
	select 
			a.ExchangeID,
			a.Exchange ShortName,
			a.ExchangeName,
			a.CountryID,
			a.Currency,
			a.TimeZone,
			(select count(*) from q.Symbol b where b.ExchangeID = a.ExchangeID) SymbolCount
	from q.Exchange a
	order by a.Exchange
end