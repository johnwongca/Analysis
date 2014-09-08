using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public class Min : IndicatorBase
    {

        int mPeriod;

        public Min(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            mPeriod = period;
        }
        
        protected override void AfterSetValue(params Window<double>[] values)
        {
            double minValue = double.NaN;
            foreach (Window<double> w in values)
            {
                for (int i = 0; i < mPeriod; i++)
                {
                    if (!w.HasValue(i))
                        break;
                    if (double.IsNaN(minValue))
                    {
                        minValue = w[i];
                        continue;
                    }
                    if (w[i].CompareTo(minValue) < 0)
                        minValue = w[i];
                }
            }
            Value = minValue;
        }
    }
    public partial class IndicatorBase
    {
        public Min Min(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new Min(period, size) { Source = this.Source};
        }
        public static Min Create_Min(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new Min(period, size) { Source = source };
        }
    }
}
