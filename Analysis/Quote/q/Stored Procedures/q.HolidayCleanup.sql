CREATE procedure q.HolidayCleanup
as
begin
	set nocount on
	declare @Data table(Exchange varchar(10), Date date)
	insert into @Data(Exchange, Date)
	select s.Exchange, q.date
	from q.Symbol s
		inner join q.Quote q on q.SymbolID = s.SymbolID
	group by s.Exchange, q.date
	having sum(q.Volume) = 0



	declare @Exchange varchar(10), @Date date
	declare c cursor static local for
		select d.Exchange, d.Date
		from @Data d
		order by d.Date, d.Exchange
	open c
	--print cast(@@CURSOR_ROWS as varchar(30)) + ' Rows found.'
	fetch next from c into @Exchange, @Date
	while @@fetch_status = 0
	begin
		
		delete q
		from q.Symbol s with(tablock)
			inner loop join q.Quote q with(tablockx) on s.SymbolID = q.SymbolID
		where s.Exchange = @Exchange and Date = @Date
		--print cast(@@rowcount as varchar(max)) + '--' + @Exchange + '-'+ cast(@date as varchar(max))
		
		fetch next from c into @Exchange, @Date
	end
	close c
	deallocate c
	
end