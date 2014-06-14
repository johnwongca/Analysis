create table LoadFromSQL1.Symbol
(
SymbolID int not null,
Exchange varchar(10) not null,
Symbol varchar(32) not null,
SymbolName varchar(128),
SymbolLongName varchar(128), 
    CONSTRAINT [PK_LoadFromSQL1_Symbol] PRIMARY KEY (SymbolID)
)