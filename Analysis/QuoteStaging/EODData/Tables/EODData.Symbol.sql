
CREATE TABLE [EODData].[Symbol]
(
	[Exchange] [varchar](10) not null,
	[Symbol] [varchar](32) not null,
	[Name] varchar(128) NULL,
	[LongName] varchar(128) NULL,
	[Date] DATE NULL, 
    CONSTRAINT [PK_EODData_Symbol] PRIMARY KEY(Exchange, Symbol) with( STATISTICS_NORECOMPUTE = on)
) 
