using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    
    
    public partial class Program
    {
        static void test1(Type t)
        {
            OutputAttribute output = null;
            foreach(var p in t.GetProperties())
            {
                if(p.PropertyType.IsWindow())
                {
                    //p.CustomAttributes.FirstOrDefault(x => x. is OutputAttribute);
                }
            }
        }
        [Output(Name = "MainMethod")]
        static void Main(string[] args)
        {
            //Test.Test.Test_Indicator();
            test1(typeof(Indicator));

            #region Paulse and quit
            Console.WriteLine("Done");
            Console.ReadKey();
            #endregion
        }
    }

   
}
