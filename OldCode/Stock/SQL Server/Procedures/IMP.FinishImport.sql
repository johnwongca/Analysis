alter procedure IMP.FinishImport
(
	@SourceFileID int
)
as
begin
	update SourceFile set Imported = 1, Error = null, ImportDate = getdate() where ID = @SourceFileID;
end