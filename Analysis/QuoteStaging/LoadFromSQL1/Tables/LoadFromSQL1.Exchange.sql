CREATE TABLE LoadFromSQL1.Exchange
(
	Exchange varchar(10) not null,
	ExchangeName varchar(128),
	Country varchar(10),
	Currency varchar(10),
	TimeZone varchar(128), 
    CONSTRAINT [PK_LoadFromSQL1_Exchange] PRIMARY KEY (Exchange)
)
