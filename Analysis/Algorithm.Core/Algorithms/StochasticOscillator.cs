using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Algorithm.Core
{
    [Description(@"Stochastic Oscillator, Developed by George C. Lane in the late 1950s, the Stochastic Oscillator is a momentum indicator that shows the location of the close relative to the high-low range over a set number of periods. According to an interview with Lane, the Stochastic Oscillator ""doesn't follow price, it doesn't follow volume or anything like that. It follows the speed or the momentum of price. As a rule, the momentum changes direction before price."" As such, bullish and bearish divergences in the Stochastic Oscillator can be used to foreshadow reversals. This was the first, and most important, signal that Lane identified. Lane also used this oscillator to identify bull and bear set-ups to anticipate a future reversal. Because the Stochastic Oscillator is range bound, is also useful for identifying overbought and oversold levels.

%K = (Current Close - Lowest Low)/(Highest High - Lowest Low) * 100
%D = 3-day SMA of %K
Lowest Low = lowest low for the look-back period
Highest High = highest high for the look-back period
%K is multiplied by 100 to move the decimal point two places

The default setting for the Stochastic Oscillator is 14 periods, which can be days, weeks, months or an intraday timeframe. A 14-period %K would use the most recent close, the highest high over the last 14 periods and the lowest low over the last 14 periods. %D is a 3-day simple moving average of %K. This line is plotted alongside %K to act as a signal or trigger line. 

It requires 3 inputs
1: High, 2:Close, 3:Low

Interpretation

The Stochastic Oscillator measures the level of the close relative to the high-low range over a given period of time. Assume that the highest high equals 110, the lowest low equals 100 and the close equals 108. The high-low range is 10, which is the denominator in the %K formula. The close less the lowest low equals 8, which is the numerator. 8 divided by 10 equals .80 or 80%. Multiply this number by 100 to find %K %K would equal 30 if the close was at 103 (.30 x 100). The Stochastic Oscillator is above 50 when the close is in the upper half of the range and below 50 when the close is in the lower half. Low readings (below 20) indicate that price is near its low for the given time period. High readings (above 80) indicate that price is near its high for the given time period. The IBM example above shows three 14-day ranges (yellow areas) with the closing price at the end of the period (red dotted) line. The Stochastic Oscillator equals 91 when the close was at the top of the range. The Stochastic Oscillator equals 15 when the close was near the bottom of the range. The close equals 57 when the close was in the middle of the range.
 
more information: http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:stochastic_oscillator

 ")]
    public class StochasticOscillator:Indicator
    {
        Window<double> highestHigh, lowestLow;
        
        int mPeriod;
        public int Period { get { return mPeriod; } }
        public Window<double> Average { get; set; }
        public StochasticOscillator(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            mPeriod = period;
            highestHigh = Max(mPeriod, 1);
            lowestLow = Min(mPeriod, 1);
            Average = null;
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            if (Average == null)
                Average = SimpleMovingAverage(3, Size);
            
            highestHigh.Push(High);
            lowestLow.Push(Low);
            double denominator = highestHigh.Value - lowestLow.Value;
            Value = denominator < double.Epsilon ? 0d : (Close.Value - lowestLow.Value)*100d / denominator;
            Average.Push(this);
        }
    }
    public partial class IndicatorBase
    {
        public StochasticOscillator StochasticOscillator(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new StochasticOscillator(period, size) { Source = this.Source };
        }
        public static StochasticOscillator Create_StochasticOscillator(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new StochasticOscillator(period, size) { Source = source };
        }
    }
}
