CREATE TABLE [q].[Name]
(
	NameID INT NOT NULL identity(1,1) constraint PK_q_Name PRIMARY KEY,
	Name varchar(128) not null constraint UK_q_Name unique with(ignore_dup_key = on)
)
