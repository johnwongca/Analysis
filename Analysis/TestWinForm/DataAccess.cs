using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinForm
{
    public class Quote
    {
        public double Open = 0, Close = 0, High = 0, Low = 0;
        public DateTime Date;
        public long Volume;
    }
    public static class DataAccess
    {
        public static List<Quote> GetData()
        {
            List<Quote> ret = new List<Quote>();
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "Quote";
            sb.DataSource = "SQL2";
            sb.IntegratedSecurity = true;
            sb.Pooling = true;
            using(SqlConnection connection = new SqlConnection(sb.ToString()))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from q.Quote where SymbolID = 186943";
                cmd.CommandType = CommandType.Text;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        ret.Add(new Quote()
                            {
                                Date = reader.GetDateTime(1),
                                Open = reader.GetFloat(2),
                                High = reader.GetFloat(3),
                                Low = reader.GetFloat(4),
                                Close = reader.GetFloat(5),
                                Volume = reader.GetInt64(6)
                            });
                    }
                }
            }
            return ret;
        }
    }
}
