CREATE PROCEDURE EODData.ReadSettings
AS
begin
	set nocount on
	select Name, Value from EODData.Setting
end
