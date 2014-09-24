using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm.Core;

namespace Algorithm.Core.Test
{
    public class TestIndicator_BollingerBands: Indicator
    {
        public BollingerBands w;
        public ExponentialMovingAverage e;
        public TestIndicator_BollingerBands(int size = DefaultDataWindowSize)
            : base(size)
        {
            SymbolID = 170976;
            IntervalType = IntervalType.Days;
            Interval = 1;
            StartDate = new DateTime(2012, 7, 16);
            EndDate = new DateTime(2012, 9, 16);
            w = BollingerBands(5);
            e = ExponentialMovingAverage(1);
            w.Average = e;
            w.AverageForStandardDeviation = e;
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            e.Push(Close);
            Value = w.Push(Close);
        }
    }
    public partial class Test
    {
        public static void Test_Indicator_BollingerBands()
        {
            TestIndicator_BollingerBands indicator = new TestIndicator_BollingerBands();
            Console.WriteLine("Close Average Lower Upper Std");
            indicator.OpenData();
            for (int i = 0; i < 2000; i++)
            {
                if (indicator.Read())
                {
                    Console.WriteLine("{0:yyyy-MM-dd hh:mm}  {1:0.00} {2:0.00} {3:0.00} {4:0.00} {5:0.00}",
                        indicator.DateFrom.Value, indicator.Close.Value, indicator.w.Average.Value, indicator.w.Lower.Value, indicator.w.Upper.Value, indicator.w.sd.Value);
                }
            }
            indicator.CloseData();
        }
    }
}
