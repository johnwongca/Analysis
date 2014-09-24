using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm.Core;

namespace Algorithm.Core.Test
{
    public class TestIndicator_Aroon : Indicator
    {
        public Aroon w;

        public TestIndicator_Aroon(int size = DefaultDataWindowSize)
            : base(size)
        {
            SymbolID = 170976;
            IntervalType = IntervalType.Days;
            Interval = 1;
            StartDate = new DateTime(2012, 7, 16);
            EndDate = new DateTime(2012, 9, 16);
            w = Aroon(5);
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            Value = w.Push();
        }
    }
    public partial class Test
    {
        public static void Test_Indicator_Aroon()
        {
            TestIndicator_Aroon indicator = new TestIndicator_Aroon(5);

            indicator.OpenData();
            for (int i = 0; i < 2000; i++)
            {
                if (indicator.Read())
                {
                    Console.WriteLine("{0:yyyy-MM-dd hh:mm} {1:yyyy-MM-dd hh:mm} {2:0.00} {3:0.00} {4:0.00} {5:0.00}",
                        indicator.DateFrom.Value, indicator.DateTo.Value, indicator.High.Value, indicator.Low.Value, indicator.w.Up.Value, indicator.w.Down.Value);
                }
            }
            indicator.CloseData();
        }
    }
}
