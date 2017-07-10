CREATE procedure [EODData].[LoadQuoteFiles]
as
begin
	set nocount, xact_abort on
	select * into #PendingQuote
	from [EODData].[PendingQuoteFile]
	declare @Exchange varchar(10), @ExchangeName varchar(128)
	insert into EODData.vSymbolChange(Date, FromExchange, FromSymbol, ToExchange, ToSymbol)
		select Date, FromExchange, FromSymbol, ToExchange, ToSymbol
		from EODData.ReadSymbolChange()
		
	declare c1 cursor local static for
		select Code, Name from EODData.ReadExchange() where Code not in ('OPRA', 'USMF')
	open c1
	fetch next from c1 into @Exchange, @ExchangeName
	while @@fetch_status = 0
	begin
		--begin transaction
		--exchange
		merge EODData.Exchange t
		using (select @Exchange as Exchange) s on t.Exchange = s.Exchange
		when not matched then
			insert (Exchange, Name) values(s.Exchange, @ExchangeName)
		when matched and t.Name<> @ExchangeName then
			update set t.name = @ExchangeName
		;
		-- import Symbols
		insert into EODData.vSymbol(Exchange, Symbol, Name, LongName, Date)
			select @Exchange, Symbol, Name, Name, Getdate()
			from EODData.ReadSymbol(@Exchange)

		insert into EODData.vSymbol(Exchange, Symbol, Name, LongName, Date)
			select @Exchange, Symbol, Name, Name, GetDate()
			from EODData.ReadFundamental(@Exchange)
		-- -- import split
		insert into EODData.vSplit(Exchange, Symbol, Date, Ratio)
			select @Exchange, Symbol, Date, Ratio
			from [EODData].[ReadSplit] (@Exchange)
			where Ratio is not null
		insert into EODData.vQuote(Exchange, Symbol, IntervalID, Date, [Open], [Close], High, Low, Volume, Ask, Bid, OpenInterest)
			select a.Exchange, b.Symbol, 6, a.Date, b.[Open], b.[Close], b.High, b.Low, b.Volume, 0, 0, 0
			from #PendingQuote a
				cross apply EODData.ReadQuote(a.Exchange, a.Date) b
			where Exchange = @Exchange
		insert into EODData.LoadedQuote(Exchange, Date, FullFileName, FileName, CreationDate, LastModifiedDate, Length)
			select Exchange, Date, FullFileName, FileName, CreationDate, LastModifiedDate, Length
			from #PendingQuote
			where Exchange = @Exchange
		fetch next from c1 into @Exchange, @ExchangeName
	end
	close c1
	deallocate c1
end