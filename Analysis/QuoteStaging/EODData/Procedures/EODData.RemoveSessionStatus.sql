CREATE PROCEDURE [EODData].[RemoveSessionStatus]
as
begin
	set nocount on 
	delete EODData.SessionStatus;
end