CREATE procedure chart.GetIntervalList
as
begin
	select * from chart.SourceInterval order by SourceIntervalID
end