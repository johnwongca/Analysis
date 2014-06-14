using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EODDataService
{
    class Program
    {
        static void Main(string[] args)
        {
            G.SetDatabaseConnection(args);
            EODDataConnection connection = new EODDataConnection();
            connection.Token = "016E65GDUEHH";
            var data = connection.GetCountries();
            Console.WriteLine(data.WriteToServer());
            Console.WriteLine(data.WriteToServer());
            Console.WriteLine(data.WriteToServer());
            Console.WriteLine(data.WriteToServer());
            Console.WriteLine(data.WriteToServer());
            /*foreach (var c in connection.GetCountries())
            {
                Console.WriteLine("{0}  {1}", c.Code, c.Name);
            }*/
            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
