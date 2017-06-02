CREATE view EODData.QuoteFile
as
with x0 as
(
	select * , charindex('_', FileName) P, len(FileName) -4 as L
	from clr.ListFile(EODData.FileLocation())
	where IsFile = 1
		and FileName like '%!_%.txt' escape '!'
)
select substring(FileName, 1, P-1) as Exchange, convert(Date, substring(FileName, P + 1, L-P), 112) as Date, FullFileName, FileName, CreationDate, LastModifiedDate, Length 
from x0