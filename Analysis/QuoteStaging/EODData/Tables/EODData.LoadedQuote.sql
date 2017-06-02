CREATE TABLE [EODData].[LoadedQuote] (
    [Exchange]         NVARCHAR (10)   NOT NULL,
    [Date]             DATE            NOT NULL,
    [FullFileName]     NVARCHAR (4000) NULL,
    [FileName]         NVARCHAR (MAX)  NULL,
    [CreationDate]     DATETIME        NULL,
    [LastModifiedDate] DATETIME        NULL,
    [Length]           BIGINT          NULL,
    CONSTRAINT [PK_EODData_LoadedQuote] PRIMARY KEY CLUSTERED ([Exchange] ASC, [Date] ASC)
);

