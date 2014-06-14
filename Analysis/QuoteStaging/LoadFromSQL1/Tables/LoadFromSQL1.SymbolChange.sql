CREATE TABLE [LoadFromSQL1].[SymbolChange]
(
	Date date not null,
	FromSymbolID int  not null,
	ToSymbolID int  not null, 
    CONSTRAINT [PK_LoadFromSQL1_SymbolChange] PRIMARY KEY ([Date],[FromSymbolID] ASC,[ToSymbolID] ASC)
)
