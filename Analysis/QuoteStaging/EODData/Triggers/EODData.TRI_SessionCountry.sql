create trigger EODData.TRI_SessionCountry on [EODData].[SessionCountry]
instead of insert
AS 
BEGIN
	set nocount on 
	merge EODData.SessionCountry as t
	using inserted s on s.CountryCode = t.CountryCode
	when matched and isnull(t.CountryName,'') <> isnull(s.CountryName,'') then 
		update set CountryName = s.CountryName
	when not matched then 
		insert (CountryCode, CountryName)
			values(s.CountryCode, s.CountryName)
	;
END