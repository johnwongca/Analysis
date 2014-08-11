CREATE procedure [q].[ImportStaging_Quote]
(
	@FromVersion binary(8) = 0x00, 
	@ToVersion binary(8) = 0xFF0000001001372E
)
as
begin
	declare @SQL nvarchar(max)
	
	select @SQL = (
					select  '
	begin transaction
	merge q.Quote t
	using (
			select s.SymbolID, cast(a.Date as Date) as Date, a.[Open] , a.[Close], a.High, a.Low, a.Volume
			from EODData.'+TableName+' a
				inner loop join q.Symbol s on s.ExchangeID = ' + CAST(q.GetExchangeID(ExchangeID) as varchar(20)) + ' and s.Symbol = a.SymbolID
			--where a.___DataVersion___ > @FromVersion
			--	and a.___DataVersion___ <= @ToVersion
			) s on t.SymbolID = s.SymbolID and t.Date = s.Date
	when matched and (s.[Open] <> t.[Open] or s.[High] <> t.[High] or s.Low <> t.Low or s.[Close]<>t.[Close] or s.Volume <> t.Volume) then 
		update 
			set	t.[Open] = s.[Open], 
				t.[High] = s.[High], 
				t.Low = s.Low, 
				t.[Close] = s.[Close], 
				t.Volume = s.Volume
	when not matched then
		insert (SymbolID, Date, [Open], [High], Low, [Close], Volume)
			values(s.SymbolID, s.Date, s.[Open], s.[High], s.Low, s.[Close], s.Volume)	
	;
	--delete EODData.'+TableName+' where ___DataVersion___ <= @ToVersion
	truncate table EODData.'+TableName+' 
	commit transaction
	' 
					from EODData.staging.AvailableQuoteTable	
					where IntervalName = 'Day'
					for xml path(''), type
				).value('.', 'nvarchar(max)')
	
	exec sp_executesql @SQL, N'@FromVersion binary(8), @ToVersion binary(8)', @FromVersion, @ToVersion
	
	
	select @SQL = (
					select '
	begin transaction
	merge q.QuoteDetail t
	using (
			select q.QuoteID, cast(a.Date as time(0)) as Time, a.[Open] , a.[Close], a.High, a.Low, a.Volume
			from EODData.'+TableName+' a
				inner loop join q.Symbol s on s.ExchangeID = ' + CAST(q.GetExchangeID(ExchangeID) as varchar(20)) + ' and s.Symbol = a.SymbolID
				inner loop join q.Quote q on q.SymbolID = s.SymbolID and q.Date = cast(a.Date as Date) 
			--where a.___DataVersion___ > @FromVersion
			--	and a.___DataVersion___ <= @ToVersion
			) s on t.Time = s.Time and t.QuoteID = s.QuoteID and t.Type = cast('+CAST([Minute] as varchar(10))+' as tinyint)
	when matched and (s.[Open] <> t.[Open] or s.[High] <> t.[High] or s.Low <> t.Low or s.[Close]<>t.[Close] or s.Volume <> t.Volume) then 
		update 
			set	t.[Open] = s.[Open], 
				t.[High] = s.[High], 
				t.Low = s.Low, 
				t.[Close] = s.[Close], 
				t.Volume = s.Volume
	when not matched then
		insert (Time, [Open], [High], Low, [Close], Volume, QuoteID, Type)
			values(s.Time, s.[Open], s.[High], s.Low, s.[Close], s.Volume, s.QuoteID, cast('+CAST([Minute] as varchar(10))+' as tinyint))
	;
	--delete EODData.'+TableName+' where ___DataVersion___ <= @ToVersion
	truncate table EODData.'+TableName+' 
	commit transaction
	' 
					from EODData.staging.AvailableQuoteTable	
					where [Minute] > 0
					for xml path(''), type
				).value('.', 'nvarchar(max)')
	exec sp_executesql @SQL, N'@FromVersion binary(8), @ToVersion binary(8)', @FromVersion, @ToVersion
	
end
