create procedure EODData.Download
(
	@DateFrom Date = null,
	@DateTo Date = null,
	@DataReload bit = 0
)
as
begin
	set nocount on
	declare @IsIntraDay bit
	declare @Exception table(Exchange varchar(10) primary key)
	insert into @Exception values('OPRA')
	insert into @Exception values('EUREX')
	insert into @Exception values('EURONEXT')
	if @DataReload = 1
	begin
		select @DateFrom = null, @DateTo = null
	end
	if @DateTo is null and @DateFrom is null
	begin
		select @DateTo = GETDATE()
		if DATENAME(weekday, @DateTo) in ('Saturday', 'Sunday') or @DataReload = 1
			select @DateFrom = DATEADD(MONTH, -1, @DateTo)
		else
			select @DateFrom = DATEADD(DAY, -4, @DateTo)
	end
	select @DateTo = isnull(@DateTo, GETDATE())
	if @DateFrom is null
	begin
		raiserror('DateFrom can not be empty unless DateTo is empty', 16, 1)
		return
	end

	truncate table EODData.Quote
	truncate table EODData.Fundamental

	declare @Date Datetime = @DateTo
	exec EODData.GetCountry
	exec EODData.GetExchange
	declare @Exchange varchar(10)
	declare c cursor local for
		select Exchange, IsIntraday from EODData.Exchange where Exchange not in (select Exchange from @Exception)
	open c
	fetch next from c into @Exchange, @IsIntraDay
	while @@fetch_status = 0
	begin
		exec EODData.GetSymbol @Exchange
		exec EODData.GetFundamental @Exchange, 0 -- run async
		select @Date = @DateTo
		while @Date>= @DateFrom
		begin
			--if @IsIntraDay = 1
				exec EODData.GetQuote @Exchange, 0, @Date, 0
			exec EODData.GetQuote @Exchange, 6, @Date, 0
			select @Date = DATEADD(DAY, -1, @Date)
		end
		exec EODData.GetSplit @Exchange, 0
		exec EODData.GetSymbolChange @Exchange, 0
		fetch next from c into @Exchange, @IsIntraDay
	end
	close c
	deallocate c		
	exec EODData.WaitForResult 0, null
end