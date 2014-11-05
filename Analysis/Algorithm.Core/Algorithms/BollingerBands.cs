using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Algorithm.Core
{
    [Description(@"BollingerBand consists of Upper and Lower bands. They are window operators. 
Upper band: Standard deviation in the window * Delta
Lower band: Standard deviation in the window * -Delta
")]
    public class BollingerBands:Indicator
    {
        double mDelta = 1.8;
        Window<double> mLower;
        public StandardDeviation sd = null;
        int mPeriod;
        bool useInternalAverage = false;
        public int Period { get { return mPeriod; } }
        public double Delta { get { return mDelta; } set { mDelta = value; } }

        public Indicator Average { get; set; }
        public Indicator AverageForStandardDeviation { get; set; }
        public Window<double> Upper { get { return this; } }
        public Window<double> Lower { get { return mLower; } }
        public BollingerBands(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            mPeriod = period;
            mLower = new Window<double>(size);
            Average = null;
            AverageForStandardDeviation = null;
        }
        
        protected override void AfterSetValue(params Window<double>[] values)
        {
            if(Average == null)
            {
                useInternalAverage = true;
                Average = SimpleMovingAverage(Period, Size);
                //Average = ExponentialMovingAverage(Period, Size);
            }
            if(sd == null)
                sd = StandardDeviation(Period, AverageForStandardDeviation, 1);

            if (useInternalAverage)
                Average.Push(values);
            sd.Push(values);
            Value = (Average.Value + sd.Value * Delta);
            Lower.Push(Average.Value - sd.Value * Delta);
        }
    }
    public partial class IndicatorBase
    {
        public BollingerBands BollingerBands(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new BollingerBands(period, size) { Source = this.Source };
        }
        public static BollingerBands Create_BollingerBands(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new BollingerBands(period, size) { Source = source };
        }
    }
}
