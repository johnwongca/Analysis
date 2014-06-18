
CREATE TABLE [EODData].[Exchange]
(
	[Exchange] [varchar](10) not null,
	[Name] [varchar](128) NOT NULL,
	[Country] [varchar](5) NULL,
	[Currency] [varchar](10) NULL,
	[Declines] [int] NULL,
	[HasIntradayProduct] [bit] NULL,
	[IntradayStartDate] DATE NULL,
	[IsIntraday] [bit] NULL,
	[LastTradeDateTime] DATE NULL,
	[Suffix] [varchar](10) NULL,
	[TimeZone] varchar(50) NULL, 
    CONSTRAINT [PK_EODData_Exchange] PRIMARY KEY(Exchange) 
) 