alter procedure STK.GetSymbol
(
	@ExchangeName nvarchar(30)
)
as
begin
	declare @id int
	if isnumeric(@ExchangeName) = 1
	begin
		select @id = cast(@ExchangeName as int)
		select @ExchangeName = Name
		from STK.Exchange
		where ID = @id;
	end
	if @id is null
	begin
		select @id = ID
		from STK.Exchange
		where Name = @ExchangeName;
	end;
	if @id is null
	begin
		raiserror('Could not find Exchange %s', 16, 1, @ExchangeName);
		return;
	end;
	select ExchangeID, @ExchangeName as ExchangeName, ID, Name, Description
	from STK.Symbol
	where ExchangeID = @ID
	order by Name
end