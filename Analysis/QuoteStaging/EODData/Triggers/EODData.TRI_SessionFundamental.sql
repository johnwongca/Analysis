create trigger EODData.TRI_Fundamental on EODData.SessionFundamental
instead of insert
AS 
BEGIN 
	set nocount on
	merge EODData.SessionFundamental t
	using inserted as s on t.Exchange = s.Exchange and t.Symbol = s.Symbol and t.Date = s.Date
	when matched and (
							isnull(t.Name,'') <> isnull(s.Name, '')
						or isnull(t.Description,'') <> isnull(s.Description, '')
						or isnull(t.Dividend,0) <> isnull(s.Dividend, 0)
						or isnull(t.DividendDate, '1960-01-01') <> isnull(s.DividendDate, '1960-01-01')
						or isnull(t.DividendYield, 0) <> isnull(s.DividendYield, 0)
						or isnull(t.DSP, 0) <> isnull(s.DSP, 0)
						or isnull(t.EBITDA, 0) <> isnull(s.EBITDA, 0)
						or isnull(t.EPS, 0) <> isnull(s.EPS, 0)
						or isnull(t.Industry, '') <> isnull(s.Industry, '')
						or isnull(t.MarketCap, 0) <> isnull(s.MarketCap,0)
						or isnull(t.NTA, 0) <> isnull(s.NTA, 0)
						or isnull(t.PE, 0) <> isnull(s.PE,0)
						or isnull(t.PEG, 0) <> isnull(s.PEG, 0)
						or isnull(t.PtB, 0) <> isnull(s.PtB, 0)
						or isnull(t.PtS, 0) <> isnull(s.PtS, 0)
						or isnull(t.Sector, '') <> isnull(s.Sector,0)
						or isnull(t.Shares, 0) <> isnull(s.Shares,0)
						or isnull(t.Yield,0) <> isnull(s.Yield, 0)
					) then
		update set	Name = s.Name, Description = s.Description, Dividend = s.Dividend,
					DividendDate = s.DividendDate, DividendYield = s.DividendYield, DSP = s.DSP,
					EBITDA = s.EBITDA, EPS = s.EPS, Industry = s.Industry,
					MarketCap = s.MarketCap, NTA = s.NTA, PE = s.PE,
					PEG = s.PEG, PtB = s.PtB, PtS = s.PtS,
					Sector = s.Sector, Shares = s.Shares, Yield = s.Yield
	when not matched then
		insert (Exchange, Symbol, Date, Name, Description, Dividend, DividendDate, DividendYield, DSP, EBITDA, EPS, Industry, MarketCap, NTA, PE, PEG, PtB, PtS, Sector, Shares, Yield)
			values(s.Exchange, s.Symbol, s.Date, s.Name, s.Description, s.Dividend, s.DividendDate, s.DividendYield, s.DSP, s.EBITDA, s.EPS, s.Industry, s.MarketCap, s.NTA, s.PE, s.PEG, s.PtB, s.PtS, s.Sector, s.Shares, s.Yield)
	;
END