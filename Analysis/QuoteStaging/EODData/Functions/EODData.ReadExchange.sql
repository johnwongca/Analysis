CREATE function [EODData].[ReadExchange]()
returns table
as
return (
			select RowID, C1 Code, C2 Name
			from EODData.ReadCSV(EODData.FileLocation() +'Names\Exchanges.txt','	', 1) 
			where rtrim(isnull(C1, ''))<>'' and rtrim(isnull(C2, '')) <>''
	)