CREATE TABLE [chart].[Chart] (
    [ChartID]          INT           IDENTITY (1, 1) NOT NULL,
    [ParentChartID]    INT           NULL,
    [ChartUserID]      INT           NOT NULL,
    [ChartName]        VARCHAR (128) NOT NULL,
    [SymbolID]         INT           NOT NULL,
    [SourceIntervalID] INT           NOT NULL,
    [DisplayStartID]   BIGINT        NULL,
    [DisplayRange]     INT           CONSTRAINT [DF_Chart_DisplayRange] DEFAULT ((100)) NOT NULL,
    [Algorithm]        XML           NOT NULL,
    [ChartDefinition]  XML           NOT NULL,
    CONSTRAINT [PK_Chart] PRIMARY KEY CLUSTERED ([ChartID] ASC) ON [QuoteData],
    CONSTRAINT [FK_Chart_Chart] FOREIGN KEY ([ParentChartID]) REFERENCES [chart].[Chart] ([ChartID]),
    CONSTRAINT [FK_Chart_ChartUser] FOREIGN KEY ([ChartUserID]) REFERENCES [chart].[ChartUser] ([ChartUserID]),
    CONSTRAINT [FK_Chart_SourceInterval] FOREIGN KEY ([SourceIntervalID]) REFERENCES [chart].[SourceInterval] ([SourceIntervalID]),
    CONSTRAINT [FK_Chart_Symbol] FOREIGN KEY ([SymbolID]) REFERENCES [q].[Symbol] ([SymbolID])
) TEXTIMAGE_ON [QuoteData];


GO
CREATE NONCLUSTERED INDEX [IX_Chart]
    ON [chart].[Chart]([ChartUserID] ASC, [ChartName] ASC)
    ON [QuoteData];

