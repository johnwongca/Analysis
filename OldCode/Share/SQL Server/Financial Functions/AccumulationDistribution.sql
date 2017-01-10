use Stock
go
if OBJECT_ID('f.AccumulationDistribution') is not null
	drop function f.AccumulationDistribution;
go
/*ID column of @Data must be sequential number*/
create function f.AccumulationDistribution (@Data f.Stock readonly)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	Info xml)
begin
	declare @ID int, @High float, @Open float, @Close float, @Low float, @Volume float, @Info xml
	declare @Total float = 0;
	declare c cursor static local for
		select ID, High, [Open], [Close], [Low], Volume, Info from @Data order by ID asc
	open c
	fetch next from c into @ID, @High, @Open, @Close, @Low, @Volume, @Info
	while @@fetch_status = 0
	begin
		select @Total = @Total + case when @High = @Low then 0 else (((@Close - @Low) - (@High - @Close)) * @Volume) / (@High-@Low) end
		insert into @Ret(ID, V1, Info)
			values(@ID, @Total, @Info)
		fetch next from c into @ID, @High, @Open, @Close, @Low, @Volume, @Info
	end
	close c
	deallocate c
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
select * from f.AccumulationDistribution(@Data) a inner join @Data b on a.ID = b.ID