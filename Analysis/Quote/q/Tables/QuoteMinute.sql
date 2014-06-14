﻿CREATE TABLE [q].[QuoteMinute] (
    [SymbolID] INT      NOT NULL,
    [Date]     DATE     NOT NULL,
    [Time]     TIME (0) NOT NULL,
    [Open]     REAL     NOT NULL,
    [High]     REAL     NOT NULL,
    [Low]      REAL     NOT NULL,
    [Close]    REAL     NOT NULL,
    [Volume]   BIGINT   NOT NULL,
    CONSTRAINT [PK_q_QuoteMinute] PRIMARY KEY CLUSTERED ([SymbolID] ASC, [Date] ASC, [Time] ASC) WITH (DATA_COMPRESSION = PAGE)
);

