use Stock
go
if OBJECT_ID('f.StochasticMomentumIndex') is not null
	drop function f.StochasticMomentumIndex;
go
/*ID column of @Data must be sequential number*/
create function f.StochasticMomentumIndex (@Data f.Stock readonly, @N float, @D float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	V2 float not null, Info xml)
begin
	declare @Numerator f.Array1, @Denominator f.Array1
	insert into @Denominator(V1, Info)
	select b.[High] - b.[Low], a.Info
	from @Data a
		cross apply(
						select min([Low]) as [Low], max(High) as [High]
						from @Data c
						where c.ID between a.ID - cast(@N as int) + 1 and a.ID
				)b
	order by a.ID

	insert into @Numerator(V1)
	select (a.[Close] - (b.[High] + b.[Low])/2)
	from @Data a
		cross apply(
						select min([Low]) as [Low], max(High) as [High]
						from @Data c
						where c.ID between a.ID - cast(@N as int) + 1 and a.ID
				)b
	order by a.ID
	
	update a
		set a.V1 = b.V1
	from @Numerator a
		inner join f.ExponentialMovingAverage(@Numerator, @N) b on a.ID = b.ID
	update a
		set a.V1 = b.V1 * 100.00
	from @Numerator a
		inner join f.ExponentialMovingAverage(@Numerator, @N) b on a.ID = b.ID
		
	update a
		set a.V1 = b.V1
	from @Denominator a
		inner join f.ExponentialMovingAverage(@Denominator, @N) b on a.ID = b.ID
		
	update a
		set a.V1 = b.V1 / 2.0
	from @Denominator a
		inner join f.ExponentialMovingAverage(@Denominator, @N) b on a.ID = b.ID
	
	update a
		set a.V1 = case when b.V1 = 0 then 0 else a.V1 / b.V1 end
	from @Numerator a
		inner join @Denominator b on a.ID = b.ID
	
	insert into @Ret(ID, V1, V2, Info)
		select a.ID, a.V1, b.V1, a.Info
		from @Numerator a
			inner join f.ExponentialMovingAverage(@Numerator, @D) b on a.ID = b.ID
	return
end
go
declare @Data f.Stock
insert into @Data(Date, High, [Open], [Close], [Low], Volume, info) 
	select a.Date, a.High, a.[Open], a.[Close], a.[Low], a.Volume, (select b.ExchangeID, b.Name as SymbolName, a.SymbolID ,a.High, a.[Open], a.[Close], a.[Low], a.Volume for xml raw, elements)
	from s.Daily a
		inner join s.Symbol b on a.SymbolID = b.SymbolID
	where b.Name = 'DLLR'
order by Date asc
--select* from @Data
select * from f.StochasticMomentumIndex(@Data, 10, 3) a inner join @Data b on a.ID = b.ID

