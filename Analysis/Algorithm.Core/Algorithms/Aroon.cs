using System;
using System.Collections.Generic;


namespace Algorithm.Core
{
    public class Sum:Indicator
    {
        protected int mPeriod;
        double previous = 0;
        public int Period { get { return mPeriod; } }
        public Sum(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            mPeriod = period;
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            Value = values[0].Value + previous - (CurrentLocation < mPeriod ? 0d : values[0][mPeriod]);
            previous = Value;
        }
    }
    public partial class IndicatorBase
    {
        public Sum Sum(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new Sum(period, size) { Source = this.Source };
        }
        public static Sum Create_Sum(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new Sum(period, size) { Source = source };
        }
    }
}
