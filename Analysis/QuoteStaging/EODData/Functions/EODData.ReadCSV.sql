create function [EODData].[ReadCSV](@FileName varchar(1000), @Separator varchar(1), @SkipRows int)
returns table
as
return (
			select a.RowID, [1] C1, [2] C2, [3] C3, [4] C4, [5] C5, [6] C6, [7] C7, [8] C8, [9] C9, [10] C10, [11] C11, [12] C12
			from EODData.BreakLine(cast(clr.GetFileContent(@FileName) as varchar(max))) a
				cross apply (
									select  [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12]
									from EODData.SplitString(a.Line, @Separator) p
									pivot
									(
										max(Value)
										for RowID in([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
									)pvt
							)b
			Where a.RowID > @SkipRows
	)