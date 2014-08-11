CREATE TABLE [chart].[ChartUser] (
    [ChartUserID] INT           IDENTITY (1, 1) NOT NULL,
    [LoginName]   VARCHAR (128) CONSTRAINT [DF_ChartUser_LoginName] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [PK_ChartUser] PRIMARY KEY CLUSTERED ([ChartUserID] ASC) ON [QuoteData]
);


GO
CREATE NONCLUSTERED INDEX [IX_ChartUser]
    ON [chart].[ChartUser]([ChartUserID] ASC, [LoginName] ASC)
    ON [QuoteData];

