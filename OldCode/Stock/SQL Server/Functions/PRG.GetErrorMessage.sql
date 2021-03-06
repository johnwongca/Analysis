ALTER function [PRG].[GetErrorMessage]()
returns varchar(max)
as 
begin
	return 	'Number: ' + isnull(rtrim(cast(error_number () as varchar(max))), 'NULL') + char(13)+char(10)+
			'Line: ' + isnull(rtrim(cast(error_line() as varchar(max))), 'NULL') + char(13)+char(10)+
			'Procedure: ' + isnull(rtrim(cast(error_procedure () as varchar(max))), 'NULL') + char(13)+char(10)+
			'Severity: ' + isnull(rtrim(cast(error_severity () as varchar(max))), 'NULL') + char(13)+char(10)+
			'State: ' + isnull(rtrim(cast(error_state() as varchar(max))), 'NULL') + char(13)+char(10)+
			'Message: ' + isnull(rtrim(cast(error_message() as varchar(max))), 'NULL') + char(13)+char(10)
end