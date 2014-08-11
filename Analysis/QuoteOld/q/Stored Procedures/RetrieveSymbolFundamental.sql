CREATE procedure [q].[RetrieveSymbolFundamental](@SymbolID int)
as
begin
	select 
			b.SymbolID, b.ExchangeID, b.Symbol, b.SymbolName, b.LongName,
			a.Date, a.Sector, a.Industry, a.Dividend, a.DividendDate, a.DividendYield, a.DSP, a.EBITDA, a.MarketCap, a.EPS, a.PtS, a.NTA, a.PE, a.PEG, a.PtB, a.Shares, a.Yield
	from q.Fundamental a
		inner join q.Symbol b on a.SymbolID = b.SymbolID
	where b.SymbolID = @SymbolID
end
