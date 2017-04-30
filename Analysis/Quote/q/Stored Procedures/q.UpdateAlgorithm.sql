CREATE procedure [q].[UpdateAlgorithm]
(
	@AlgorithmName varchar(128),
	@Class varchar(256),
	@Description varchar(max)
)
as
begin
	set nocount on
	update q.Algorithm
		set Class = @Class, Description = @Description
	where AlgorithmName = @AlgorithmName
	if @@rowcount = 0
		insert into q.Algorithm(AlgorithmName, Class, Description)
			values(@AlgorithmName, @Class, @Description)
end