CREATE TABLE [EODData].[Fundamental] (
    [Exchange]      VARCHAR (10)    NOT NULL,
    [Symbol]        VARCHAR (32)    NOT NULL,
    [Date]          DATE            CONSTRAINT [DF_Fundamental_Date] DEFAULT (getdate()) NULL,
    [Name]          VARCHAR (128)   NULL,
    [Description]   NVARCHAR (1000) NULL,
    [Dividend]      FLOAT (53)      NULL,
    [DividendDate]  DATETIME        NULL,
    [DividendYield] FLOAT (53)      NULL,
    [DPS]           FLOAT (53)      NULL,
    [EBITDA]        FLOAT (53)      NULL,
    [EPS]           FLOAT (53)      NULL,
    [Industry]      VARCHAR (1000)  NULL,
    [MarketCap]     BIGINT          NULL,
    [NTA]           FLOAT (53)      NULL,
    [PE]            FLOAT (53)      NULL,
    [PEG]           FLOAT (53)      NULL,
    [PtB]           FLOAT (53)      NULL,
    [PtS]           FLOAT (53)      NULL,
    [Sector]        VARCHAR (128)   NULL,
    [Shares]        BIGINT          NULL,
    [Yield]         FLOAT (53)      NULL
);

 
GO
CREATE UNIQUE CLUSTERED INDEX [IX_Fundamental]
    ON [EODData].[Fundamental]([Date] ASC, [Exchange] ASC, [Symbol] ASC);

