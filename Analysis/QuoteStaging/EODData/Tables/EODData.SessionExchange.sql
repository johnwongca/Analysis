
CREATE TABLE [EODData].[SessionExchange]
(
	[Exchange] [varchar](10) not null,
	[Name] [varchar](128) NOT NULL,
	[Country] [varchar](5) NULL,
	[Currency] [varchar](10) NULL,
	[Delines] [int] NULL,
	[HasIntradayProduct] [bit] NULL,
	[IntradayStartDate] [datetime] NULL,
	[IsIntraday] [bit] NULL,
	[LastTradeDateTime] [datetime] NULL,
	[Suffix] [varchar](10) NULL,
	[TimeZone] varchar(50) NULL, 
    CONSTRAINT [PK_EODData_SessionExchange] PRIMARY KEY(Exchange) 
) 