CREATE procedure [q].[ImportStaging_Fundamental]
(
	@FromVersion binary(8) = 0x00, 
	@ToVersion binary(8) = 0x000000001001372E
)
as
begin
	begin transaction
	merge q.Fundamental t
	using (
			select 
					b.SymbolID, a.Date, a.Sector, 
					a.Industry, a.Dividend, a.DividendDate, 
					a.DividendYield, a.DSP, a.EBITDA, 
					a.MarketCap, a.EPS, a.PtS, 
					a.NTA, a.PE, a.PEG, 
					a.PtB, a.Shares, a.Yield, a.___DataVersion___
			from EODData.staging.Fundamental a
					inner join q.ExchangeSymbol b with (noexpand) on a.ExchangeID = b.Exchange and a.SymbolID = b.Symbol
			where a.___DataVersion___ > @FromVersion
				and a.___DataVersion___ <= @ToVersion
			) s on t.SymbolID = s.SymbolID and t.Date = s.Date
	when Matched and (isnull(t.Sector, '') <> isnull(s.Sector, '') or isnull(t.Industry, '') <> isnull(s.Industry, '') or isnull(t.Dividend, 0) <> isnull(s.Dividend, 0) or isnull(t.DividendDate, '1900-01-01') <> isnull(s.DividendDate, '1900-01-01') or isnull(t.DividendYield, 0) <> isnull(s.DividendYield, 0) or isnull(t.DSP, 0) <> isnull(s.DSP, 0) or isnull(t.EBITDA, 0) <> isnull(s.EBITDA, 0) or isnull(t.MarketCap, 0) <> isnull(s.MarketCap, 0) or isnull(t.EPS, 0) <> isnull(s.EPS, 0) or isnull(t.PtS, 0) <> isnull(s.PtS, 0) or isnull(t.NTA, 0) <> isnull(s.NTA, 0) or isnull(t.PE, 0) <> isnull(s.PE, 0) or isnull(t.PEG, 0) <> isnull(s.PEG, 0) or isnull(t.PtB, 0) <> isnull(s.PtB, 0) or isnull(t.Shares, 0) <> isnull(s.Shares, 0) or isnull(t.Yield, 0) <> isnull(s.Yield, 0)) then
		update
			set t.Sector = s.Sector, 
				t.Industry = s.Industry, 
				t.Dividend = s.Dividend, 
				t.DividendDate = s.DividendDate, 
				t.DividendYield = s.DividendYield, 
				t.DSP = s.DSP, 
				t.EBITDA = s.EBITDA, 
				t.MarketCap = s.MarketCap, 
				t.EPS = s.EPS, 
				t.PtS = s.PtS, 
				t.NTA = s.NTA, 
				t.PE = s.PE, 
				t.PEG = s.PEG, 
				t.PtB = s.PtB, 
				t.Shares = s.Shares, 
				t.Yield = s.Yield
	when not matched then
		insert (SymbolID, Date, Sector, Industry, Dividend, DividendDate, DividendYield, DSP, EBITDA, MarketCap, EPS, PtS, NTA, PE, PEG, PtB, Shares, Yield)
			values(s.SymbolID, s.Date, s.Sector, s.Industry, s.Dividend, s.DividendDate, s.DividendYield, s.DSP, s.EBITDA, s.MarketCap, s.EPS, s.PtS, s.NTA, s.PE, s.PEG, s.PtB, s.Shares, s.Yield)
	;
	delete EODData.staging.Fundamental where ___DataVersion___ <= @ToVersion;
	commit
	return @@rowcount
end
