CREATE TABLE [EODData].[Task]
(
	TaskID INT NOT NULL identity(1,1),
	PoolID smallint not null,
	MethodName varchar(50) not null,
	[IntervalID] TINYINT NULL, 
    [Exchange] VARCHAR(10) NULL, 
    [Symbol] VARCHAR(32) NULL, 
    [DateFrom] DATE NULL, 
    [DateTo] DATE NULL, 
    [EnlistDate] DATETIME NOT NULL constraint DF_EODData_Task_EnlistDate default (GetDate()), 
    [Retries] INT not NULL constraint DF_EODData_Task_Retries DEFAULT (0), 
	StartDate as dateadd(second, power(2, Retries) -1, EnlistDate) persisted,
	IsRegularPool as case when PoolID = 0 then 0 else 1 end persisted, 
	Error varchar(max),
    constraint PK_EODData_Task PRIMARY KEY(TaskID),
	constraint FK_EODData_Task_EODData_Interval foreign key (IntervalID) references EODData.Interval(IntervalID),
	index IX_EODData_Task_StartDate(IsRegularPool, StartDate)
)
