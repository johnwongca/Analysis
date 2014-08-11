create function q.DateTimeToFloat(@Date date, @Time Time)
returns float
as
begin
	return cast(datediff(day, '1900-01-01', @Date) as float) + cast(datediff(minute, '00:00:00', @Time) as float) / 1440
end
