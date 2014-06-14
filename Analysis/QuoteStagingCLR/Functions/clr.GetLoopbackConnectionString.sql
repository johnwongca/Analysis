CREATE FUNCTION clr.GetLoopbackConnectionString()
RETURNS varchar(max)
AS
BEGIN
	return 'Server='+@@ServerName+';Database='+db_name()+';trusted_connection=true'
END
