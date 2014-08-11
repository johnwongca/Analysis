CREATE TABLE [q].[Split] (
    [SymbolID] INT          NOT NULL,
    [Date]     DATE         NOT NULL,
    [Ratio]    VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_Split] PRIMARY KEY CLUSTERED ([SymbolID] ASC, [Date] ASC) WITH (DATA_COMPRESSION = PAGE) ON [QuoteData],
    CONSTRAINT [FK_Split_Symbol] FOREIGN KEY ([SymbolID]) REFERENCES [q].[Symbol] ([SymbolID])
);

