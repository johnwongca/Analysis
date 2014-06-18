
CREATE TABLE [EODData].[Split]
(
	[Exchange] [varchar](10) not null,
	[Symbol] [varchar](32) not null,
	[Date] [date] NOT NULL,
	[Ratio] [varchar](10) NOT NULL, 
    CONSTRAINT [PK_EODData_Split] PRIMARY KEY(Exchange, Symbol, Date) with( STATISTICS_NORECOMPUTE = on)
) 
