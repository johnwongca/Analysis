-- Add your test scenario here --
if object_ID('PRG.TextFile_GetColumns') is not null
	drop function PRG.[TextFile_GetColumns]
go
if object_ID('[PRG].[TextFile_GetDirectory]') is not null
	drop function [PRG].[TextFile_GetDirectory]
go
if object_ID('[PRG].[TextFile_GetFileInfo]') is not null
	drop function [PRG].[TextFile_GetFileInfo]
go
if object_ID('[PRG].[TextFile_GetLines]') is not null
	drop function [PRG].[TextFile_GetLines]
go
if object_ID('[PRG].[TextFile_ListHandles]') is not null
	drop function [PRG].[TextFile_ListHandles]
go
if object_ID('[PRG].[TextFile_CloseFile]') is not null
	drop function [PRG].[TextFile_CloseFile]
go
if object_ID('[PRG].[TextFile_GetText]') is not null
	drop function [PRG].[TextFile_GetText]
go
if object_ID('[PRG].[TextFile_OpenFile]') is not null
	drop function [PRG].[TextFile_OpenFile]
go
if object_ID('[PRG].[TextFile_OpenString]') is not null
	drop function [PRG].[TextFile_OpenString]
go
if object_ID('[PRG].[TextFile_SetDelimiter]') is not null
	drop function [PRG].[TextFile_SetDelimiter]
go
if object_ID('[PRG].[TextFile_GetTable]') is not null
	drop procedure [PRG].[TextFile_GetTable]
go