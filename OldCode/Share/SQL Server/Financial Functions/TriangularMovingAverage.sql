use Stock
go
if OBJECT_ID('f.TriangularMovingAverage') is not null
	drop function f.TriangularMovingAverage;
go
create function f.TriangularMovingAverage (@Data f.Array1 readonly, @N float, @K float = 2)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	Info xml)
begin
	if @K <= 1
		select @K = 2;
	declare @A float
	select @A = Round(@N/ @K, 0)
	Declare @Data1 f.Array1
	insert into @Data1	(V1, Info)
		select V1, Info
		from f.SimpleMovingAverage(@Data, @A)
		order by ID
	insert into @Ret(ID, V1, Info)
		select ID, V1, Info
		from f.SimpleMovingAverage(@Data, @N - @A)
		order by ID
	return
end
go
declare @Data f.Array1
insert into @Data(V1) values(1),(2),(3),(4),(5),(6),(7)
--select * from @Data
select * from f.TriangularMovingAverage(@Data, 3.01, default)