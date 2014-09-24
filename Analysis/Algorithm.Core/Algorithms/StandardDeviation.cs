using System;
using System.Collections.Generic;


namespace Algorithm.Core
{
    public class StandardDeviation:Indicator
    {

        public Window<double> Average { get; set; }
        public bool IsPopulation { get; set; }
        bool isAverageInternal = true;
        public int Period { get; set; }
        
        public StandardDeviation(int period, Window<double> average = null, int size = Window<double>.DefaultDataWindowSize)
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
            IsPopulation = false;
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            if (Average == null)
                Average = SimpleMovingAverage(Period, 1);
            if(isAverageInternal)
            {
                Average.Push(values);
            }
            double n = 0, v = 0;
            for(int i=0; i<Period; i++)
            {
                if (!values[0].HasValue(i))
                    break;
                n++;
                v = v + Math.Pow(values[0][i] - Average.Value, 2);
            }
            Value = Math.Sqrt(v / Math.Max(IsPopulation ? n : n - 1, 1));
        }
    }
    public partial class IndicatorBase
    {
        public StandardDeviation StandardDeviation(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new StandardDeviation(period, null, size) { Source = this.Source };
        }
        public StandardDeviation StandardDeviation(int period, Window<double> average ,int size = Window<double>.DefaultDataWindowSize)
        {
            return new StandardDeviation(period, average, size) { Source = this.Source };
        }
        public static StandardDeviation Create_StandardDeviation(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new StandardDeviation(period, size) { Source = source };
        }
    }
}
