CREATE TABLE [scheduler].[ServiceApplication]
(
	[ServiceApplicationName] VARCHAR(50) NOT NULL 
	constraint PK_scheduler_ServiceApplication PRIMARY KEY(ServiceApplicationName), 
    [Description] VARCHAR(MAX) NULL, 
    [Active] BIT NOT NULL DEFAULT 1
)
