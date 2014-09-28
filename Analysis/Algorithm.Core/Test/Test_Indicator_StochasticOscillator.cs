using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm.Core;

namespace Algorithm.Core.Test
{
    public class TestIndicator_StochasticOscillator: Indicator
    {
        public StochasticOscillator w;

        public TestIndicator_StochasticOscillator(int size = DefaultDataWindowSize)
            : base(size)
        {
            SymbolID = 170976;
            IntervalType = IntervalType.Days;
            Interval = 1;
            StartDate = new DateTime(2012, 7, 16);
            EndDate = new DateTime(2012, 9, 16);
            w = StochasticOscillator(14);
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            Value = w.Push(Close);
        }
    }
    public partial class Test
    {
        public static void Test_Indicator_StochasticOscillator()
        {
            TestIndicator_StochasticOscillator indicator = new TestIndicator_StochasticOscillator();
            indicator.OpenData();
            for (int i = 0; i < 2000; i++)
            {
                if (indicator.Read())
                {
                    Console.WriteLine("{0:yyyy-MM-dd hh:mm}  {1:0.00} {2:0.00} {3:0.00} {4:0.00} {5:0.00}",
                        indicator.DateFrom.Value, indicator.High.Value, indicator.Close.Value, indicator.Low.Value, indicator.w.Value, indicator.w.Average.Value);
                }
            }
            indicator.CloseData();
        }
    }
}
