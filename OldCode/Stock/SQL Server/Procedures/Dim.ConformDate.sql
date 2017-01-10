alter procedure DIM.ConformDate
as
begin 
	declare @YearFrom smallint, @YearTo smallint, @DateFrom datetime, @DateTo datetime, @i int
	select @DateFrom = DIM.DateOnly(min(Date)), @DateTo = DIM.DateOnly(Max(Date)) from A.Daily
	if @DateFrom is null
		return;
	select @YearFrom = datepart(year, @DateFrom), @YearTo = datepart(year, @DateTo)
	while @YearFrom < = @YearTo
	begin
		if not exists(select 1 from DIM.A_Year where ID = @YearFrom)
			insert into DIM.A_Year(ID, [Name], SortOrder) values(@YearFrom, @YearFrom, @YearFrom)
		select @YearFrom = @YearFrom +1
	end
	if not exists( select 1 from DIM.A_Month where ID = 1)	insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (1, 'January', 'Jan', '01', 1)
	if not exists( select 1 from DIM.A_Month where ID = 2)	insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (2, 'Feburary', 'Feb', '02', 2)
	if not exists( select 1 from DIM.A_Month where ID = 3)	insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (3, 'March', 'Mar', '03', 3)
	if not exists( select 1 from DIM.A_Month where ID = 4)	insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (4, 'April', 'Apr', '04', 4)
	if not exists( select 1 from DIM.A_Month where ID = 5)	insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (5, 'May', 'May', '05', 5)
	if not exists( select 1 from DIM.A_Month where ID = 6)	insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (6, 'June', 'Jun', '06', 6)
	if not exists( select 1 from DIM.A_Month where ID = 7)	insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (7, 'July', 'Jul', '07', 7)
	if not exists( select 1 from DIM.A_Month where ID = 8)	insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (8, 'August', 'Aug', '08', 8)
	if not exists( select 1 from DIM.A_Month where ID = 9)	insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (9, 'September', 'Sept', '09', 9)
	if not exists( select 1 from DIM.A_Month where ID = 10)	insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (10, 'October', 'Oct', '10', 10)
	if not exists( select 1 from DIM.A_Month where ID = 11) insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (11, 'November', 'Nov', '11', 11)
	if not exists( select 1 from DIM.A_Month where ID = 12) insert into DIM.A_Month (ID, Name, ShortName1, ShortName2, SortOrder) values (12, 'December', 'Dec', '12', 12)
	select @i = 1
	while @i<=4
	begin	
		if not exists( select 1 from DIM.A_Quarter where ID = cast(@i as tinyint))	
			insert into DIM.A_Quarter (ID, Name, SortOrder) values (cast(@i as tinyint), 'Q'+cast(@i as varchar(1)), cast(@i as tinyint))
		select @i = @i +1
	end
	select @i = 1
	while @i <= 366
	begin
		if not exists( select 1 from DIM.A_YearDay where ID = cast(@i as smallint))	
			insert into DIM.A_YearDay (ID, Name, SortOrder) values (cast(@i as smallint), right('000' + cast(@i as varchar(3)), 3), cast(@i as smallint))
		select @i = @i + 1
	end
	select @i = 1
	while @i <= 31
	begin
		if not exists( select 1 from DIM.A_Day where ID = cast(@i as smallint))	
			insert into DIM.A_Day (ID, Name, SortOrder) values (cast(@i as smallint), right('00' + cast(@i as varchar(2)), 2), cast(@i as smallint))
		select @i = @i + 1
	end
	select @i = 1
	while @i <=54
	begin
		if not exists( select 1 from DIM.A_Week where ID = cast(@i as smallint))	
			insert into DIM.A_Week (ID, Name, SortOrder) values (cast(@i as smallint), right('00' + cast(@i as varchar(2)), 2), cast(@i as smallint))
		select @i = @i + 1
	end
	if not exists( select 1 from DIM.A_WeekDay where ID = 1)	insert into DIM.A_WeekDay (ID, Name, ShortName1, ShortName2, SortOrder) values (1, 'Monday', 'Mon', '01', 1)
	if not exists( select 1 from DIM.A_WeekDay where ID = 2)	insert into DIM.A_WeekDay (ID, Name, ShortName1, ShortName2, SortOrder) values (2, 'Tuesday', 'Tue', '02', 2)
	if not exists( select 1 from DIM.A_WeekDay where ID = 3)	insert into DIM.A_WeekDay (ID, Name, ShortName1, ShortName2, SortOrder) values (3, 'Wednesday', 'Wed', '03', 3)
	if not exists( select 1 from DIM.A_WeekDay where ID = 4)	insert into DIM.A_WeekDay (ID, Name, ShortName1, ShortName2, SortOrder) values (4, 'Thursday', 'Thu', '04', 4)
	if not exists( select 1 from DIM.A_WeekDay where ID = 5)	insert into DIM.A_WeekDay (ID, Name, ShortName1, ShortName2, SortOrder) values (5, 'Friday', 'Fri', '05', 5)
	if not exists( select 1 from DIM.A_WeekDay where ID = 6)	insert into DIM.A_WeekDay (ID, Name, ShortName1, ShortName2, SortOrder) values (6, 'Saturday', 'Sat', '06', 6)
	if not exists( select 1 from DIM.A_WeekDay where ID = 7)	insert into DIM.A_WeekDay (ID, Name, ShortName1, ShortName2, SortOrder) values (7, 'Sunday', 'Sun', '07', 7)
	set datefirst 1
	while @DateFrom <= @DateTo
	begin
		if not exists(select 1 from DIM.Date where Date = @DateFrom)
			insert into DIM.Date(
									Date,		Name,		YearID, 
									MonthID,	DayID,		QuarterID, 
									WeekID,		WeekDayID,	YearDayID, 
									HolidayID,	SortOrder
								)
				Values(
									@DateFrom, convert(varchar(10), @DateFrom, 120), Year(@DateFrom),
									Month(@DateFrom), Day(@DateFrom), case when Month(@DateFrom) in (1, 2, 3) then 1 when Month(@DateFrom) in (4, 5, 6) then 2 when Month(@DateFrom) in (7, 8, 9) then 3 else 4 end,
									DatePart(week, @DateFrom), DATEPART(weekday, @DateFrom), datediff(day, convert(datetime, datename(year, @DateFrom)+'-01-01', 120), @DateFrom)+1,
									-1, @DateFrom
						)
		select @DateFrom = dateadd(day, 1, @DateFrom )
	end
end
go
--truncate table DIM.Date
exec DIM.ConformDate
select * from DIM.Date
update DIM.Date set SortOrder = Date
--select * from DIM.A_Day
--select * from DIM.A_WeekDay


