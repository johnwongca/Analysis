use Stock
go
if OBJECT_ID('f.GetANumber') is not null
	drop function f.GetANumber
go
create function f.GetANumber()
returns int
begin 
	return Sequence.SessionNextVal('#f.GetNumber', @@SPID)
end;
go
if OBJECT_ID('f.GetADate') is not null
	drop function f.GetADate
go
create function f.GetADate()
returns datetime2(3)
begin 
	return dateadd(millisecond, f.GetANumber(), getdate())
end;
go
select f.GetADate()	, f.GetANumber()
go