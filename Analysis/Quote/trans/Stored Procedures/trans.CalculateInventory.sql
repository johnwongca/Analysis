CREATE procedure trans.CalculateInventory 
(
	@SymbolID int
)
as
begin
	set nocount on
	declare @Date datetime, @Quantity int, @Price money, @Commission money, @Total money,
			@Position int, @Rate money, @Cost money, @ProfitLoss money,
			@PreviousPosition int = 0, @PreviousCost money = 0, @IsFirstRecord bit = 1
	declare c cursor local static for
		select Date, Quantity, Price, Commission 
		from trans.Inventory
		where SymbolID = @SymbolID
		order by Date asc
	open c
	fetch next from c into @Date, @Quantity, @Price, @Commission
	while @@fetch_status = 0
	begin
		select	@Total = @Quantity * @Price
		select	@Position = @PreviousPosition + @Quantity,
				@Cost = @PreviousCost + @Commission + @Total
		select @Rate = case when @Position = 0 then 0 else @Cost / @Position end

		update trans.Inventory
			set Total = @Total, Position = @Position, Rate = @Rate, Cost = @Cost
		where SymbolID = @SymbolID
			and Date = @Date
			and (Total <> @Total or Position <> @Position or Rate <> @Rate or Cost <> @Cost)
		select @PreviousPosition = @Position, @PreviousCost = @Cost
		fetch next from c into @Date, @Quantity, @Price, @Commission
	end
	close c
	deallocate c
end