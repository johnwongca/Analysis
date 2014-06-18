create view EODData.vSymbolChange
as
select Date, FromExchange, FromSymbol, ToExchange, ToSymbol from EODData.SymbolChange