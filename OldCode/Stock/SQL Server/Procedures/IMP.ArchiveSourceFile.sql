alter procedure IMP.ArchiveSourceFile
(
	@SourceFileID int
)
as
begin
	
	insert into SourceFileArchive(
									ID, LocationID, FileName, 
									FileDate, FileSize, ImportDate, 
									Body, Hash
								)
		exec sp_executesql 	N'delete from IMP.SourceFile 
								output	deleted.ID, deleted.LocationID, deleted.FileName, 
										deleted.FileDate, deleted.FileSize, deleted.ImportDate, 
										deleted.Body, deleted.Hash
							where ID = @SourceFileID  and Imported = 1
						', N'@SourceFileID int', @SourceFileID = @SourceFileID
end