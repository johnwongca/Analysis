create function PRG.Date
(
	@Date datetime
) returns datetime
as
begin
	return cast(floor(cast(@Date as float)) as datetime)
end
go
