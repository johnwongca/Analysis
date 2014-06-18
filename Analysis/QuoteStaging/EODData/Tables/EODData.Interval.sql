CREATE TABLE [EODData].[Interval]
(
	[IntervalID] [tinyint] NOT NULL,
	[IntervalName] [varchar](16) NOT NULL,
	[Minute] [tinyint] NULL,
	Constraint PK_EODData_Interval primary key(IntervalID),
	Constraint UK_EODData_Interval unique(IntervalName)
)
