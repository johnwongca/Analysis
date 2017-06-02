CREATE function [EODData].[ReadSymbol](@Exchange varchar(1000))
returns table
as
return (
			select RowID, C1 as Symbol, C2 as Name
			from EODData.ReadCSV(EODData.FileLocation() +'Names\'+rtrim(ltrim(@Exchange))+'.txt', '	', 1)
	)