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
        static void Test_Window()
        {
            Window<int> a = new Window<int>(7);
            for(int i=0; i<10; i++)
            {
                a.Push(i);
                Console.WriteLine("Values in window: {0}", a.ValuesInWindow);
            }
            Console.WriteLine(a[1]);
            Console.WriteLine("-----------");
            foreach (double v in a)
            {
                Console.WriteLine(v);
            }
            Console.WriteLine("-----------");
            for(int i=0; i< 12; i++)
            {
                Console.WriteLine("{0}:{1}, HasValue:{2}",i,a[i], a.HasValue(i));
            }
            Console.WriteLine("-----------");
            foreach(var v in a.SubSet(2,5))
            {
                Console.WriteLine(v);
            }
        }
    }
}
