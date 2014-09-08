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
        private static void Test_Min()
        {
            Window<double> w1 = new Window<double>(5);
            var min = Indicator.Create_Min(3, 3);
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                w1.Push(r.Next(20));
                min.Push(w1);
                Console.WriteLine("Window value: {0}, Min value: {1}", w1.Value, min.Value);
            }
            foreach (int value in min)
            {
                Console.WriteLine(value);
            }
            Min m1 = Indicator.Create_Min(1, 1);
            Console.WriteLine("---------------");
            Console.WriteLine(m1.Push(min.ToWindowArray()));
        }
    }
}
