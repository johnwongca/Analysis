CREATE TABLE [trans].[InventoryBackup] (
    [SymbolID]   INT      NOT NULL,
    [Date]       DATETIME NOT NULL,
    [Quantity]   INT      NOT NULL,
    [Price]      MONEY    NOT NULL,
    [Total]      MONEY    NOT NULL,
    [Commission] MONEY    NOT NULL,
    [Position]   INT      NOT NULL,
    [Rate]       MONEY    NOT NULL,
    [Cost]       MONEY    NOT NULL
);

