CREATE TABLE [EODData].[TaskCompleted]
(
	PoolID smallint not null,
	[TaskID] INT NOT NULL ,
	MethodName varchar(50) not null,
	[IntervalID] TINYINT NULL, 
    [Exchange] VARCHAR(10) NULL, 
    [Symbol] VARCHAR(32) NULL, 
    [DateFrom] DATE NULL, 
    [DateTo] DATE NULL, 
    [EnlistDate] DATETIME NOT NULL, 
    [StartDate] DATETIME NULL, 
    [Retries] INT not NULL, 
	CompletedDate datetime not null constraint DF_EDOData_TaskCompleted_CompeteDate default(getdate()),
	Error varchar(max),
    constraint PK_EODData_TaskCompleted PRIMARY KEY(TaskID)
)
