CREATE TABLE [q].[WatchList] (
    [SymbolID]     INT      NOT NULL,
    [CreationDate] DATETIME CONSTRAINT [DF_WatchList_CreationDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_WatchList] PRIMARY KEY CLUSTERED ([SymbolID] ASC) WITH (IGNORE_DUP_KEY = ON)
);

