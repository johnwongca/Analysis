use Stock
go
if OBJECT_ID('f.ExponentialMovingAverage') is not null
	drop function f.ExponentialMovingAverage;
go
/*ID column of @Data must be sequential number*/
create function f.ExponentialMovingAverage (@Data f.Array1 readonly, @N float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	Info xml)
begin
	declare @EMA float = 0, @A float = 2.0/(1.0+@N)
	declare @ID int, @V1 float, @Info xml
	declare c cursor static local for
		select ID, V1, Info from @Data order by ID asc
	open c
	fetch next from c into @ID, @V1, @Info
	while @@fetch_status = 0
	begin
		select @EMA = @A * @V1 + (1.0 - @A) * @EMA
		insert into @Ret(ID, V1, Info)
			values(@ID, @EMA, @Info)
		fetch next from c into @ID, @V1, @Info
	end
	close c
	deallocate c
	return
end
go
declare @Data f.Array1
insert into @Data(V1) values(7),(6),(5),(4),(3),(2),(1)
--select * from @Data
select * from f.ExponentialMovingAverage(@Data, 3) a inner join @Data b on a.ID = b.ID