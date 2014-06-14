
CREATE TABLE [EODData].[SessionSymbol]
(
	[Exchange] [varchar](10) not null,
	[Symbol] [varchar](32) not null,
	[Name] varchar(128) NULL,
	[LongName] varchar(128) NULL,
	[Date] DATE NULL, 
    CONSTRAINT [PK_EODData_SessionSymbol] PRIMARY KEY(Exchange, Symbol)
) 
