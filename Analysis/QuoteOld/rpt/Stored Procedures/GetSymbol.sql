create procedure [rpt].[GetSymbol]
(
	@Exchange varchar(10) = null,
	@ExchangeID int = null
)
as
begin
	select @ExchangeID = isnull( @ExchangeID, (select ExchangeID from q.Exchange where Exchange = @Exchange))
	if @ExchangeID is null
	begin
		raiserror(N'Exchange does not exist', 16, 1)
		return
	end
	select 
			s.SymbolID, s.Symbol, s.SymbolName, s.LongName
	from q.Symbol s 
	where s.ExchangeID = @ExchangeID
	order by s.Symbol
end
