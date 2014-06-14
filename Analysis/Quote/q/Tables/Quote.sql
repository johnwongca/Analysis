CREATE TABLE [q].[Quote] (
    [SymbolID] INT    NOT NULL,
    [Date]     DATE   NOT NULL,
    [Open]     REAL   NOT NULL,
    [High]     REAL   NOT NULL,
    [Low]      REAL   NOT NULL,
    [Close]    REAL   NOT NULL,
    [Volume]   BIGINT NOT NULL,
    CONSTRAINT [PK_q_Quote] PRIMARY KEY CLUSTERED ([SymbolID] ASC, [Date] ASC) WITH (DATA_COMPRESSION = PAGE)
);

