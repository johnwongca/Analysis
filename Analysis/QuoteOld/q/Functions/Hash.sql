CREATE function [q].[Hash](@Value sql_variant)
returns uniqueidentifier
as
begin
	return cast(hashbytes('MD5', convert(varbinary(8000), @Value)) as uniqueidentifier)
end


