CREATE function [EODData].[ReadQuote](@Exchange varchar(1000), @Date date)
returns table
as
return (
			select RowID, nullif(rtrim(C1), '') as Symbol, try_cast(C3 as float) as [Open], try_cast(C4 as float) as High, try_cast(C5 as float) as Low, try_cast(C6 as float) as [Close], try_cast(C7 as bigint) as Volume
			from EODData.ReadCSV(EODData.FileLocation() + rtrim(ltrim(@Exchange))+'_'+convert(varchar(8), @Date, 112)+'.txt', ',', 0)
	)