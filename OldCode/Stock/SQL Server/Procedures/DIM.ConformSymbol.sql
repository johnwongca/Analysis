create procedure DIM.ConformSymbol
as
begin
	insert into DIM.A_Exchange(ID, Name, Description, SortOrder)
	select a.ID, rtrim(ltrim(a.Name)), a.Description, a.ID
	from A.Exchange a
	where not exists(select 1 from DIM.A_Exchange b where a.ID = b.ID)
	update b
		set b.Name = rtrim(ltrim(a.Name)),
			b.Description = a.Description
	from A.Exchange a
		inner join DIM.A_Exchange b on a.ID = b.ID

	update b
		set b.Name = rtrim(ltrim(a.Name)),
			b.Description = a.Description
	from A.Exchange a
		inner join DIM.A_Exchange b on a.ID = b.ID

	update a
		set a.SortOrder = b.SortOrder
	from DIM.A_Exchange a
		inner join (select ID, Row_Number() over(order by Name) as SortOrder from  DIM.A_Exchange)b on a.ID = b.ID
	
	delete a from DIM.A_Exchange a where not exists(select 1 from A.Exchange b where a.ID = b.ID)


	insert into DIM.Symbol(ID, Name, Description, SortOrder, ExchangeID)
		select ID, Name, Description, ID, ExchangeID 
		from A.Symbol a
		where not exists(select 1 from DIM.Symbol b where a.ID = b.ID)
	update a
		set a.Name = b.Name,
			a.Description = b.Description,
			a.ExchangeID = b.ExchangeID,
			a.SortOrder = c.SortOrder
	from DIM.Symbol a
		inner join A.Symbol b on a.ID = b.ID
		inner join (select ID, Row_Number() over(order by Name) as SortOrder from  A.Symbol) c on a.ID = c.ID
	delete a from DIM.Symbol a where not exists(select 1 from A.Symbol b where a.ID = b.ID)
end