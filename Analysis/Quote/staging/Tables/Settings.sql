CREATE TABLE [staging].[Settings] (
    [Section]          VARCHAR (128) NOT NULL,
    [Name]             VARCHAR (128) NOT NULL,
    [Value]            VARCHAR (128) NOT NULL,
    [LastModifiedDate] DATETIME      CONSTRAINT [DF_Settings_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED ([Section] ASC, [Name] ASC)
);

