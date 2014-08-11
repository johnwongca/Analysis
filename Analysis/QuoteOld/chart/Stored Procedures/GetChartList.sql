CREATE procedure [chart].[GetChartList] @ChartID int = null
as
begin
	set nocount on
	if @ChartID is null
		select 
				ChartID, ExchangeID, Exchange, 
				Symbol, SymbolID, ChartName,
				GroupItemCount, iif(GroupItemCount > 1, Exchange + '-' + Symbol, '') GroupName, 
				Exchange + '-' + Symbol +' ' + ChartName MenuItemName,
				SourceIntervalID, DisplayStartID, DisplayRange,
				Algorithm, ChartDefinition
			
		from(
				select 
						ChartID, Exchange, Symbol, ExchangeID, SymbolID,
						ChartName, count(*) over(partition by Exchange, Symbol) GroupItemCount, 
						SourceIntervalID, DisplayStartID, DisplayRange,
						Algorithm, ChartDefinition
				from Chart.ChartList 
			) cl
		order by Exchange, Symbol, ChartName
	else
		select 
				ChartID, ExchangeID, Exchange, 
				Symbol, SymbolID, ChartName,
				GroupItemCount, iif(GroupItemCount > 1, Exchange + '-' + Symbol, '') GroupName, 
				Exchange + '-' + Symbol +' ' + ChartName MenuItemName,
				SourceIntervalID, DisplayStartID, DisplayRange,
				Algorithm, ChartDefinition
			
		from(
				select 
						ChartID, Exchange, Symbol, ExchangeID, SymbolID,
						ChartName, count(*) over(partition by Exchange, Symbol) GroupItemCount, 
						SourceIntervalID, DisplayStartID, DisplayRange,
						Algorithm, ChartDefinition
				from Chart.ChartList 
				where ChartID = @ChartID
			) cl
		
end
