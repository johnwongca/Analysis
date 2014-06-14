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