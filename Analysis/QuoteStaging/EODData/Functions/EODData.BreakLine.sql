CREATE function EODData.BreakLine(@Text varchar(max))
returns table
as
return
(
	select RowID, replace(value, char(0x0d), '') Line
	from EODData.SplitString(@Text, char(0x0a))
)