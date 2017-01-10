alter procedure STK.GetSecurity;1
(
	@ExchangeID int, 
	@SymbolName varchar(30), 
	@Date smalldatetime = '1970-01-01'
)
as
begin
	--declare @ExchangeID int, @SymbolName varchar(30), @Date smalldatetime
	--select @ExchangeID = 16, @SymbolName = 'DROOY', @Date = '2002-01-01'
	declare @temp table (OldName varchar(30), NewName varchar(30), Date datetime, [Level] int);
	with x(OldName, NewName, Date, [Level]) as
	(
		select 
			 a.OldName, a.NewName, a.Date, 0 as [Level]
		from STK.SymbolHistory a 
		where a.ExchangeID  = @ExchangeID
			and a.NewName = @SymbolName
			and a.Active = 1
		union all
		select 
			 a.OldName, a.NewName, a.Date, [Level]+1
		from STK.SymbolHistory a 
			inner join x on x.OldName = a.NewName
		where a.ExchangeID  = @ExchangeID
			and x.Date >= a.Date
			and a.Active = 1
	)
	insert into @temp(OldName, NewName, Date, Level)
	select OldName, [NewName], Date, [Level]  from x
	if exists(select 1 from @temp)
	begin
		select 
			row_number() over(order by a.Date asc) RowNum, a.Date, cast(replace(convert(varchar(10), a.Date, 120), '-', '') as int) DateInt, a.ID, a.SymbolID, b.Name as SymbolName, a.Opening, a.High, a.Low, a.Closing, a.Volume, a.Interest, s.ID as SplitID, s.[From], s.[To]
		from STK.Daily a
			inner join STK.Symbol b on a.SymbolID = b.ID
			left outer join STK.Split s on s.SymbolID = a.SymbolID and a.Date = s.Date
		where b.ExchangeID = @ExchangeID
			and exists(select 1 from @temp t where t.OldName = b.Name or t.NewName = b.Name)
			and a.Date > @Date 
		order by a.Date asc
		return
	end
	select 
		row_number() over(order by a.Date asc) RowNum, a.Date, cast(replace(convert(varchar(10), a.Date, 120), '-', '') as int) DateInt, a.ID, a.SymbolID, b.Name as SymbolName, a.Opening, a.High, a.Low, a.Closing, a.Volume, a.Interest, s.ID as SplitID, s.[From], s.[To]
	from STK.Daily a
		inner join STK.Symbol b on a.SymbolID = b.ID
		left outer join STK.Split s on s.SymbolID = a.SymbolID and a.Date = s.Date
	where b.ExchangeID = @ExchangeID
		and b.Name = @SymbolName
		and a.Date > @Date 
	order by a.Date asc
end
go
alter procedure STK.GetSecurity;2
(
	@SymbolID int, 
	@Date smalldatetime = '1970-01-01'
)
as
begin
	declare @ExchangeID int, @SymbolName varchar(30)	
	select @ExchangeID = ExchangeID, @SymbolName = Name from STK.Symbol where ID = @SymbolID;
	exec STK.GetSecurity;1 @ExchangeID, @SymbolName, @Date
end
go