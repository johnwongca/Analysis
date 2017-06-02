CREATE TABLE [EODData].[FilePath] (
    [FilePathID]    INT           NOT NULL,
    [FilePath]      VARCHAR (MAX) NOT NULL,
    [ProcedureName] VARCHAR (128) NOT NULL,
    [Exception]     VARCHAR (MAX) NULL,
    CONSTRAINT [PK_EODData_FilePath] PRIMARY KEY CLUSTERED ([FilePathID] ASC)
);

