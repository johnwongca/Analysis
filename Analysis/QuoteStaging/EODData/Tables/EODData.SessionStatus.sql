
CREATE TABLE EODData.SessionStatus
(
	ManagementThreadID INT NOT NULL,
	TaskSessionID smallint,
	TaskID int,
	PoolID smallint,
	MethodName varchar(50),
	IntervalID tinyint,
	Exchange varchar(10),
	Symbol varchar(32),
	DateFrom date,
	DateTo date,
	Status varchar(50),
	StatusDate datetime,
	Error varchar(max),
	Constraint PK_EODData_ExecutionStatus primary key  (ManagementThreadID)
) 