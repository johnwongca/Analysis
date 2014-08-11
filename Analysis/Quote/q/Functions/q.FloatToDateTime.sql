create function q.FloatToDateTime(@Value float)
returns datetime
as
begin
	return dateadd(minute, (@Value - floor(@Value)) * 1440, dateadd(day, floor(@Value), '1900-01-01'))
end