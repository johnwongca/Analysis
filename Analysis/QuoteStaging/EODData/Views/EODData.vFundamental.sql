create view EODData.vFundamental
as
select Exchange, Symbol, Date, Name, Description, Dividend, DividendDate, DividendYield, DPS, EBITDA, EPS, Industry, MarketCap, NTA, PE, PEG, PtB, PtS, Sector, Shares, Yield from EODData.Fundamental