use Stock
go
if OBJECT_ID('f.RelativeVolatilityIndex') is not null
	drop function f.RelativeVolatilityIndex;
go
/*ID column of @Data must be sequential number*/
create function f.RelativeVolatilityIndex (@Data f.Stock readonly, @N float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	Info xml)
begin
	declare @U f.Array1, @D f.Array1
	insert into @U(V1, Info)
		select [High], Info
		from @Data
		order by ID
	insert into @D(V1)
		select [Low]
		from @Data
		order by ID
	insert into @Ret(ID, V1, Info)		
	select	u.ID, 
			case when d.V1 = 0 then 100 else 100.0 - (100.0/(1 + u.V1/d.V1)) end as V1,
			u.Info
	from f.StandardDeviation(@U, @N) u
		inner join f.StandardDeviation(@D, @N) d on u.ID = d.ID
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
select * from f.RelativeVolatilityIndex(@Data, 10) a inner join @Data b on a.ID = b.ID