CREATE function [EODData].[ReadExchange]()
returns table
as
return (
			select RowID, C1 Code, C2 Name
			from EODData.ReadCSV(EODData.FileLocation() +'Names\Exchanges.txt','	', 1) 

	)