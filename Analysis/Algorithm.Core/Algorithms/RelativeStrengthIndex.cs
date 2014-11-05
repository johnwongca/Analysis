using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Algorithm.Core
{
    [Description(@"Relative Strength Index is a vairation of Relative Strength
It calculates moving average of Gain and loss in a period as 2 inputs for Relative Strength
return = 100*1/(1+ MA(Gain)/MA(Loss))
when MA(Loss) is 0, output is 100
")]
    public class RelativeStrengthIndex:Indicator
    {
        Window<double> mAverageGain = null, mAverageLoss = null;
        public bool UseInternalAverage = false;
        int mPeriod;
        public int Period { get { return mPeriod; } }
        public RelativeStrengthIndex(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            mPeriod = period;
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            if(mAverageGain==null)
            {
                UseInternalAverage = true;
                mAverageGain = SimpleMovingAverage(Period, 1);
                mAverageGain.InputCacheSize = Period + 1;
                mAverageLoss = SimpleMovingAverage(Period, 1);
                mAverageLoss.InputCacheSize = Period + 1;
            }
            if (UseInternalAverage)
            {
                mAverageGain.Push(values[0].Gain());
                mAverageLoss.Push(values[0].Loss());
            }
            Value = mAverageGain.RelativeStrength(mAverageLoss);
        }
    }
    public partial class IndicatorBase
    {
        public RelativeStrengthIndex RelativeStrengthIndex(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new RelativeStrengthIndex(period, size) { Source = this.Source };
        }
        public static RelativeStrengthIndex Create_RelativeStrengthIndex(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new RelativeStrengthIndex(period, size) { Source = source };
        }
    }
}
