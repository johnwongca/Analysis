CREATE procedure [chart].[GetSourcedata](@SymbolID int = 170061, @Interval int = 2880)
as
begin
	if (@Interval in (1, 5, 10, 15, 30, 60))
	begin
		select	Date,
				case when @Interval < 60 then
						convert(varchar(20), cast(Date as Date), 120) + ' ' + cast(Time as varchar(5)) 
					else
						convert(varchar(20), cast(Date as Date), 120) + ' ' + cast(Time as varchar(2)) 
				end as XDisplay,
				q.[Open], q.[High], q.[Low], q.[Close], q.[Volume]
		from (
				select 
						dateadd(second, datediff(second, '00:00:00', qd.Time), cast(q.Date as datetime)) Date,
						qd.Time,
						qd.[Open], qd.[High], qd.[Low], qd.[Close], qd.[Volume]
				from q.Quote q
					inner join q.QuoteDetail qd on q.QuoteID = qd.QuoteID
				where q.SymbolID = @SymbolID 
					and qd.Type = CAST(@Interval as tinyint)
			)q
		order by q.Date
	end
	else if(@Interval = 1440)
	begin
		select 
				cast(q.Date as datetime) Date,
				convert(varchar(20), cast(Date as Date), 120) as XDisplay,
				q.[Open], q.[High], q.[Low], q.[Close], q.[Volume]
		from(
				select	
						lag(Date) over( order by Date) PreviousDate,
						*
				from q.Quote q
				where q.SymbolID = @SymbolID 
			) q
		order by q.Date
		
	end
	else if(@Interval % 1440 = 0)
	begin
		declare @Days int = @Interval / 1440
		;with q1 as
		(
			select	
				(row_number() over(order by Date desc)+ @Days-1)/@Days PID,
				row_number() over(order by Date desc) RID,
				*
			from q.Quote q
			where q.SymbolID = @SymbolID 
		),
		q as (select 
				PID,
				first_value(q1.Date) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) Date,
				first_value(q1.Date) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) DateFrom,
				last_value(q1.Date) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) DateTo,
				Max(q1.High) over(partition by PID) [High],
				last_value(q1.[Open]) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) [Open],
				first_value(q1.[Close]) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) [Close],
				min(q1.[Low]) over(partition by PID) [Low],
				sum(q1.[Volume]) over(partition by PID) [Volume],
				count(*) over(partition by PID) [Days]
		from q1
		)
		select distinct 
				cast(q.Date as datetime) Date,
				convert(varchar(20), cast(DateFrom as Date), 120) + '~' + convert(varchar(20), cast(DateTo as Date), 120) as XDisplay,
				q.[Open], q.[High], q.[Low], q.[Close], q.[Volume], Days
		from q
		order by Date ASC
	end
	else if @Interval in(43201, 86401, 129601)
	begin
		declare @month int = iif(@Interval = 43201, 1, iif(@Interval = 86401, 2, 3))
		;with q1 as
		(
			select	
				year(Date) *100 + month(Date)/@month PID,
				Date RID,
				*
			from q.Quote q
			where q.SymbolID = @SymbolID 
		),
		q as (select 
				PID,
				first_value(q1.Date) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) Date,
				first_value(q1.Date) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) DateFrom,
				last_value(q1.Date) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) DateTo,
				Max(q1.High) over(partition by PID) [High],
				last_value(q1.[Open]) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) [Open],
				first_value(q1.[Close]) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) [Close],
				min(q1.[Low]) over(partition by PID) [Low],
				sum(q1.[Volume]) over(partition by PID) [Volume],
				count(*) over(partition by PID) [Days]
		from q1
		)
		select distinct 
				cast(q.Date as datetime) Date,
				convert(varchar(20), cast(DateFrom as Date), 120) + '~' + convert(varchar(20), cast(DateTo as Date), 120) as XDisplay,
				q.[Open], q.[High], q.[Low], q.[Close], q.[Volume], Days
		from q
		order by Date ASC
	end
	else if @Interval in(7201, 14401, 21601)
	begin
		declare @wk int = iif(@Interval = 7201, 1, iif(@Interval = 14401, 2, 3)) * 7
		;with q1 as
		(
			select	
				datediff(day, '1900-01-07', Date) / @wk PID,
				Date RID,
				*
			from q.Quote q
			where q.SymbolID = @SymbolID 
		),
		q as (select 
				PID,
				first_value(q1.Date) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) Date,
				first_value(q1.Date) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) DateFrom,
				last_value(q1.Date) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) DateTo,
				Max(q1.High) over(partition by PID) [High],
				last_value(q1.[Open]) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) [Open],
				first_value(q1.[Close]) over(partition by PID order by RID ASC rows Between UNBOUNDED PRECEDING and UNBOUNDED FOLLOWING) [Close],
				min(q1.[Low]) over(partition by PID) [Low],
				sum(q1.[Volume]) over(partition by PID) [Volume],
				count(*) over(partition by PID) [Days]
		from q1
		)
		select distinct 
				cast(q.Date as datetime) Date,
				convert(varchar(20), cast(DateFrom as Date), 120) + '~' + convert(varchar(20), cast(DateTo as Date), 120) as XDisplay,
				q.[Open], q.[High], q.[Low], q.[Close], q.[Volume], Days
		from q
		order by Date ASC
	end
end
