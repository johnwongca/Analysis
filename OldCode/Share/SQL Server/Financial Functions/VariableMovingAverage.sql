use Stock
go
if OBJECT_ID('f.VariableMovingAverage') is not null
	drop function f.VariableMovingAverage;
go
/*ID column of @Data must be sequential number*/
create function f.VariableMovingAverage (@Data f.Array1 readonly, @N float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	Info xml)
begin
	if @N<=1
	begin
		insert into @Ret(ID, V1, Info)
			select ID, 0, Info
			from @Data
		return
	end
	declare @ID int, @V1 float, @Info xml
	declare @Su float = 0, @Sd float = 0, @Vc float = 0, @Vp float, @Vpn float, @VMA float = 0, @A float
	declare @Change table (ID int NOT NULL primary key, V1 float NOT NULL)
	select top 1 @Vp = V1 from @Data order By ID
	
	declare c cursor static local for
		select ID, V1, Info from @Data order by ID asc
	open c
	fetch next from c into @ID, @V1, @Info
	while @@fetch_status = 0
	begin
		select	@Vc = @V1 - @Vp
		insert into @Change(ID, V1) values(@ID, @Vc)
		select	@Vp = @V1,
				@Vpn = isnull((select V1 from @Change where ID = @ID - cast(@N as int)),0)
		select	@Su = @Su + case when @Vc >=0 then @Vc else 0.00 end - case when @Vpn >=0 then @Vpn else 0.0 end,
				@Sd = @Sd + case when @Vc <=0 then -@Vc else 0.00 end - case when @Vpn <=0 then -@Vpn else 0.0 end
		if @Su + @Sd = 0
			select @A = 0
		else
			select @A = abs(((@Su - @Sd) * 2/ (@Su + @Sd)) / (@N - 1))
		select @VMA = @A * @V1 + (1.0 - @A) * @VMA
		insert into @Ret(ID, V1, Info)
			values(@ID, @VMA, @Info)
		fetch next from c into @ID, @V1, @Info
	end
	close c
	deallocate c
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
select * from f.VariableMovingAverage(@Data, 5) a inner join @Data b on a.ID = b.ID