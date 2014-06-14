CREATE TABLE [LoadFromSQL1].[QuoteMinute]
(
	SymbolID int not null,
	Date date not null,
	Time time(0) not null,
	[Open] float not null,
	High float not null,
	Low float not null,
	[Close] float not null,
	Volume bigint not null, 
    CONSTRAINT [PK_LoadFromSQL1_QuoteMinute] PRIMARY KEY (SymbolID, Date, Time)
)
