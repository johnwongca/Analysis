CREATE procedure [chart].[GetChartCursor] @SymbolID int, @Interval int, @algorithm xml, @CursorName varchar(128) output, @ForceCreation bit = 0
as
begin
	declare @SQL varchar(max) = 'exec chart.GetSourcedata @SymbolID, @Interval'
	declare @Source xml, @Parameter xml
	select @Source = (
						select @SQL [@SQL] ,
							(
								select *
								from (values
									('@SymbolID', 'int', @SymbolID),
									('@Interval', 'int', @Interval)) a([@Name], [@Type], [@Value])
								for xml path('Parameter'), type
							) Parameters
						for xml path('Source')
						)
	select @Parameter = (
							select db_name() as [@Database],
									@Source,
									@Algorithm
							for xml path ('Data')
						)
	exec chart.CreateCursor @Parameter, @CursorName output, @ForceCreation
end
