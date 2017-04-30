CREATE procedure q.GetFundamental @SymbolID int
as
begin
	set nocount on
	select
		s.Exchange, s.Symbol, s.LongName,f.SymbolID, f.EffectiveDate, f.ExpiryDate, isnull(sec.Name, '') Sector, isnull(ind.Name,'') Industry, f.Dividend, f.DividendDate, f.DividendYield, f.DPS, f.EBITDA, f.MarketCap, f.EPS, f.PtS, f.NTA, f.PE, f.PEG, f.PtB, f.Shares, f.Yield
	from  q.Fundamental  f
		inner join q.Symbol s on f.SymbolID = s.SymbolID
		inner join q.Name sec on sec.NameID = f.Sector
		inner join q.Name Ind on Ind.NameID = f.Industry
	where f.SymbolID = @SymbolID
		and ExpiryDate is null
end