CREATE procedure [trans].[CalculateInventory] 
(
	@InventoryID int,
	@CloseInventory bit = 0
)
as
begin
	set nocount on
	declare @Date datetime, @Quantity int, @Price money, @Commission money, @Total money,
			@Position int, @Rate money, @Cost money, @ProfitLoss money,
			@PreviousPosition int = 0, @PreviousCost money = 0, @IsFirstRecord bit = 1, @IsSell bit,
			@Sign int, @Count int = 0;
	declare c cursor local static for
		select Date, Quantity, Price, Commission, IsSell
		from trans.InventoryDetail
		where InventoryID = @InventoryID
		order by Date asc
	open c
	fetch next from c into @Date, @Quantity, @Price, @Commission, @IsSell
	while @@fetch_status = 0
	begin
		select @Sign = case when @IsSell = 1 then -1 else 1 end, @Count = @Count + 1
		select	@Total = @Quantity * @Price * @Sign
		select	@Position = @PreviousPosition + @Quantity * @Sign,
				@Cost = @PreviousCost + @Commission + @Total
		select @Rate = case when @Position = 0 then 0 else @Cost / @Position end

		update trans.InventoryDetail
			set Total = abs(@Total), Position = abs(@Position), Rate = abs(@Rate), Cost = abs(@Cost), IsShort = case when @Position > =0 then 0 else 1 end
		where InventoryID = @InventoryID
			and Date = @Date
		select @PreviousPosition = @Position, @PreviousCost = @Cost
		fetch next from c into @Date, @Quantity, @Price, @Commission, @IsSell
	end
	close c
	deallocate c
	update i
		set CloseDate = case when @CloseInventory = 1 then id.CloseDate end,
			OpenDate = id.OpenDate,
			GrossEarn = case when @CloseInventory = 1 then id.GrossEarn end,
			Commission = id.Commission,
			NetEarn = case when @CloseInventory = 1 then id.NetEarn end,
			TradeCount = id.TradeCount,
			Position = id.Position,
			IsShort = id.IsShort,
			Cost = id.NetEarn
	from trans.Inventory i
		cross join (select 
						min(Date) OpenDate, 
						Max(Date) CloseDate, 
						sum( case when IsSell = 1 then 1 else -1 end * Quantity* Price) GrossEarn,
						sum(Commission) Commission,
						sum( case when IsSell = 1 then 1 else -1 end * Quantity* Price) - sum(Commission) NetEarn,
						abs(sum( case when IsSell = 1 then 1 else -1 end * Quantity)) Position,
						case when sum( case when IsSell = 1 then 1 else -1 end * Quantity) > 0 then 1 else 0 end IsShort,
						Count(*) TradeCount
					from trans.InventoryDetail where InventoryID = @InventoryID) id
	where i.InventoryID = @InventoryID
	delete trans.Inventory where InventoryID = @InventoryID and TradeCount = 0
end