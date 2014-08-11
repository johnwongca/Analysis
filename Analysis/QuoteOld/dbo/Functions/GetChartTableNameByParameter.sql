CREATE FUNCTION [dbo].[GetChartTableNameByParameter]
(@Parameter XML)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [QuoteCLR].[QuoteCLR.QuoteAlgorithms].[GetChartTableNameByParameter]

