CREATE TABLE [q].[SymbolChange] (
    [Date]         DATE NOT NULL,
    [FromSymbolID] INT  NOT NULL,
    [ToSymbolID]   INT  NOT NULL,
    CONSTRAINT [PK_q_SymbolChange] PRIMARY KEY CLUSTERED ([Date] ASC, [FromSymbolID] ASC, [ToSymbolID] ASC)
);

