create trigger EODData.TRI_Exchange on [EODData].[vExchange]
instead of insert
AS 
BEGIN 
	set nocount on
	merge EODData.Exchange t 
	using (select 
					Exchange, Name, Country, 
					Currency, Declines, HasIntradayProduct, 
					case when IntradayStartDate < '1960-01-01' then null else IntradayStartDate end IntradayStartDate, IsIntraday, LastTradeDateTime, 
					Suffix, TimeZone 
			from inserted) s on t.Exchange = s.Exchange
	when matched and (
							isnull(t.Name, '') <> isnull(s.Name, '')
							or isnull(t.Country,'') <> isnull(s.Country, '')
							or isnull(t.Currency, '') <> isnull(s.Currency, '')
							or isnull(t.Declines,0) <> isnull(s.Declines,0)
							or isnull(t.HasIntradayProduct,0) <> isnull(s.HasIntradayProduct,0)
							or isnull(t.IntradayStartDate,'1960-01-01') <> isnull(s.IntradayStartDate,'1960-01-01')
							or isnull(t.IsIntraday, 0) <> isnull(s.IsIntraday,0)
							or isnull(t.LastTradeDateTime, '1960-01-01') <> isnull(s.LastTradeDateTime, '1960-01-01')
							or isnull(t.Suffix,'') <> isnull(s.Suffix,0)
							or isnull(t.TimeZone,'') <> isnull(s.TimeZone, '')
						) then 
		update set Name = s.Name,
					Country = s.Country,
					Currency = s.Currency,
					Declines = s.Declines,
					HasIntradayProduct = s.HasIntradayProduct,
					IntradayStartDate = s.IntradayStartDate,
					IsIntraday = s.IsIntraday,
					LastTradeDateTime = s.LastTradeDateTime,
					Suffix = s.Suffix,
					TimeZone = s.TimeZone
	when not matched then
		insert (Exchange, Name, Country, Currency, Declines, HasIntradayProduct, IntradayStartDate, IsIntraday, LastTradeDateTime, Suffix, TimeZone)
			values(s.Exchange, s.Name, s.Country, s.Currency, s.Declines, s.HasIntradayProduct, s.IntradayStartDate, s.IsIntraday, s.LastTradeDateTime, s.Suffix, s.TimeZone)
	option(loop join);
END