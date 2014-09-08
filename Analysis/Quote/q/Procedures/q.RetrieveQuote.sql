create procedure q.RetrieveQuote
(
	@SymbolID int = 170976,  --msft
	@IsEOD bit = 1, --Minutes = 0, Days = 1
	@DateFrom date = '2000-01-01',
	@DateTo date = '2100-01-01'
)
as
begin
	set nocount on 
	if @IsEOD = 0
		select	Datepart(isowk, Date) ISOWeek, dateadd(minute, datediff(minute, '00:00:00', Time ) ,cast(Date as datetime)) Date, 
				[Open], [High], [Low], [Close], Volume 
		from q.QuoteMinute  q
		where SymbolID = @SymbolID 
			and Date between @DateFrom and @DateTo
		order by q.Date, q.Time
	else
		select	Datepart(isowk, Date) ISOWeek, cast(Date as datetime) date, 
				[Open], [High], [Low], [Close], Volume 
		from q.Quote q
		where SymbolID = @SymbolID 
			and Date between @DateFrom and @DateTo
		order by q.Date
end