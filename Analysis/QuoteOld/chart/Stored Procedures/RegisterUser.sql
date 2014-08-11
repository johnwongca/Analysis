create procedure chart.RegisterUser
as
begin
	set nocount on
	insert into chart.ChartUser(LoginName)
		select system_user
		where not exists(select * from chart.ChartUser where LoginName = system_user)
end
