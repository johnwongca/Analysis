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
            Console.WriteLine("Country: {0}",data.WriteToServer());
            var exchange = connection.GetExchanges();
            Console.WriteLine("Exchange: {0}", exchange.WriteToServer());
            var fundamental = connection.GetFundamentals("NASDAQ");
            Console.WriteLine("Fundamental:{0}", fundamental.WriteToServer("NASDAQ"));
            
            var q = connection.GetQuotes("NASDAQ", new DateTime(2014, 6, 16), EODDataInterval.Day);
            Console.WriteLine("Quote: {0}", q.WriteToServer("NASDAQ", EODDataInterval.Day));

            q = connection.GetQuotes("NASDAQ", new DateTime(2014, 6, 16), EODDataInterval.OneMinute);
            Console.WriteLine("Quote: {0}", q.WriteToServer("NASDAQ", EODDataInterval.OneMinute));

            var split = connection.GetSplits("NASDAQ");
            Console.WriteLine("Split: {0}", split.WriteToServer("NASDAQ"));

            var symbol = connection.GetSymbols("NASDAQ");
            Console.WriteLine("Symbol: {0}", symbol.WriteToServer("NASDAQ"));

            var symbolChange = connection.GetSymbolChanges("NASDAQ");
            Console.WriteLine("symbolChange: {0}", symbolChange.WriteToServer("NASDAQ"));
            
            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
