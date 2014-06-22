CREATE TABLE [EODData].[TaskCompleted]
(
	
	[TaskID] INT NOT NULL,
	TaskCompletionID int not null identity,
	PoolID smallint not null,
	MethodName varchar(50) not null,
	[IntervalID] TINYINT NULL, 
    [Exchange] VARCHAR(10) NULL, 
    [Symbol] VARCHAR(32) NULL, 
    [DateFrom] DATE NULL, 
    [DateTo] DATE NULL, 
    [EnlistDate] DATETIME NOT NULL, 
    [Retries] INT not NULL, 
	CompletedDate datetime not null constraint DF_EDOData_TaskCompleted_CompeteDate default(getdate()),
	Error varchar(max),
    constraint PK_EODData_TaskCompleted PRIMARY KEY(TaskID, TaskCompletionID)
)
