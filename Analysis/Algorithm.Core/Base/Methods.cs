using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public static partial class Methods
    {
        public static SqlConnection GetConnection()
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.DataSource = "SQL2";
            sb.InitialCatalog = "quote";
            sb.IntegratedSecurity = true;
            sb.Pooling = true;
            sb.ApplicationName = "Analysis Application";
            SqlConnection connection = new SqlConnection(sb.ToString());
            connection.Open();
            return connection;
        }
        public static SqlDataReader RetrieveQuote(int symbolId, bool isEndOfDate, DateTime dateFrom = default(DateTime), DateTime dateTo = default(DateTime))
        {
            SqlConnection connection = GetConnection();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "q.RetrieveQuote";
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;
            command.Parameters.Add("@SymbolID", SqlDbType.Int).Value = symbolId;
            command.Parameters.Add("@IsEOD", SqlDbType.Bit).Value = isEndOfDate;
            command.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dateFrom;
            command.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dateTo == default(DateTime)? DateTime.MaxValue: dateTo;
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public static double[] FillWithNaN(this double[] arg)
        {
            if (arg == null)
                return null;
            for (int i = 0; i < arg.Length; i++)
                arg[i] = double.NaN;
            return arg;
        }
        public static double ToDouble(this int i)
        {
            return Convert.ToDouble(i);
        }
        public static DateTime ToDateTime(this double date)
        {
            return (new DateTime(1900, 1, 1)).AddMinutes(date * 1440);
        }

        public static Window<T>[] ToWindowArray<T>(this IEnumerable<T> buffer) where T : IComparable<T>, IEquatable<T>, IConvertible
        {
            return buffer.Select(x => Window<T>.NewVariable(x)).ToArray();
        }
        public static bool IsWindow(this Type t)
        {
            return t == typeof(Window<double>) || t == typeof(Window<DateTime>) || t == typeof(Window<int>);
        }
    }
}
