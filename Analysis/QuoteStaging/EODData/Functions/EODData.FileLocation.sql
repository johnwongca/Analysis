CREATE function [EODData].[FileLocation]()
returns varchar(max)
as
begin
	return ('\\jvs01\X\EOD\')--( select Value from EODData.Setting where Name = 'FileLocation')
end