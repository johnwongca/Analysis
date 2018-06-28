create function EODData.SplitString(@Text varchar(max), @Separator varchar(1))
returns table 
as return
(
	with x0 as
	(
		select 1 as one 
	)
	select row_number() over(order by one) as RowID, value as Value
	from string_split(@Text, @Separator)
		cross apply x0

)