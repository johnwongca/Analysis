CREATE TABLE [EODData].[Task] (
    [TaskID]             INT           IDENTITY (1, 1) NOT NULL,
    [PoolID]             SMALLINT      NOT NULL,
    [MethodName]         VARCHAR (50)  NOT NULL,
    [IntervalID]         TINYINT       NULL,
    [Exchange]           VARCHAR (10)  NULL,
    [Symbol]             VARCHAR (32)  NULL,
    [DateFrom]           DATE          NULL,
    [DateTo]             DATE          NULL,
    [EnlistDate]         DATETIME      CONSTRAINT [DF_EODData_Task_EnlistDate] DEFAULT (getdate()) NOT NULL,
    [LastCompletionDate] DATETIME      CONSTRAINT [DF_EODData_Task_LastCompletionDate] DEFAULT (getdate()) NOT NULL,
    [Retries]            INT           CONSTRAINT [DF_EODData_Task_Retries] DEFAULT ((0)) NOT NULL,
    [NextStartDate]      AS            (dateadd(second,power((2),case when [Retries]<(12) then [Retries] else (11) end)-(1),[LastCompletionDate])) PERSISTED,
    [IsRegularPool]      AS            (case when [PoolID]=(0) then (0) else (1) end) PERSISTED NOT NULL,
    [Status]             VARCHAR (10)  CONSTRAINT [DF_EODData_Task_Status] DEFAULT ('Pending') NOT NULL,
    [PostScript]         VARCHAR (MAX) NULL,
    [Error]              VARCHAR (MAX) NULL,
    CONSTRAINT [PK_EODData_Task] PRIMARY KEY CLUSTERED ([TaskID] ASC),
    CONSTRAINT [FK_EODData_Task_EODData_Interval] FOREIGN KEY ([IntervalID]) REFERENCES [EODData].[Interval] ([IntervalID])
);


