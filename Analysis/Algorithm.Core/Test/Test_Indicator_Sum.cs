using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm.Core;

namespace Algorithm.Core.Test
{
    public class TestIndicator_Sum : Indicator
    {
        Sum sum;
        //Max<int> max;
        public TestIndicator_Sum(int size = DefaultDataWindowSize)
            : base(size)
        {
            SymbolID = 170976;
            IntervalType = IntervalType.Days;
            Interval = 3;
            StartDate = new DateTime(2010, 7, 16);
            EndDate = new DateTime(2010, 8, 16);
            sum = Sum(3);
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            Value = sum.Push(Close);
        }
    }
    public partial class Test
    {
        public static void Test_Indicator_Sum()
        {
            TestIndicator_Sum indicator = new TestIndicator_Sum(5);

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
