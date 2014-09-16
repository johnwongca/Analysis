using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm.Core;

namespace Algorithm.Core.Test
{
    public class TestIndicator01 : Indicator
    {
        Min min;
        //Max<int> max;
        public TestIndicator01(int size = DefaultDataWindowSize)
            : base(size)
        {
            SymbolID = 170976;
            IntervalType = IntervalType.Days;
            Interval = 3;
            StartDate = new DateTime(2010, 7, 16);
            EndDate = new DateTime(2015, 8, 16);
            min = Min(3, 1);
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            Value = min.Push(Open, High, Low, Close);
        }
    }
    public partial class Test
    {
        public static void Test_Indicator()
        {
            TestIndicator01 indicator = new TestIndicator01(5);

            indicator.OpenData();
            for (int i = 0; i < 2000; i++)
            {
                if (indicator.Read())
                {
                    Console.WriteLine("{0:yyyy-MM-dd hh:mm} {1:yyyy-MM-dd hh:mm} {2:0.00} {3:0.00} {4:0.00} {5:0.00} {6} {7} {8:0.00}",
                        indicator.DateFrom.Value, indicator.DateTo.Value, indicator.Open.Value, indicator.High.Value, indicator.Low.Value, indicator.Close.Value, indicator.Volume.Value, indicator.ItemCount.Value, indicator.Value);
                }
            }
            indicator.CloseData();
        }
    }
}
