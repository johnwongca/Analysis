use Stock
go
if OBJECT_ID('f.MoneyFlowIndex') is not null
	drop function f.MoneyFlowIndex;
go
/*ID column of @Data must be sequential number*/
create function f.MoneyFlowIndex (@Data f.Stock readonly, @N float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	Info xml)
begin
	declare @Data1 f.Array1 
	insert into @Data1 (V1, info)
		select ([High] + [Low] + [Close])* Volume/3.0, info
		from @Data
		order by ID
	insert into @Ret
		select ID, V1, Info
		from f.RelativeStrengthIndex(@Data1, @N)
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
select * from f.MoneyFlowIndex(@Data, 14) a inner join @Data b on a.ID = b.ID