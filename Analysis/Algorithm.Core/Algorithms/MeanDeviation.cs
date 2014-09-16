using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Algorithm.Core
{
    public class MeanDeviation:Indicator
    {
        [Description("Average by default is mean value. It can be set to be any type of average values.")]
        public Window<double> Average { get; set; }
        bool isAverageInternal = true;
        public int Period { get; set; }
        
        public MeanDeviation(int period, Window<double> average = null, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            Period = period;
            if (average == null)
            {
                isAverageInternal = true;
            }
            else
            {
                Average = average;
                isAverageInternal = false;
            }
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            if (Average == null)
                Average = SimpleMovingAverage(Period, 1);
            if(isAverageInternal)
            {
                Average.Push(values);
            }
            double v = 0;
            for(int i=0; i<Period; i++)
            {
                if (!values[0].HasValue(i))
                    break;
                v = v + Math.Abs(values[0][i] - Average.Value);
            }
            Value = v / Period.ToDouble();
        }
    }
    public partial class IndicatorBase
    {
        public MeanDeviation MeanDeviation(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new MeanDeviation(period, null, size) { Source = this.Source };
        }
        public MeanDeviation MeanDeviation(int period, Window<double> average ,int size = Window<double>.DefaultDataWindowSize)
        {
            return new MeanDeviation(period, size) { Source = this.Source };
        }
        public static MeanDeviation Create_MeanDeviation(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new MeanDeviation(period, size) { Source = source };
        }
    }
}
