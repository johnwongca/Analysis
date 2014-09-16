using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public class Max : Indicator
    {
        public int Period { get; set; }
        public Max(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            Period = period;
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            double maxValue = double.NaN;
            foreach (Window<double> w in values)
            {
                for (int i = 0; i < Period; i++)
                {
                    if (!w.HasValue(i))
                        break;
                    if (double.IsNaN(maxValue))
                    {
                        maxValue = w[i];
                        continue;
                    }
                    if (w[i].CompareTo(maxValue) > 0)
                        maxValue = w[i];
                }
            }
            Value = maxValue;
        }
    }

    public partial class IndicatorBase
    {
        public Max Max(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new Max(period, size) { Source = this.Source };
        }
        public static Max Create_Max(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new Max(period, size) { Source = source };
        }
    }
}
