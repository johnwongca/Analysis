CREATE TABLE [chart].[SourceInterval] (
    [SourceIntervalID]   INT          NOT NULL,
    [SourceIntervalName] VARCHAR (30) NOT NULL,
    CONSTRAINT [PK_SourceInterval] PRIMARY KEY CLUSTERED ([SourceIntervalID] ASC) ON [QuoteData]
);

