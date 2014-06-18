create trigger EODData.TRI_Country on [EODData].[vCountry]
instead of insert
AS 
BEGIN
	set nocount on 
	merge EODData.Country as t
	using (
			select *
			from (
				select  CountryCode, CountryName, row_number()over (partition by CountryCode order by CountryCode) RowNumber
				from inserted
				) R where RowNumber = 1
			) s on s.CountryCode = t.CountryCode
	when matched and isnull(t.CountryName,'') <> isnull(s.CountryName,'') then 
		update set CountryName = s.CountryName
	when not matched then 
		insert (CountryCode, CountryName)
			values(s.CountryCode, s.CountryName)
	option(loop join);
END