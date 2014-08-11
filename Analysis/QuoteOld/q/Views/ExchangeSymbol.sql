

CREATE view [q].[ExchangeSymbol] with schemabinding
as
	SELECT     
			e.ExchangeID, e.Exchange, s.SymbolID, s.Symbol, s.SymbolName, s.LongName SymbolLongName
	FROM q.Exchange e 
		INNER JOIN q.Symbol s ON e.ExchangeID = s.ExchangeID



GO
CREATE UNIQUE CLUSTERED INDEX [PK_ExchangeSymbol]
    ON [q].[ExchangeSymbol]([Exchange] ASC, [Symbol] ASC);

