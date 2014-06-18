create view EODData.vSplit
AS 
select Exchange, Symbol, Date, Ratio from EODData.Split
