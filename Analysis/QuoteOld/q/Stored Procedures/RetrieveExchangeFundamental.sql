CREATE procedure [q].[RetrieveExchangeFundamental](@ExchangeID int)
as
begin
	select 
			b.SymbolID, b.ExchangeID, b.Symbol, b.SymbolName, b.LongName,
			a.Date, a.Sector, a.Industry, a.Dividend, a.DividendDate, a.DividendYield, a.DSP, a.EBITDA, a.MarketCap, a.EPS, a.PtS, a.NTA, a.PE, a.PEG, a.PtB, a.Shares, a.Yield
	from q.SymbolFundamental a
		inner join q.Symbol b on a.SymbolID = b.SymbolID
	where b.ExchangeID = @ExchangeID
end
