using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm.Core;

namespace Algorithm.Core.Test
{
    public class TestIndicator_RelativeStrengthIndex: Indicator
    {
        public RelativeStrengthIndex w;

        public TestIndicator_RelativeStrengthIndex(int size = DefaultDataWindowSize)
            : base(size)
        {
            SymbolID = 170976;
            IntervalType = IntervalType.Days;
            Interval = 1;
            StartDate = new DateTime(2012, 7, 16);
            EndDate = new DateTime(2012, 9, 16);
            w = RelativeStrengthIndex(14);
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            Value = w.Push(Close);
        }
    }
    public partial class Test
    {
        public static void Test_Indicator_RelativeStrengthIndex()
        {
            TestIndicator_RelativeStrengthIndex indicator = new TestIndicator_RelativeStrengthIndex();
            indicator.OpenData();
            for (int i = 0; i < 2000; i++)
            {
                if (indicator.Read())
                {
                    Console.WriteLine("{0:yyyy-MM-dd hh:mm}  {1:0.00} {2:0.00}",
                        indicator.DateFrom.Value, indicator.Close.Value, indicator.w.Value);
                }
            }
            indicator.CloseData();
        }
    }
}
