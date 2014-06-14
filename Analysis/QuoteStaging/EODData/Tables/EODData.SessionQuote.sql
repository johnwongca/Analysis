CREATE TABLE [EODData].[SessionQuote]
(
	[Exchange] [varchar](10) not null,
	[Symbol] [varchar](32) not null,
	[Date] [datetime2](0) NOT NULL,
	[Open] [float] NOT NULL,
	[Close] [float] NOT NULL,
	[High] [float] NOT NULL,
	[Low] [float] NOT NULL,
	[Volume] [bigint] NOT NULL,
	[Ask] [float] NOT NULL,
	[Bid] [float] NOT NULL,
	[OpenInterest] [bigint] NOT NULL, 
    CONSTRAINT [PK_EODData_SessionQuote] PRIMARY KEY (Exchange, Symbol, Date) ,
) 