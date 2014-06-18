/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

;merge EODData.Setting t
using (
		select * 
		from (
				values('MaxConcurrentExecution','5'),
				('Token', '016E65GDUEHH'),
				('WebService','http://ws.eoddata.com/data.asmx')
			) c(Name, Value)
	) as s on t.Name = s.Name
When not matched then
	insert(Name, Value) values(s.Name, s.Value);


;merge EODData.Interval as t
using (
		select * from (values	
		(0, N'OneMinute', 1),
		(1, N'FiveMinute', 5),
		(2, N'TenMinute', 10),
		(3, N'FifteenMinute', 15),
		(4, N'ThirtyMinute', 30),
		(5, N'OneHour', 60),
		(6, N'Day', NULL),
		(7, N'Week', NULL),
		(8, N'Month', NULL),
		(9, N'Top10Gain', NULL),
		(10, N'Top10Loss', NULL)
		) c([IntervalID], [IntervalName], [Minute])
		) as s on t.IntervalID = s.IntervalID
when not matched then
	insert ([IntervalID], [IntervalName], [Minute]) values(s.[IntervalID], s.[IntervalName], s.[Minute]);