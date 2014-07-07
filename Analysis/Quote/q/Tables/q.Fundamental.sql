CREATE TABLE [q].[Fundamental] (
    [SymbolID]      INT           NOT NULL,
    [EffectiveDate]	DATE          NOT NULL,
	ExpiryDate		Date		null,
    [Sector]        int			NULL,
    [Industry]      int				NULL,
    [Dividend]      FLOAT (53)    NULL,
    [DividendDate]  DATETIME      NULL,
    [DividendYield] FLOAT (53)    NULL,
    [DPS]           FLOAT (53)    NULL,
    [EBITDA]        FLOAT (53)    NULL,
    [MarketCap]     BIGINT        NULL,
    [EPS]           FLOAT (53)    NULL,
    [PtS]           FLOAT (53)    NULL,
    [NTA]           FLOAT (53)    NULL,
    [PE]            FLOAT (53)    NULL,
    [PEG]           FLOAT (53)    NULL,
    [PtB]           FLOAT (53)    NULL,
    [Shares]        BIGINT        NULL,
    [Yield]         FLOAT (53)    NULL,
    CONSTRAINT [PK_q_Fundamental] PRIMARY KEY CLUSTERED ([SymbolID] ASC, [EffectiveDate] ASC) WITH (DATA_COMPRESSION = PAGE)
);

