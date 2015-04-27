using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm.Core;

namespace Algorithm.Core.Forms
{
    public class WatchList
    {
        static public  DataTable Get()
        {
            using(SqlCommand command = Methods.GetConnection().CreateCommand())
            {
                command.CommandText = "q.GetWatchList";
                command.CommandType = CommandType.StoredProcedure;
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable ret = new DataTable("WatchList");
                    ret.Load(reader);
                    return ret;
                }
            }
        }
        static public void Remove(int symbolID)
        {
            using (SqlCommand command = Methods.GetConnection().CreateCommand())
            {
                command.CommandText = "q.RemoveFromWatchList";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SymbolID", symbolID);
                command.ExecuteNonQuery();
            }
        }
        static public void Add(int symbolID)
        {
            using (SqlCommand command = Methods.GetConnection().CreateCommand())
            {
                command.CommandText = "q.AddToWatchList";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SymbolID", symbolID);
                command.ExecuteNonQuery();
            }
        }
    }
}
