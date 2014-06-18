create view EODData.vQuote
AS 
select Exchange, Symbol, Date, [Open], [Close], High, Low, Volume, Ask, Bid, OpenInterest from EODData.Quote