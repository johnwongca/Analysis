using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Algorithm.Core
{
    [Description(@"Weighted Moving Average
It gives weigt for the first value in the window 1, 2 for second, 3 for third, ...etc.
Sum all weighted value in the window: 1*first value + 2* second value + 3* third value...
then divide it by sum of the weight, 1+2+3....")]
    public class WeightedMovingAverage:Indicator
    {
        double denominator = 0, numerator = 0;
        Sum sum = null;
        int mPeriod;
        public int Period { get { return mPeriod; } }

        public WeightedMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            mPeriod = period;
            denominator = (1d + mPeriod.ToDouble()) * mPeriod.ToDouble() / 2d;
            sum = Sum(period, 2);
        }
        
        protected override void AfterSetValue(params Window<double>[] values)
        {
            if (CurrentLocation < mPeriod)
            {
                numerator = numerator + (CurrentLocation + 1) * values[0].Value;
                sum.Push(values);
                return;
            }
            numerator = numerator + mPeriod * values[0].Value- sum.Value;
            sum.Push(values);
            Value = numerator / denominator;
        }
    }
    public partial class IndicatorBase
    {
        public WeightedMovingAverage WeightedMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new WeightedMovingAverage(period, size) { Source = this.Source };
        }
        public static WeightedMovingAverage Create_WeightedMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new WeightedMovingAverage(period, size) { Source = source };
        }
    }
}
