use Stock
--select * from A.Symbol where ExchangeID = 16 and Name = 'DLLR'


select SymbolID,
		Seq, Date, Closing, 
		EMA_003, EMA_012, EMA_026, EMA_050,
		-- A.Cal_Average(SymbolID,  Seq, null, 'Exponential', 3,'EMA_012 - EMA_026', 'A.Daily_EMA' ) as Signal,
		
		EMA_012 - EMA_026 as MA
into #a
from A.Daily_EMA where SymbolID = 12773
create clustered index #paa on #a(SymbolID, Seq)
select 
		Seq, Date, Closing, 
		EMA_003, EMA_012, EMA_026, EMA_050,
		A.Cal_Average(SymbolID,  Seq, null, 'Exponential', 3,'EMA_012 - EMA_026', '#a' ) as Signal,
		
		EMA_012 - EMA_026 as MA
from #a



kill 60
select Seq, Date, Closing, 
A.Cal_Average(SymbolID, Seq, null, 'Exponential', 20, 'Opening', default)
from A.Daily where SymbolID = 12773