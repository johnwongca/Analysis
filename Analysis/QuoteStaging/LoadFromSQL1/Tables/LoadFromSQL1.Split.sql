CREATE TABLE [LoadFromSQL1].[Split]
(
	SymbolID int not null,
Date date not null,
Ratio varchar(10), 
    CONSTRAINT [PK_LoadFromSQL1_Split] PRIMARY KEY (SymbolID, Date)
)
