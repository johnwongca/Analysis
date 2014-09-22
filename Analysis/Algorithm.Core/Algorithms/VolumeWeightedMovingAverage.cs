using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Algorithm.Core
{
    [Description(@"")]
    public class VolumeWeightedMovingAverage:Indicator
    {
        Window<double> mWeight = null, s1;
        Sum sum0, sum1;
        int mPeriod;
        public int Period { get { return mPeriod; }}
        [Description(@"This value is default to Volume")]
        public Window<double> Weight { get { return mWeight; } set { mWeight = value; } }
        public VolumeWeightedMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            mPeriod = period;
            sum0 = Sum(period, 1);
            sum1 = Sum(period, 1);
            s1 = new Window<double>(period + 1);
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            if(Weight==null)
            {
                if(Source!=null)
                {
                    Weight = Source.Volume;
                }
            }
            if(values.Length > 1)
            {
                Weight = values[1];
            }
            s1.Push(Weight * values[0]);
            sum0.Push(s1);
            sum1.Push(Weight);
            Value = sum1 < double.Epsilon ? values[0].Value : sum0 / sum1;
        }
    }
    public partial class IndicatorBase
    {
        public VolumeWeightedMovingAverage VolumeWeightedMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new VolumeWeightedMovingAverage(period, size) { Source = this.Source, Weight = Volume };
        }
        
        public static VolumeWeightedMovingAverage Create_VolumeWeightedMovingAverage(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new VolumeWeightedMovingAverage(period, size) { Source = source, Weight = source.Volume };
        }
    }
}
