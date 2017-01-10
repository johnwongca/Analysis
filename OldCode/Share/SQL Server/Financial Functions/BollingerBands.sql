use Stock
go
if OBJECT_ID('f.BollingerBands') is not null
	drop function f.BollingerBands;
go
/*ID column of @Data must be sequential number*/
create function f.BollingerBands (@Data f.Array1 readonly, @N float, @D Float = 1.8)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	V2 float, V3 float, V4 float,  Info xml)
begin

	insert into @Ret(ID, V1, V2, V3, V4, Info)
		select a.ID, B.V1 + a.V1 * @D, B.V1 - a.V1 * @D, b.V1, a.V1, a.Info 
		from f.StandardDeviation(@Data, @N) a
			inner join f.SimpleMovingAverage(@Data, @N) b on a.ID = b.ID
	return
end
go
declare @Data f.Array1
insert into @Data(V1, info) 
	select a.[Close], (select b.ExchangeID, b.Name as SymbolName, a.SymbolID ,a.High, a.[Open], a.[Close], a.[Low], a.Volume for xml raw, elements)
	from s.Daily a
		inner join s.Symbol b on a.SymbolID = b.SymbolID
	where b.Name = 'DLLR'
	order by Date asc
--select* from @Data
		
select * from f.BollingerBands(@Data, 14, default) a inner join @Data b on a.ID = b.ID

--select STDEVp(a) from (select 2 a union all select 4 union all select 4 union all select 4 union all select 5 union all select 5 union all select 7 union all select 9)a



