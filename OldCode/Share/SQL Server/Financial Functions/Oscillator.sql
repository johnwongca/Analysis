use Stock
go
if OBJECT_ID('f.Oscillator') is not null
	drop function f.Oscillator;
go
/*ID column of @Data must be sequential number*/
create function f.Oscillator (@Data f.Array1 readonly, @NShort float, @NLong float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	V2 float, V3 float, Info xml)
begin
	insert into @Ret(ID, V1, V2, V3, Info)
	select a.ID, case when b.V1 = 0 then 0 else (100.00 * (a.V1 - b.V1))/b.V1 end, a.V1, b.V1, a.Info
	from f.ExponentialMovingAverage(@Data, @NShort) a
		inner join f.ExponentialMovingAverage(@Data, @NLong) b on a.ID = b.ID
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
select * from f.Oscillator(@Data, 10, 28) a inner join @Data b on a.ID = b.ID



