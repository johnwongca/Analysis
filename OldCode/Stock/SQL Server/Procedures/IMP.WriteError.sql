alter procedure IMP.WriteError
(
	@Proc nvarchar(256),
	@Parameter nvarchar(max),
	@Error	nvarchar(max)
)
as
begin
	insert into Error(Name, Parameter, Error)
		values(@Proc, @Parameter, @Error)
end