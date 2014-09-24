using System;
using System.Collections.Generic;


namespace Algorithm.Core
{
    public class SimpleMovingAverage:Sum
    {
        public SimpleMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(period, size)
        {
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            base.AfterSetValue(values);
            Value = Value / Convert.ToDouble(Math.Min(mPeriod, CurrentLocation + 1));
        }
    }
    public partial class IndicatorBase
    {
        public SimpleMovingAverage SimpleMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new SimpleMovingAverage(period, size) { Source = this.Source };
        }
        public static SimpleMovingAverage Create_SimpleMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new SimpleMovingAverage(period, size) { Source = source };
        }
    }
}
