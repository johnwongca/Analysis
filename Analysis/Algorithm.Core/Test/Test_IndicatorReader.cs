using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core.Test
{
    public partial class Test
    {
        public static void Test_IndicatorReader()
        {
            Algorithm.Core.Test.TestIndicator01 indicator = new Algorithm.Core.Test.TestIndicator01(5);

            indicator.OpenData();
            IndicatorReader r = new IndicatorReader();
            r.Indicator = indicator;
            var t = r.GetSchemaTable();

            while (r.Read())
            {
                for (int i = 0; i < t.Rows.Count; i++)
                {
                    Console.WriteLine("{0} : {1}", r.GetName(i), r[i]);
                }
                Console.ReadKey();
            }
        }
    }
}
