CREATE TABLE [EODData].[SessionSymbolChange]
(
	[Date] [date] NOT NULL,
	[FromExchange] [varchar](10) not null,
	[FromSymbol] [varchar](32) not null,
	[ToExchange] [varchar](10) not null,
	[ToSymbol] [varchar](32) not null, 
    CONSTRAINT [PK_EODData_SessionSymbolChange] PRIMARY KEY(Date, FromExchange, FromSymbol, ToExchange, ToSymbol) ,
) 
