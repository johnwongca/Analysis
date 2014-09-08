using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm.Core;
namespace Test
{
    public partial class Program
    {
        private static void Test_Source()
        {
            Source source = new Source(170976, IntervalType.Days, 3);
            source.StartDate = new DateTime(2014, 7, 17);
            source.EndDate = new DateTime(2014, 9, 16);
            source.OpenData();
            for (int i = 0; i < 2000; i++)
            {
                if (source.Read())
                {
                    Console.WriteLine("{0:yyyy-MM-dd hh:mm} {1:yyyy-MM-dd hh:mm} {2:0.00} {3:0.00} {4:0.00} {5:0.00} {6} {7}",
                        source.DateFrom.Value, source.DateTo.Value, source.Open.Value, source.High.Value, source.Low.Value, source.Close.Value, source.Volume.Value, source.ItemCount.Value);
                }
            }
            source.CloseData();
        }
    }
}
