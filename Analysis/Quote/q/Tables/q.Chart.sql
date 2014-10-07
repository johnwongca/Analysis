CREATE TABLE [q].[Chart]
(
	ChartName varchar(128) NOT NULL,
	[AlgorithmName] VARCHAR(128) NOT NULL, 
	SymbolID int not null constraint DF_q_Chart_SymbolID default(170976), -- microsoft
	IntervalType varchar(20) not null constraint DF_q_Chart_IntervalType default('Minutes'),
	Interval int not null constraint DF_q_Chart_Interval default(1),
    [ChartDefinition] XML NOT NULL, 
    constraint PK_q_Chart primary key(ChartName)
)
