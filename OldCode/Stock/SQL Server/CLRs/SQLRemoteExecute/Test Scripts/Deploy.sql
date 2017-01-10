/*
	ConnectionType{CurrentContext, NewConnection, NoConnection}
	CommandError{None, Raise}
	TransactionType{CurrentContext, NewTransaction, NoTransaction}
	TransactionAction{NoAction, CommitTransaction, RollBackTransaction}
	ReturnType{Console, Event, None}
	EventError{IgnoreError, StopProcess}
*/
ALTER PROCEDURE dbo.sp_CLRRemoteExecute
	@ID nvarchar(100) = null,

	@ConnectionType nvarchar(20) = N'NewConnection',
	@ConnectionString nvarchar(4000) = null,
	@CommandTimeout int = 86400,
	@SQL nvarchar(max),
	@CommandError nvarchar(20) = N'Raise',
	@Transaction_Type nvarchar(20) = N'NoTransaction',
	@Transaction_Success nvarchar(20) = N'NoAction',
	@Transaction_Failure nvarchar(20) = N'NoAction',

	@InfoMessage_IncludeDetail nvarchar(20) = N'false',
	@InfoMessage_ReturnType nvarchar(20) = N'Console',
	@InfoMessage_Connection_ConnectionType nvarchar(20) = N'NoConnection',
	@InfoMessage_Connection_ConnectionString nvarchar(4000) = null,
	@InfoMessage_Connection_CommandTimeout int = 86400,
	@InfoMessage_OnMessage nvarchar(4000) = null,
	@InfoMessage_OnEventError nvarchar(20) = 'StopProcess',

	@ErrorMessage_ReturnType nvarchar(20) = 'Console',
	@ErrorMessage_Connection_ConnectionType nvarchar(20) = 'NoConnection',
	@ErrorMessage_Connection_ConnectionString nvarchar(4000) = null,
	@ErrorMessage_Connection_CommandTimeout int = 86400,
	@ErrorMessage_OnMessage nvarchar(4000) = null,
	@ErrorMessage_OnEventError nvarchar(20) = 'StopProcess',

	@ResultSets_ReturnType nvarchar(20) = 'Console',
	@ResultSets_Connection_ConnectionType nvarchar(20) = 'NoConnection',
	@ResultSets_Connection_ConnectionString nvarchar(4000) = null,
	@ResultSets_Connection_CommandTimeout int = 86400,
	@ResultSets_OnMessage nvarchar(4000) = null,
	@ResultSets_OnEventError nvarchar(20) = 'StopProcess'
WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [SQLRemoteExecute].[SqlRemoteExecute.SQLRExecute].[sp_CLRRemoteExecute]
