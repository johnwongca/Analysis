CREATE TABLE [LoadFromSQL1].[Quote]
(
	SymbolID int not null,
	Date date not null,
	[Open] float not null,
	High float not null,
	Low float not null,
	[Close] float not null,
	Volume bigint not null, 
    CONSTRAINT [PK_LoadFromSQL1_Quote] PRIMARY KEY (SymbolID, Date)
)
