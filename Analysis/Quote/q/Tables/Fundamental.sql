CREATE TABLE [q].[Fundamental] (
    [SymbolID]      INT           NOT NULL,
    [DateFrom]      DATE          NOT NULL,
    [DateTo]        DATE          NOT NULL,
    [Sector]        VARCHAR (128) NULL,
    [Industry]      VARCHAR (128) NULL,
    [Dividend]      FLOAT (53)    NOT NULL,
    [DividendDate]  DATETIME      NULL,
    [DividendYield] FLOAT (53)    NOT NULL,
    [DSP]           FLOAT (53)    NOT NULL,
    [EBITDA]        FLOAT (53)    NOT NULL,
    [MarketCap]     BIGINT        NULL,
    [EPS]           FLOAT (53)    NOT NULL,
    [PtS]           FLOAT (53)    NULL,
    [NTA]           FLOAT (53)    NULL,
    [PE]            FLOAT (53)    NULL,
    [PEG]           FLOAT (53)    NULL,
    [PtB]           FLOAT (53)    NULL,
    [Shares]        BIGINT        NULL,
    [Yield]         FLOAT (53)    NULL,
    CONSTRAINT [PK_q_Fundamental] PRIMARY KEY CLUSTERED ([SymbolID] ASC, [DateFrom] ASC)
);

