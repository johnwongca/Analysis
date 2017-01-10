alter procedure A.Sync
as
begin
	set nocount on
	declare @ExchangeID int, @SymbolID int, @d datetime
	declare @Date datetime, @Opening Float, @High Float, @Low Float, @Closing Float, @Volume Float, @Interest Float
	declare cExchange cursor local static for
		select ID
		from STK.Exchange
		where Name = 'NASDAQ'
	open cExchange
	fetch next from cExchange into @ExchangeID
	while @@fetch_status = 0
	begin
		declare cSymbol cursor local static for
			select ID
			from STK.Symbol
			where ExchangeID = @ExchangeID
		open cSymbol
		fetch next from cSymbol into @SymbolID
		while @@fetch_Status = 0
		begin
			set @d = isnull((select max(date) from a.Daily where SymbolID = @SymbolID), '1970-01-01')
			begin try
				declare cSecurity cursor local static for
					select Date, Opening, High, Low, Closing, Volume, Interest
					from A.GetSecurity(@SymbolID, @d)
			end try
			begin catch
				print A.GetErrorMessage()
				continue;
			end catch 
			open cSecurity
			fetch next from cSecurity into @Date, @Opening, @High, @Low, @Closing, @Volume, @Interest
			while @@fetch_status = 0
			begin
				exec A.DailyWrite @SymbolID, @Date, @Opening, @High, @Low, @Closing, @Volume, @Interest, 1
				fetch next from cSecurity into @Date, @Opening, @High, @Low, @Closing, @Volume, @Interest
			end
			close cSecurity
			deallocate cSecurity
			fetch next from cSymbol into @SymbolID
		end
		close cSymbol
		deallocate cSymbol
		fetch next from cExchange into @ExchangeID
	end
	close cExchange
	deallocate cExchange
end