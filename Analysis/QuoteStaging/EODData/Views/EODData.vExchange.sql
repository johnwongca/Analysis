create view [EODData].[vExchange]
as
select 
		Exchange, Name, Country, 
		Currency, Declines, HasIntradayProduct, 
		IntradayStartDate, IsIntraday, LastTradeDateTime, 
		Suffix, TimeZone 
from EODData.Exchange