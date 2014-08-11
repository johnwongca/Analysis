CREATE TABLE [q].[LastLoadedVersion] (
    [LastLoadedVersion] BINARY (8) CONSTRAINT [DF_LastLoadedVersion_LastLoadedVersion] DEFAULT (0x00000000) NOT NULL,
    CONSTRAINT [PK_LastLoadedVersion] PRIMARY KEY CLUSTERED ([LastLoadedVersion] ASC) WITH (DATA_COMPRESSION = PAGE) ON [QuoteData]
);

