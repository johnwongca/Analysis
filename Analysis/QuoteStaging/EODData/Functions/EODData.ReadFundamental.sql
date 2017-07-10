CREATE function [EODData].[ReadFundamental](@Exchange varchar(1000))
returns table
as
return (
			select 
					RowID, nullif(rtrim(C1), '') as Symbol, 
					nullif(rtrim(C2), '') as Name, 
					nullif(rtrim(C3), '') as Sector, 
					nullif(rtrim(C4), '') as Industry,
					try_cast(C5 as float) as PE, 
					try_cast(C6 as float) as EPS, 
					try_cast(C7 as float) as DivYield, 
					try_cast(C8 as bigint) as Shares, 
					try_cast(C9 as float) as DPS, 
					try_cast(C10 as float) as PEG, 
					try_cast(C11 as float) as PtS, 
					try_cast(C12 as float) as PtB
			from EODData.ReadCSV(EODData.FileLocation()+'Fundamentals\' + rtrim(ltrim(@Exchange))+'.txt', '	', 1)
			where rtrim(isnull(C1, ''))<>'' and rtrim(isnull(C2, '')) <>''
	)