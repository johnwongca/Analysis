CREATE TABLE [EODData].[Country]
(
	CountryCode varchar(5) not null,
	CountryName varchar(128) null, 
    CONSTRAINT [PK_EODData_Country] PRIMARY KEY(CountryCode)  with(Ignore_DUP_Key=on)

) 