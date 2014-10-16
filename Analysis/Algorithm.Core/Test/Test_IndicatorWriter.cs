using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core.Test
{
    public partial class Test
    {
        public static void Test_IndicatorWriter()
        {

            //Algorithm.Core.Test.TestIndicator01 indicator = new Algorithm.Core.Test.TestIndicator01(5);

            //indicator.OpenData();
            //IndicatorReader r = new IndicatorReader();
            //r.Indicator = indicator;
            //r.WriteToServer("abc");

            DataTable d1 = new DataTable();
            (new Algorithm.Core.Test.TestIndicator01(5)).WriteToDataTable(d1);
            (new Algorithm.Core.Test.TestIndicator01(5)).WriteToDataTable(d1);
            
        }
    }
}
