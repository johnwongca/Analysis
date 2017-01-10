alter procedure IMP.ImportFile
as
begin
	delete from IMP.SourceFile where Body is null
	delete from IMP.SourceFile where FileSize <> len(isnull(body,''))
	declare @LocationID int
	declare c cursor local static for
		select l.ID
		from Location l
			inner join Provider p on p.ID = l.ProviderID
			inner join Address a on a.ID = l.AddressID
		where	l.Active = 1
			and p.Active = 1
			and a.Active = 1
	open c
	fetch next from c into @LocationID
	while @@fetch_status = 0
	begin
		exec IMP.ReadFile @LocationID
		fetch next from c into @LocationID
	end
	close c
	deallocate c
	delete from IMP.SourceFile where FileSize <> len(isnull(body,''))
end