alter procedure DIM.ConformFact_Average
as
begin
	set nocount on
	declare @ret int
	exec @ret = sp_getapplock	@Resource = 'DIM.ConformFact_Average', 
								@LockMode = 'Exclusive',
								@LockOwner = 'Session',
								@LockTimeout = 0
	if @@error <> 0  or @ret <> 0
	begin
		raiserror('Another instance is running in the system.', 16, 1)
	end

	declare @SymbolID int, @Seq int, @Operation varchar(10), @ID bigint
	declare c cursor local fast_forward for
		select  SymbolID, Seq, Operation, ID from DIM.Pending_Daily order by ID
	open c
	fetch next from c into @SymbolID, @Seq, @Operation, @ID
	while @@fetch_status = 0
	begin
		begin try
			If @Operation = 'Insert'
				exec DIM.Fact_Average_Insert @SymbolID, @Seq
			else
				exec DIM.Fact_Average_Delete @SymbolID, @Seq
			delete DIM.Pending_Daily where ID = @ID
		end try
		begin catch
			declare @msg varchar(max)
			select @msg = DIM.GetErrorMessage()
			raiserror('Error occored while calculatint. Message: %s', 16, 1, @msg)
			return
		end catch
		fetch next from c into @SymbolID, @Seq, @Operation, @ID
	end
	close c
	deallocate c
	exec sp_releaseapplock	@Resource = 'DIM.ConformFact_Average', 
							@LockOwner = 'Session'
end