CREATE TABLE [q].[SymbolChange] (
    [Date]         DATE NOT NULL,
    [FromSymbolID] INT  NOT NULL,
    [ToSymbolID]   INT  NOT NULL,
    CONSTRAINT [PK_SymbolChange] PRIMARY KEY CLUSTERED ([Date] ASC, [FromSymbolID] ASC, [ToSymbolID] ASC) WITH (DATA_COMPRESSION = PAGE) ON [QuoteData],
    CONSTRAINT [FK_SymbolChange_Symbol] FOREIGN KEY ([FromSymbolID]) REFERENCES [q].[Symbol] ([SymbolID]),
    CONSTRAINT [FK_SymbolChange_Symbol1] FOREIGN KEY ([ToSymbolID]) REFERENCES [q].[Symbol] ([SymbolID])
);


GO
CREATE NONCLUSTERED INDEX [IX_SymbolChange]
    ON [q].[SymbolChange]([FromSymbolID] ASC) WITH (DATA_COMPRESSION = PAGE)
    ON [QuoteData];


GO
CREATE NONCLUSTERED INDEX [IX_SymbolChange_1]
    ON [q].[SymbolChange]([ToSymbolID] ASC) WITH (DATA_COMPRESSION = PAGE)
    ON [QuoteData];

