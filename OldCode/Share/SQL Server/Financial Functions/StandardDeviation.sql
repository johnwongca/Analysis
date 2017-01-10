use Stock
go
if OBJECT_ID('f.StandardDeviation') is not null
	drop function f.StandardDeviation;
go
/*ID column of @Data must be sequential number*/
create function f.StandardDeviation (@Data f.Array1 readonly, @N float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	Info xml)
begin
	insert into @Ret(ID, V1, Info)
	select a.ID, isnull(b.V1, 0), a.Info
	from @Data a
		cross apply(select stdev(V1) V1 from @Data b where b.ID between a.ID - cast(@N as int)+1 and a.ID) b
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
		
select * from f.StandardDeviation(@Data, 14) a inner join @Data b on a.ID = b.ID



