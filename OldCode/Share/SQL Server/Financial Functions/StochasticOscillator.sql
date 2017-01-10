use Stock
go
if OBJECT_ID('f.StochasticOscillator') is not null
	drop function f.StochasticOscillator;
go
/*ID column of @Data must be sequential number*/
create function f.StochasticOscillator (@Data f.Stock readonly, @N float, @D float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	V2 float not null, Info xml)
begin
	declare @tmp f.Array1
	insert into @tmp(V1, Info)
	select case when b.Low = b.[High] then 0 else (a.[Close] - b.[Low]) * 100.0/(b.[High] - b.[Low]) end, a.Info
	from @Data a
		cross apply(
						select min([Low]) as [Low], max(High) as [High]
						from @Data c
						where c.ID between a.ID - cast(@N as int) + 1 and a.ID
				)b
	order by a.ID
	insert into @Ret(ID, V1, V2, Info)
		select a.ID, a.V1, b.V1, a.Info
		from @tmp a
			inner join f.ExponentialMovingAverage(@tmp, @D) b on a.ID = b.ID
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
select * from f.StochasticOscillator(@Data, 10, 3) a inner join @Data b on a.ID = b.ID