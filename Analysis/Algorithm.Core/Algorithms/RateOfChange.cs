using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Algorithm.Core
{
    [Description(@"Rate of Change (ROC) can be applied to Close and Volumes
Caculation: (today's value - N days' ago's value)*100/N days' ago's value

When pushing 2 data windows to the indicator, period will be ignored, the rate of change will be calculated between those 2")]
    public class RateOfChange:Indicator
    {
        int mPeriod;
        public int Period { get { return mPeriod; } }
        public RateOfChange(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            mPeriod = period;
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            if(values.Length == 1)
            {
                Value = values[0].HasValue(Period - 1) ? (values[0].Value - values[0][Period - 1]) * 100d / Math.Max(values[0][Period - 1], 1d) : 0d;
                return;
            }
            else if (values.Length > 1)
            {
                Value = values[1].HasValue(0) ? (values[0].Value - values[1].Value) * 100d / Math.Max(values[1].Value, 0d) : 0d;
            }
        }
    }
    public partial class IndicatorBase
    {
        public RateOfChange RateOfChange(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new RateOfChange(period, size) { Source = this.Source };
        }
        public static RateOfChange Create_RateOfChange(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new RateOfChange(period, size) { Source = source };
        }
    }
}
