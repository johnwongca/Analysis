CREATE TABLE [trans].[Inventory] (
    [SymbolID]   INT      NOT NULL,
    [Date]       DATETIME NOT NULL,
    [Quantity]   INT      NOT NULL,
    [Price]      MONEY    NOT NULL,
    [Total]      MONEY    NOT NULL,
    [Commission] MONEY    NOT NULL,
    [Position]   INT      NOT NULL,
    [Rate]       MONEY    NOT NULL,
    [Cost]       MONEY    NOT NULL,
    CONSTRAINT [PK_trans_Inventory] PRIMARY KEY CLUSTERED ([SymbolID] ASC, [Date] DESC),
    CONSTRAINT [FK_trans_Inventory_q_Symbol] FOREIGN KEY ([SymbolID]) REFERENCES [q].[Symbol] ([SymbolID])
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
