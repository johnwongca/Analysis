alter procedure DIM.Fact_Average_Delete
(
	@SymbolID int,
	@Seq int
)
as
begin
	delete from DIM.Fact_Average where SymbolID = @SymbolID and Seq = @Seq
end