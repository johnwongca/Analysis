CREATE function [EODData].[FileLocation]()
returns varchar(max)
as
begin
	--return ('\\jvs01\X\EOD\')--( select Value from EODData.Setting where Name = 'FileLocation')
	--return ('E:\Temp\')--( select Value from EODData.Setting where Name = 'FileLocation')
	return ('\\jvs01\EOD\Data\')
end