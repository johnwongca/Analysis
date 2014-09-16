using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core.Test
{
    public partial class Test
    {
        public static void Test_IndicatorWriter()
        {
            Algorithm.Core.Test.TestIndicator01 indicator = new Algorithm.Core.Test.TestIndicator01(5);

            indicator.OpenData();
            IndicatorReader r = new IndicatorReader();
            r.Indicator = indicator;
            r.WriteToServer("abc");
        }
    }
}
