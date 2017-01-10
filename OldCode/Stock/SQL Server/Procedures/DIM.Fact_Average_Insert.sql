alter procedure DIM.Fact_Average_Insert
(
	@SymbolID int,
	@Seq int,
	@Table varchar(20) = 'A.Daily'
)
as
begin
	declare @Performance_Date datetime
	select @Performance_Date = getdate()
	declare @SQL varchar(max)	
	select @SQL = 'select a.SymbolID, a.Seq, a.Date, 1, Opening from ' + @Table + ' a where SymbolID = ' + cast(@SymbolID as varchar(20)) + ' and Seq = ' + cast(@Seq as varchar(20))
	insert into DIM.Fact_Average(SymbolID, Seq, Date, FormulaID, Value)
		exec DIM.Ex @SQL 

	select @SQL = 'select a.SymbolID, a.Seq, a.Date, 2, High from ' + @Table + ' a where SymbolID = ' + cast(@SymbolID as varchar(20)) + ' and Seq = ' + cast(@Seq as varchar(20))
	insert into DIM.Fact_Average(SymbolID, Seq, Date, FormulaID, Value)
		exec DIM.Ex @SQL 

	select @SQL = 'select a.SymbolID, a.Seq, a.Date, 3, Low from ' + @Table + ' a where SymbolID = ' + cast(@SymbolID as varchar(20)) + ' and Seq = ' + cast(@Seq as varchar(20))	
	insert into DIM.Fact_Average(SymbolID, Seq, Date, FormulaID, Value)
		exec DIM.Ex @SQL 

	select @SQL = 'select a.SymbolID, a.Seq, a.Date, 4, Closing from ' + @Table + ' a where SymbolID = ' + cast(@SymbolID as varchar(20)) + ' and Seq = ' + cast(@Seq as varchar(20))	
	insert into DIM.Fact_Average(SymbolID, Seq, Date, FormulaID, Value)
		exec DIM.Ex @SQL 

	select @SQL = 'select a.SymbolID, a.Seq, a.Date, 5, Volume from ' + @Table + ' a where SymbolID = ' + cast(@SymbolID as varchar(20)) + ' and Seq = ' + cast(@Seq as varchar(20))	
	insert into DIM.Fact_Average(SymbolID, Seq, Date, FormulaID, Value)
		exec DIM.Ex @SQL 

	select @SQL = 'select a.SymbolID, a.Seq, a.Date, 6, Interest from ' + @Table + ' a where SymbolID = ' + cast(@SymbolID as varchar(20)) + ' and Seq = ' + cast(@Seq as varchar(20))	
	insert into DIM.Fact_Average(SymbolID, Seq, Date, FormulaID, Value)
		exec DIM.Ex @SQL 

	insert into DIM.Fact_Average(SymbolID, Seq, Date, FormulaID, Value)
		select  @SymbolID, 
				@Seq, 
				DIM.RunSQLDate('select Date from ' + @Table + ' where SymbolID = ' + cast(@SymbolID as varchar(20)) + ' and Seq = ' + cast(@Seq as varchar(20)) ), 
				A.ID as FormulaID,
				isnull(
						A.Cal_Average(
										@SymbolID,
										@Seq,
										null,
										case c.ID 
											when 2 then 'Simple'
											when 3 then 'Weighted'
											when 4 then 'Exponential'
										end,
										b.ID,
										'Closing',
										@Table
									),0
						)
		from DIM.Formula a
			inner join DIM.A_FormulaPeriod b on a.PeriodID = b.ID
			inner join DIM.A_FormulaType c on a.TypeID = c.ID
		where c.ID in (2, 3, 4)
		insert into DIM.Performance	(SymbolID, Seq, Start, Finish)
			values(@SymbolID, @Seq, @Performance_Date, getdate())
end