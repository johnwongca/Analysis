use Stock
go
if OBJECT_ID('f.WeightedMovingAverage') is not null
	drop function f.WeightedMovingAverage;
go
/*ID column of @Data must be sequential number*/
create function f.WeightedMovingAverage (@Data f.Array1 readonly, @N float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	Info xml)
begin
	declare @WMA float = 0, @Numerator float =0, @Denominator float = @N*(@N+1.0)/2.0, @Total float = 0, @First float = 0
	declare @ID int, @V1 float, @Info xml
	declare c cursor static local for
		select ID, V1, Info from @Data order by ID asc
	open c
	fetch next from c into @ID, @V1, @Info
	while @@fetch_status = 0
	begin
		select 
				@First = isnull((select V1 from @Data where ID = @ID - cast(@N as int)), 0),
				@Numerator = @Numerator + @V1 * @N - @Total
		select @WMA = @Numerator/@Denominator
		insert into @Ret(ID, V1, Info)
			values(@ID, @WMA, @Info)
			
		select @Total = @Total + @V1 - @First
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
select * from f.WeightedMovingAverage(@Data, 3) a inner join @Data b on a.ID = b.ID