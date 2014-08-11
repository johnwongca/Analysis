CREATE FUNCTION [dbo].[CLRAlgoritmDescription]
( )
RETURNS 
     TABLE (
        [ID]            INT            NULL,
        [AlgorithmName] [sysname]      NULL,
        [Type]          [sysname]      NULL,
        [Definition]    XML            NULL,
        [Description]   NVARCHAR (MAX) NULL)
AS
 EXTERNAL NAME [QuoteCLR].[QuoteCLR.QuoteAlgorithms].[CLRAlgoritmDescription]

