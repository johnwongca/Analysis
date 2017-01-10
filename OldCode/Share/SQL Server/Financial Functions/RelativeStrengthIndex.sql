use Stock
go
if OBJECT_ID('f.RelativeStrengthIndex') is not null
	drop function f.RelativeStrengthIndex;
go
/*ID column of @Data must be sequential number*/
create function f.RelativeStrengthIndex (@Data f.Array1 readonly, @N float)
returns	@Ret table (ID int NOT NULL primary key, V1 float NOT NULL,	Info xml)
begin
	declare @ID int, @V1 float, @Info xml
	declare @Su float = 0, @Sd float = 0, @Vc float = 0, @Vp float, @Vpn float, @RSI float
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
		if @Sd = 0
			select @RSI = 100
		else
			select @RSI = 100.0 - (100.0/(1 + @Su/@Sd))
		
		insert into @Ret(ID, V1, Info)
			values(@ID, @RSI, @Info)
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
select * from f.RelativeStrengthIndex(@Data, 14) a inner join @Data b on a.ID = b.ID