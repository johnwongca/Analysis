alter procedure DIM.Ex
(
	@SQL varchar(max)
)
as
begin
	execute dbo.sp_CLRRemoteExecute	@ID = null,
									@ConnectionType = N'CurrentContext',
									@ConnectionString = 'Context Connection = True',
									@CommandTimeout = 0,
									@SQL = @SQL,
									@CommandError = N'Raise',
									@Transaction_Type = N'CurrentContext',
									@Transaction_Success = N'NoAction',
									@Transaction_Failure = N'NoAction',

									@InfoMessage_IncludeDetail = N'false',
									@InfoMessage_ReturnType  = N'Console',
									@InfoMessage_Connection_ConnectionType = N'NoConnection',
									@InfoMessage_Connection_ConnectionString = null,
									@InfoMessage_Connection_CommandTimeout = 0,
									@InfoMessage_OnMessage = null,
									@InfoMessage_OnEventError = 'StopProcess',

									@ErrorMessage_ReturnType = 'Console',
									@ErrorMessage_Connection_ConnectionType = 'NoConnection',
									@ErrorMessage_Connection_ConnectionString = null,
									@ErrorMessage_Connection_CommandTimeout = 86400,
									@ErrorMessage_OnMessage = null,
									@ErrorMessage_OnEventError = 'StopProcess',

									@ResultSets_ReturnType = 'Console',
									@ResultSets_Connection_ConnectionType = 'NoConnection',
									@ResultSets_Connection_ConnectionString = null,
									@ResultSets_Connection_CommandTimeout = 86400,
									@ResultSets_OnMessage = null,
									@ResultSets_OnEventError = 'StopProcess'
end