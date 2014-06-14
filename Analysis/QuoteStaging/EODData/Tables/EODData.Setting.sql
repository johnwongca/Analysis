CREATE TABLE [EODData].[Setting]
(
	[Name] VARCHAR(128) NOT NULL , 
    [Value] VARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_EODData_Setting] PRIMARY KEY (Name)
)
