using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Algorithm.Core;
namespace Test
{
    public class Base0
    {

    }
    public class Base1:Base0
    {
        [Description("Property1")]
        public int P1 { get; set; }
        public int P2 { get; set; }
    }
    public class Base2 : Base1
    {
        [Description("Property2")]
        public int P3 { get; set; }
        public int P4 { get; set; }
    }
    class Program
    {
      
       
        static void Main(string[] args)
        {
            Base0 o = new Base2();
            foreach(var p in o.GetType().GetProperties())
            {
                foreach (var p1 in p.GetCustomAttributes(false))
                {
                    
                    Console.WriteLine(p.Name);
                    if (p1 is DescriptionAttribute)
                    {
                        Console.WriteLine(((DescriptionAttribute)p1).Description);
                        
                    }
                    Console.WriteLine("-------");
                }
            }
            Console.WriteLine("Done");
            Console.ReadKey();
        }
        
    }
}
