alter function PRG.QuoteStr
(
	@str varchar(max)
)
returns varchar(max)
as
begin
	return ''''+replace(@str,'''','''''')+'''';
end;