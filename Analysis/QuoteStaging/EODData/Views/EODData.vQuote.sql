create view EODData.vQuote
AS 
select Exchange, Symbol, IntervalID, Date, [Open], [Close], High, Low, Volume, Ask, Bid, OpenInterest from EODData.Quote