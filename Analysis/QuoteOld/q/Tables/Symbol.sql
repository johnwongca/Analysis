CREATE TABLE [q].[Symbol] (
    [SymbolID]   INT           IDENTITY (1, 1) NOT NULL,
    [ExchangeID] INT           NOT NULL,
    [Symbol]     VARCHAR (32)  NOT NULL,
    [SymbolName] VARCHAR (128) NOT NULL,
    [LongName]   VARCHAR (128) NULL,
    CONSTRAINT [PK_Symbol] PRIMARY KEY NONCLUSTERED ([SymbolID] ASC) WITH (DATA_COMPRESSION = PAGE) ON [QuoteData],
    CONSTRAINT [FK_Symbol_Exchange] FOREIGN KEY ([ExchangeID]) REFERENCES [q].[Exchange] ([ExchangeID])
);


GO
CREATE CLUSTERED INDEX [IX_Symbol]
    ON [q].[Symbol]([ExchangeID] ASC, [Symbol] ASC) WITH (DATA_COMPRESSION = PAGE)
    ON [QuoteData];


GO
CREATE NONCLUSTERED INDEX [IX_Symbol_SymbolExchange]
    ON [q].[Symbol]([Symbol] ASC, [ExchangeID] ASC) WITH (DATA_COMPRESSION = PAGE)
    ON [QuoteData];

