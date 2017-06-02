CREATE function [EODData].[ReadSplit](@Exchange varchar(1000))
returns table
as
return (
			select RowID, C1 as Symbol, C2 as Date, C3 Ratio
			from EODData.ReadCSV(EODData.FileLocation()+'splits\'+rtrim(ltrim(@Exchange))+'.txt', '	', 1)
	)