use Stock
go
if OBJECT_ID('f.WilderSmoothing') is not null
	drop function f.WilderSmoothing;
go
/*ID column of @Data must be sequential number*/
create function f.WilderSmoothing (@Data f.Array1 readonly, @N float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	Info xml)
begin
	declare @SMAm float = 0, @PreviousMA float = 0, @WS float = 0
	declare @ID int, @V1 float, @Info xml
	declare c cursor static local for
		select ID, V1, Info from @Data order by ID asc
	open c
	fetch next from c into @ID, @V1, @Info
	while @@fetch_status = 0
	begin
		set @SMAm = @SMAm + @V1/@N - isnull((select V1 from @Data where ID = @ID - cast(@N as int)) / @N, 0)
		select @WS = @PreviousMA + (1/@N) *(@V1 - @PreviousMA)
		insert into @Ret(ID, V1, Info)
			values(@ID, @WS, @Info)
		select @PreviousMA = @SMAm 
		fetch next from c into @ID, @V1, @Info
	end
	close c
	deallocate c
	return
end
go
declare @Data f.Array1
insert into @Data(V1) values(1),(2),(3),(4),(5),(6),(7)
--select * from @Data
select * from f.WilderSmoothing(@Data, 2)