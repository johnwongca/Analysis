-- Add your test scenario here --
if object_ID('PRG.TextFile_GetColumns') is not null
	drop function PRG.[TextFile_GetColumns]
go
CREATE FUNCTION [PRG].[TextFile_GetColumns](@handle [int])
RETURNS  TABLE (
	[LineID] [int] NULL,
	[ColumnID] [int] NULL,
	[Value] [nvarchar](max) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [CommonCLR].[CommonCLR.TextFile].[ColumnTable]
go


if object_ID('[PRG].[TextFile_GetDirectory]') is not null
	drop function [PRG].[TextFile_GetDirectory]
go
CREATE FUNCTION [PRG].[TextFile_GetDirectory](@path [nvarchar](4000))
RETURNS  TABLE (
	[FullName] nvarchar(max), 
	[FileName] nvarchar(256),
	[Size] [bigint] NULL,
	[Type] [nvarchar](10) NULL,
	[FileDate] [datetime] NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [CommonCLR].[CommonCLR.TextFile].[ReadDirectory]
GO


if object_ID('[PRG].[TextFile_GetFileInfo]') is not null
	drop function [PRG].[TextFile_GetFileInfo]
go
CREATE FUNCTION [PRG].[TextFile_GetFileInfo](@handle [int])
RETURNS  TABLE (
	[LineID] [int] NULL,
	[Name] [nvarchar](30) NULL,
	[Value] [nvarchar](max) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [CommonCLR].[CommonCLR.TextFile].[Info]
GO


if object_ID('[PRG].[TextFile_GetLines]') is not null
	drop function [PRG].[TextFile_GetLines]
go
CREATE FUNCTION [PRG].[TextFile_GetLines](@handle [int])
RETURNS  TABLE (
	[LineID] [int] NULL,
	[Value] [nvarchar](max) NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [CommonCLR].[CommonCLR.TextFile].[LineTable]
go


if object_ID('[PRG].[TextFile_ListHandles]') is not null
	drop function [PRG].[TextFile_ListHandles]
go
CREATE FUNCTION [PRG].[TextFile_ListHandles]()
RETURNS  TABLE (
	[Handle] [int] NULL,
	[FileName] [nvarchar](1024) NULL,
	[Size] [bigint] NULL,
	[Type] [nvarchar](10) NULL,
	[FileDate] [datetime] NULL,
	[LoadDate] [datetime] NULL
) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [CommonCLR].[CommonCLR.TextFile].[ListHandles]
GO


if object_ID('[PRG].[TextFile_CloseFile]') is not null
	drop function [PRG].[TextFile_CloseFile]
go
CREATE FUNCTION [PRG].[TextFile_CloseFile](@handle [int])
RETURNS [bit] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [CommonCLR].[CommonCLR.TextFile].[CloseFile]
go


if object_ID('[PRG].[TextFile_GetText]') is not null
	drop function [PRG].[TextFile_GetText]
go
CREATE FUNCTION [PRG].[TextFile_GetText](@handle [int])
RETURNS [nvarchar](max) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [CommonCLR].[CommonCLR.TextFile].[GetText]
GO


if object_ID('[PRG].[TextFile_OpenFile]') is not null
	drop function [PRG].[TextFile_OpenFile]
go
CREATE FUNCTION [PRG].[TextFile_OpenFile](@file [nvarchar](4000))
RETURNS [int] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [CommonCLR].[CommonCLR.TextFile].[OpenFile]
go


if object_ID('[PRG].[TextFile_OpenString]') is not null
	drop function [PRG].[TextFile_OpenString]
go
CREATE FUNCTION [PRG].[TextFile_OpenString](@str [nvarchar](max))
RETURNS [int] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [CommonCLR].[CommonCLR.TextFile].[OpenString]
GO

if object_ID('[PRG].[TextFile_SetDelimiter]') is not null
	drop function [PRG].[TextFile_SetDelimiter]
go
CREATE FUNCTION [PRG].[TextFile_SetDelimiter](@handle [int], @delimiter [nvarchar](4000))
RETURNS [bit] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [CommonCLR].[CommonCLR.TextFile].[SetDelimiter]
GO

if object_ID('[PRG].[TextFile_GetTable]') is not null
	drop procedure [PRG].[TextFile_GetTable]
go
CREATE PROCEDURE [PRG].[TextFile_GetTable]
(
	@handle [int],
	@Columns int = -1
)
AS
EXTERNAL NAME [CommonCLR].[CommonCLR.TextFile].[GetTable]
go