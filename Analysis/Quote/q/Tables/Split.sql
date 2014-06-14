CREATE TABLE [q].[Split] (
    [SymbolID] INT          NOT NULL,
    [Date]     DATE         NOT NULL,
    [Ratio]    VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_q_Split] PRIMARY KEY CLUSTERED ([SymbolID] ASC, [Date] ASC)
);

