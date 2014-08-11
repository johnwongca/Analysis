CREATE TABLE [q].[Exchange] (
    [ExchangeID]   INT           IDENTITY (1, 1) NOT NULL,
    [Exchange]     VARCHAR (10)  NOT NULL,
    [ExchangeName] VARCHAR (128) NOT NULL,
    [CountryID]    VARCHAR (10)  NULL,
    [Currency]     VARCHAR (10)  NULL,
    [TimeZone]     VARCHAR (128) NULL,
    CONSTRAINT [PK_Exchange] PRIMARY KEY CLUSTERED ([ExchangeID] ASC) WITH (DATA_COMPRESSION = PAGE) ON [QuoteData]
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Exchange]
    ON [q].[Exchange]([Exchange] ASC, [ExchangeID] ASC) WITH (DATA_COMPRESSION = PAGE)
    ON [QuoteData];

