CREATE procedure q.RetrieveQuote
(
	@SymbolID int
)
as
begin
	set nocount on
	select Date, [Open], [High], [Low], [Close], [Volume]
	from q.Quote 
	where SymbolID = @SymbolID
	order by Date
end 
