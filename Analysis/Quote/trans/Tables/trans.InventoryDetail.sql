CREATE TABLE [trans].[InventoryDetail] (
    [InventoryID] INT      NOT NULL,
    [Date]        DATETIME NOT NULL,
    [IsSell]      BIT      NOT NULL,
    [Quantity]    INT      NOT NULL,
    [Price]       MONEY    NOT NULL,
    [Total]       MONEY    NULL,
    [Commission]  MONEY    NULL,
    [Rate]        MONEY    NULL,
    [Position]    INT      NULL,
    [IsShort]     BIT      NULL,
    [Cost]        MONEY    NULL,
    CONSTRAINT [PK_trans_InventoryDetail] PRIMARY KEY CLUSTERED ([InventoryID] ASC, [Date] DESC),
    CONSTRAINT [FK_trans_InventoryDetail_trans_Inventory] FOREIGN KEY ([InventoryID]) REFERENCES [trans].[Inventory] ([InventoryID])
);

