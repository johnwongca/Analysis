create function chart.GetCursorName (@Algorithm xml)
returns varchar(128)
as
begin
	return dbo.GetChartTableNameByParameter(@Algorithm);
end
