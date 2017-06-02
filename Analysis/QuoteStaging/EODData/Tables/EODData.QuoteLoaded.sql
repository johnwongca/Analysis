CREATE TABLE [EODData].[QuoteLoaded] (
    [Exchange]         NVARCHAR (MAX)  NOT NULL,
    [Date]             DATE            NOT NULL,
    [FullFileName]     NVARCHAR (4000) NULL,
    [FileName]         NVARCHAR (MAX)  NULL,
    [CreationDate]     DATETIME        NULL,
    [LastModifiedDate] DATETIME        NULL,
    [Length]           BIGINT          NULL
);

