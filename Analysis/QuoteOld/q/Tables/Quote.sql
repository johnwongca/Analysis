CREATE TABLE [q].[Quote] (
    [QuoteID]  BIGINT     IDENTITY (1, 1) NOT NULL,
    [SymbolID] INT        NOT NULL,
    [Date]     DATE       NOT NULL,
    [Open]     FLOAT (53) NOT NULL,
    [High]     FLOAT (53) NOT NULL,
    [Low]      FLOAT (53) NOT NULL,
    [Close]    FLOAT (53) NOT NULL,
    [Volume]   BIGINT     NOT NULL,
    CONSTRAINT [PK_Quote] PRIMARY KEY NONCLUSTERED ([QuoteID] ASC) WITH (DATA_COMPRESSION = PAGE) ON [QuoteData],
    CONSTRAINT [FK_Quote_Symbol] FOREIGN KEY ([SymbolID]) REFERENCES [q].[Symbol] ([SymbolID])
);


GO
CREATE UNIQUE CLUSTERED INDEX [IX_Quote]
    ON [q].[Quote]([SymbolID] ASC, [Date] ASC) WITH (DATA_COMPRESSION = PAGE)
    ON [QuoteData];

