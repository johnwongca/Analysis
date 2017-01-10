use Stock
go
if OBJECT_ID('f.Arron') is not null
	drop function f.Arron;
go
/*ID column of @Data must be sequential number*/
create function f.Arron (@Data f.Array1 readonly, @N float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	V2 float, Info xml)
begin

	insert into @Ret
	select a.ID, (@N - b.[Up])/@N, (@N - b.[Down])/@N, a.Info
	from @Data a
		cross apply(
						select cast(max(case when [Max] = V1 then MaxID - ID end) as float)[Up], cast(max(case when [Min] = V1 then MaxID - ID end ) as float)[Down]
						from (
								select c.ID , MAX(ID)over() MaxID, MAX(V1)over() [Max], MIN(V1)over() [Min], V1
								from @Data c
								where c.ID between a.ID - @N and a.ID
							)d
					)b		

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
select * from f.Arron(@Data, 14) a inner join @Data b on a.ID = b.ID



