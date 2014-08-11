CREATE TABLE [chart].[Cache] (
    [CursorName] VARCHAR (128) NOT NULL,
    [Algorithm]  XML           NOT NULL,
    [Date]       DATETIME      CONSTRAINT [DF_Cache_Date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Cache] PRIMARY KEY CLUSTERED ([CursorName] ASC)
);

