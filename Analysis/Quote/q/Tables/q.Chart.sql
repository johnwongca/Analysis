CREATE TABLE [q].[Chart]
(
	ChartName varchar(128) NOT NULL,
    [ChartDefinition] XML NOT NULL, 
    constraint PK_q_Chart primary key(ChartName)
)
