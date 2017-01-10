use Stock
go
if OBJECT_ID('f.VolumnAdjustedMovingAverage') is not null
	drop function f.VolumnAdjustedMovingAverage;
go
/*ID column of @Data must be sequential number*/
create function f.VolumnAdjustedMovingAverage (@Data f.Stock readonly, @N float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	Info xml)
begin
	declare @tmp table (ID int NOT NULL primary key, V1 float NOT NULL)
	declare @TV1 float = 0, @TV2 float = 0
	declare @A float
	
	declare @ID int, @Volume float, @Close float, @Info xml
	declare c cursor static local for
		select ID, [Volume], [Close], Info from @Data order by ID asc
	open c
	fetch next from c into @ID, @Volume, @Close, @Info
	while @@fetch_status = 0
	begin
		set @TV1 = @TV1 + @Volume - isnull((select Volume from @Data where ID = @ID - cast(@N as int)), 0)
		set @A = @Volume * @Close
		insert into @tmp(ID, V1) values(@ID, @A)
		set @TV2 = @TV2 + @A - isnull((select V1 from @tmp where ID = @ID - cast(@N as int)), 0)
		insert into @Ret(ID, V1, Info)
			values(@ID, case when @TV1 = 0 then @Close else @TV2 / @TV1 end, @Info)
		fetch next from c into  @ID, @Volume, @Close, @Info
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
select * from f.VolumnAdjustedMovingAverage(@Data, 10) a inner join @Data b on a.ID = b.ID