ALTER function A.GetSecurity
(
	@SymbolID int, 
	@Date smalldatetime = '1970-01-01'
) returns @ret table (
						Date		Datetime,
						Opening		Float,
						High		Float,
						Low			Float,
						Closing		Float,
						Volume		Float,
						Interest	Float
				)
as
begin
	declare @ExchangeID int, @SymbolName varchar(30)
	select @ExchangeID = ExchangeID, @SymbolName = Name from STK.Symbol where ID = @SymbolID;

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
		declare @tt table(ID int)
		insert into @tt
			select ID
			from STK.Symbol b
			where exists(select 1 from @temp t where t.OldName = b.Name or t.NewName = b.Name)
				and b.ExchangeID = @ExchangeID
		insert into @ret(Date, Opening, High, Low, Closing, Volume, Interest)
			select 
						a.Date, a.Opening, a.High, a.Low, a.Closing, a.Volume, a.Interest
			from STK.Daily a
			where a.SymbolID in (select ID from @tt)
				and a.Date > @Date 
			order by a.Date asc
		return
	end
	insert into @ret(Date, Opening, High, Low, Closing, Volume, Interest)
		select 
			a.Date, a.Opening, a.High, a.Low, a.Closing, a.Volume, a.Interest
		from STK.Daily a
		where a.SymbolID =@SymbolID
			and a.Date > @Date 
		order by a.Date asc
	return
end
