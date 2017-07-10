CREATE view EODData.PendingQuoteFile
as
SELECT [Exchange]
      ,[Date]
      ,[FullFileName]
      ,[FileName]
      ,[CreationDate]
      ,[LastModifiedDate]
      ,[Length]
  FROM [EODData].[QuoteFile] a
  where not exists(select * from EODData.LoadedQuote b where a.Exchange = b.Exchange and a.Date = b.Date)