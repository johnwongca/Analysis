using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm.Core;

namespace Algorithm.Core.Forms
{
    public class Fundamental
    {
        public string Exchange; 
        public string Symbol;
        public int SymbolID; 
        public string Sector; 
        public string Industry; 
        public double Dividend; 
        public DateTime DividendDate; 
        public double DividendYield; 
        public double DPS; 
        public double EBITDA; 
        public int MarketCap; 
        public double EPS; 
        public double PtS; 
        public double NTA; 
        public double PE; 
        public double PEG; 
        public double PtB; 
        public int Shares;
        public double Yield;
        static public DataTable Get(int symbolID)
        {
            using (SqlCommand command = Methods.GetConnection().CreateCommand())
            {
                command.CommandText = "q.GetFundamental";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SymbolID", symbolID);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader == null)
                        return null;
                    DataTable ret = new DataTable("Fundamental");
                    ret.Load(reader);
                    return ret;
                }
            }
        }
    }
}
