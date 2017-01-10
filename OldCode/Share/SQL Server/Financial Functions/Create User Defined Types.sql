USE Stock
GO
if type_id('f.Stock') is not null
	drop type f.Stock
go
create type f.Stock AS TABLE(
	ID int NOT NULL identity(1,1) primary key,
	[Date] datetime2(3) NOT NULL unique,
	High float,
	[Open] float,
	[Close] float,
	Low float,
	interest float,
	Volume float,
	info xml
)
GO
if type_id('f.Array1') is not null
	drop type f.Array1
go
CREATE TYPE f.Array1 AS TABLE(
	ID int NOT NULL identity(1,1) primary key,	
	V1 float NOT NULL,
	Info xml
)
go
if type_id('f.Array2') is not null
	drop type f.Array2
go
CREATE TYPE f.Array2 AS TABLE(
	ID int NOT NULL identity(1,1) primary key,  
	V1 float NOT NULL,
	V2 float NOT NULL,
	Info xml
)
go
if type_id('f.Array3') is not null
	drop type f.Array3
go
CREATE TYPE f.Array3 AS TABLE(
	ID int NOT NULL identity(1,1) primary key,  
	V1 float NOT NULL,
	V2 float NOT NULL,
	V3 float NOT NULL,
	Info xml
)
go




