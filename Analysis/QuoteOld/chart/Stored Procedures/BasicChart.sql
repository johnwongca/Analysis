CREATE procedure chart.BasicChart(@Exchange varchar(10) = 'NASDAQ', @Symbol varchar(32) = 'DLLR', @IncludeTime bit = 0)
as
begin
	declare @Chart xml = null, @Data xml = null
	declare @SQL nvarchar(max) 
	if @IncludeTime = 0
		select @SQL = 'select e.ExchangeID, e.Exchange, s.SymbolID, s.Symbol, s.SymbolName, q.QuoteID, q.Date, q.High, q.[Open], q.[Close], q.Low, q.Volume
						from q.Quote q
							inner join q.Symbol s on s.SymbolID = q.SymbolID
							inner join q.Exchange e on e.ExchangeID = s.ExchangeID
						where s.Symbol = @Symbol
							and e.Exchange = @Exchange
							--and q.Volume > 0
						order by q.SymbolID, q.Date Asc' 
	select @Data = (
					select 
							(
							select 
									@SQL [@SQL],
									(
										select *
										from (values ('@Exchange', 'varchar', 10, @Exchange), ('@Symbol', 'varchar', 32, @Symbol)) a([@Name], [@Type], [@Size], [@Value])
										for xml path('Parameter'), type
									) Parameters
							for xml path('Source'), type
							),
							(
							select cast(c as xml)
							from ( values
									(cast('<Algorithm AlgorithmName="Window" WindowSize="200" Output="W200" Visible="True"><Inputs><Input Name="Close" /></Inputs></Algorithm>' as xml))
									,(cast('<Algorithm AlgorithmName="Average" Window="W200" Output="MovingAverage200" Visible="True" />' as xml) )
									,(cast('<Algorithm AlgorithmName="StandardDeviation" Window="W200" Period="20" Output="StandardDeviation" Visible="True" />' as xml) )
									,(cast('<Algorithm AlgorithmName="BollingerBands" IsUpperBand="true" Window="W200" Period="20" Output="BollingerBandsUp20" Visible="True" />' as xml) )
									,(cast('<Algorithm AlgorithmName="Average" Window="W200" Period="20" Output="MovingAverage20" Visible="True" />' as xml) )
									,(cast('<Algorithm AlgorithmName="BollingerBands" IsUpperBand="false" Window="W200" Period="20" Output="BollingerBandsDown20" Visible="True" />' as xml) )
									,(cast('<Algorithm AlgorithmName="Average" Window="W200" Period="13" Output="MA13" Visible="False" />' as xml) )
									,(cast('<Algorithm AlgorithmName="Average" Window="W200" Period="26" Output="MA26" Visible="False" />' as xml) )
									,(cast('<Algorithm AlgorithmName="Subtract" Output="MACD13_26" Visible="True"><Inputs><Input Name="MA13" /><Input Name="MA26" /></Inputs></Algorithm>' as xml) )
									,(cast('<Algorithm AlgorithmName="Window" WindowSize="3" Output="WMACD13_26" Visible="false"><Inputs><Input Name="MACD13_26" /></Inputs></Algorithm>' as xml))
									,(cast('<Algorithm AlgorithmName="Average" Window="WMACD13_26" Output="MACDSignal" Visible="True" />' as xml) )
								) x(c)
							for xml path(''), type
							) Algorithms
							for xml path ('Data')
						)
	select ID = 1, Chart = @Chart, Data = @Data 

	--exec dbo.CLRGetBasicChart @Data
					
	
end
