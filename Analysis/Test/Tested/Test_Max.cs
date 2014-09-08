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
        private static void Test_Max()
        {
            Window<double> w1 = new Window<double>(5);
            var Max = Indicator.Create_Max(3, 3);
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                w1.Push(r.Next(20));
                Max.Push(w1);
                Console.WriteLine("Window value: {0}, Max value: {1}", w1.Value, Max.Value);
            }
            foreach (int value in Max)
            {
                Console.WriteLine(value);
            }
        }
    }
}
