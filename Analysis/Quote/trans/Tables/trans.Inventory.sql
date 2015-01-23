CREATE TABLE [trans].[Inventory] (
    [InventoryID] INT          IDENTITY (1, 1) NOT NULL,
    [SymbolID]    INT          NOT NULL,
    [Exchange]    VARCHAR (10) NOT NULL,
    [Symbol]      VARCHAR (32) NOT NULL,
    [OpenDate]    DATETIME     NULL,
    [CloseDate]   DATETIME     NULL,
    [GrossEarn]   MONEY        NULL,
    [Commission]  MONEY        NULL,
    [NetEarn]     MONEY        NULL,
    [TradeCount]  INT          NULL,
    [Position]    INT          NULL,
    [IsShort]     BIT          NULL,
    [Rate]        AS           (case when [Position]=(0) then (0) else [Cost]/[Position] end),
    [Cost]        MONEY        NULL,
    CONSTRAINT [PK_Trans_Inventory] PRIMARY KEY CLUSTERED ([InventoryID] ASC),
    CONSTRAINT [UK_trans_Inventory] UNIQUE NONCLUSTERED ([Exchange] ASC, [Symbol] ASC, [CloseDate] ASC)
);






GO

--CREATE TRIGGER [trans].[TRI_trans_Inventory]ON [trans].[Inventory]
--FOR DELETE, INSERT, UPDATE
--AS
--BEGIN
--	if @@rowcount = 0
--		return
--    SET NoCount ON
--	declare @iSymbolID int, @iDate datetime, @iIsSell bit, @iQuantity int, @iPrice money, @iCommission money,
--			@dSymbolID int, @dDate datetime, @dIsSell bit, @dQuantity int, @dPrice money, @dCommission money
--	declare c cursor local for
--		select	i.SymbolID, i.Date, i.IsSell, i.Quantity, i.Price, i.Commission,
--				d.SymbolID, d.Date, d.IsSell, d.Quantity, d.Price, d.Commission
--		from inserted i
--			full outer join deleted d on i.SymbolID = D.SymbolID and i.Date= d.Date
--	fetch next from c into @iSymbolID, @iDate, @iIsSell, @iQuantity, @iPrice, @iCommission,@dSymbolID, @dDate, @dIsSell, @dQuantity, @dPrice, @dCommission
--	while @@fetch_status = 0
--	begin
--		fetch next from c into @iSymbolID, @iDate, @iIsSell, @iQuantity, @iPrice, @iCommission,@dSymbolID, @dDate, @dIsSell, @dQuantity, @dPrice, @dCommission
--	end
--	close c
--	deallocate c
--END
--go
