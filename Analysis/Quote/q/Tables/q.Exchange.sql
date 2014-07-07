CREATE TABLE [q].[Exchange] (
    [Exchange]     VARCHAR (10)  NOT NULL,
    [ExchangeName] VARCHAR (128) NOT NULL,
    [Country]      VARCHAR (10)  NULL,
    [Currency]     VARCHAR (10)  NULL,
    [TimeZone]     VARCHAR (128) NULL,
    CONSTRAINT [PK_q_Exchange] PRIMARY KEY CLUSTERED ([Exchange] ASC)
);

