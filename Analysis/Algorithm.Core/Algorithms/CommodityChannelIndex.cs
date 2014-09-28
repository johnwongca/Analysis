using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Algorithm.Core
{
    [Description(@"Developed by Donald Lambert and featured in Commodities magazine in 1980, the Commodity Channel Index (CCI) is a versatile indicator that can be used to identify a new trend or warn of extreme conditions. Lambert originally developed CCI to identify cyclical turns in commodities, but the indicator can successfully applied to indices, ETFs, stocks and other securities. In general, CCI measures the current price level relative to an average price level over a given period of time. CCI is relatively high when prices are far above their average. CCI is relatively low when prices are far below their average. In this manner, CCI can be used to identify overbought and oversold levels. 

Interpretation: CCI measures the difference between a security's price change and its average price change. High positive readings indicate that prices are well above their average, which is a show of strength. Low negative readings indicate that prices are well below their average, which is a show of weakness.
The Commodity Channel Index (CCI) can be used as either a coincident or leading indicator. As a coincident indicator, surges above +100 reflect strong price action that can signal the start of an uptrend. Plunges below -100 reflect weak price action that can signal the start of a downtrend.
As a leading indicator, chartists can look for overbought or oversold conditions that may foreshadow a mean reversion. Similarly, bullish and bearish divergences can be use to detect early momentum shifts and anticipate trend reversals.

Calcluation: (Close - Moving Average)/(0.015* MeanDeviation(Close))
you can also use typical price for the calculation
")]
    public class CommodityChannelIndex:Indicator
    {
        MeanDeviation meanDeviation;
        public Window<double> Average;
        bool IsInternalAverage = false;
        int mPeriod;
        public int Period { get { return mPeriod; } }
        public CommodityChannelIndex(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            mPeriod = period;
            meanDeviation = MeanDeviation(mPeriod, 1);
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            if (CurrentLocation == 0)
            {
                IsInternalAverage = Average == null;
                if (IsInternalAverage)
                    Average = SimpleMovingAverage(Period, 1);
            }
            if (IsInternalAverage)
                Average.Push(values);
            meanDeviation.Push(values);
            double denominator = 0.015 * meanDeviation.Value;
            Value = denominator < double.Epsilon ? 0 : (values[0].Value - Average.Value) / denominator;
            //Value = meanDeviation.Value;
        }
    }
    public partial class IndicatorBase
    {
        public CommodityChannelIndex CommodityChannelIndex(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new CommodityChannelIndex(period, size) { Source = this.Source };
        }
        public static CommodityChannelIndex Create_CommodityChannelIndex(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new CommodityChannelIndex(period, size) { Source = source };
        }
    }
}
