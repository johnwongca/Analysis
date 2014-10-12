create procedure q.GetToken
as
begin
	select next value for q.TokenSequence as Token
end