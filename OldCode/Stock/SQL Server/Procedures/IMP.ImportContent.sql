alter procedure IMP.ImportContent
as
begin
	declare @SourceFileID int, @proc nvarchar(256)
	declare c cursor local static for
		select f.ProcedureName, s.ID
		from Location l
			inner join Provider p on p.ID = l.ProviderID
			inner join Address a on a.ID = l.AddressID
			inner join SourceFile s on s.LocationID = l.ID
			inner join Format f on f.ID = l.FormatID
		where	l.Active = 1
			and p.Active = 1
			and a.Active = 1
			and f.ProcedureName is not null
			and s.Imported = 0
		order by l.[Order] asc
	open c
	fetch next from c into @proc, @SourceFileID
	while @@fetch_status = 0
	begin
		exec @proc @SourceFileID
		fetch next from c into @proc, @SourceFileID
	end
	close c
	deallocate c
end