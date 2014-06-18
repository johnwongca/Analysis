create view EODData.vSymbol
as
select Exchange, Symbol, Name, LongName, Date from EODData.Symbol