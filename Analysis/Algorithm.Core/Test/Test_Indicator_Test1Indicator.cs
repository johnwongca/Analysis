using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm.Core;

namespace Algorithm.Core.Test
{
    
    public partial class Test
    {
        public static void Test_Indicator_Test1Indicator()
        {

            using (Test1Indicator indicator = new Test1Indicator())
            {
                indicator.SetDefaultValues();
                indicator.StartDate = new DateTime(2014, 10, 15);
                indicator.SymbolID = 170976;
                indicator.Interval = 1;
                indicator.IntervalType = IntervalType.Days;

                indicator.WriteToServer();
                /*
                indicator.OpenData();
                for (int i = 0; i < 2000; i++)
                {
                    if (indicator.Read())
                    {
                        Console.WriteLine("{0:yyyy-MM-dd hh:mm} {1:0.00} {2:0.00} {3:0.00} ",
                            indicator.DateTo.Value, indicator.Close.Value, indicator.BollingerBandsUpper.Value, indicator.BollingerBandsLower.Value);
                    }
                }
                indicator.CloseData();
                 */
            }
        }
    }
}
