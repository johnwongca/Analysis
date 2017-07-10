CREATE function [EODData].[ReadSymbolChange]()
returns table
as
return (
			select RowID, C1 as Date, C2 as FromExchange, C3 FromSymbol, C4 ToExchange, C5 ToSymbol
			from EODData.ReadCSV(EODData.FileLocation()+'Names\Changes.txt', '	', 1)
			where C1 is not null and C2 is not null and c3 is not null and c4 is not null and c5 is not null
	)