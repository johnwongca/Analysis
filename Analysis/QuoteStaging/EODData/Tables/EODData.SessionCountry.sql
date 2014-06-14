CREATE TABLE [EODData].[SessionCountry]
(
	CountryCode varchar(5) not null,
	CountryName varchar(128) null, 
    CONSTRAINT [PK_EODData_SessionCountry] PRIMARY KEY(CountryCode)  

) 