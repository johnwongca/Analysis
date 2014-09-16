using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Algorithm.Core
{
    [Description(@"Calculation: Value *(1- (2/(1+period))) + ((2/(1+period)) * previous Exponential Moving Average")]
    public class ExponentialMovingAverage:Indicator
    {
        double previous;
        public int Period { get; set; }
        public bool IsWildersSmoothing { get; set; }

        public ExponentialMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            Period = period;
            IsWildersSmoothing = false;
        }
        
        protected override void AfterSetValue(params Window<double>[] values)
        {
            double pct;
            pct = IsWildersSmoothing ? 1d / Period.ToDouble() : 2d / (1d + Period.ToDouble());
            if (CurrentLocation == 0)
            {
                previous = values[0];
                return;
            }
            Value = IsWildersSmoothing ? previous + pct * (values[0] - previous) : values[0] * pct + previous * (1d - pct);
            previous = Value;
        }
    }
    public partial class IndicatorBase
    {
        public ExponentialMovingAverage ExponentialMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new ExponentialMovingAverage(period, size) { Source = this.Source };
        }
        public static ExponentialMovingAverage Create_ExponentialMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new ExponentialMovingAverage(period, size) { Source = source };
        }
    }
}
