using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EODDataService.EODDataSvc;

namespace EODDataService
{
    public static partial class G
    {
        public static string ConnectionString;
        public static string SetDatabaseConnection(params string[] args)
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.DataSource = args.Length == 0 ? "." : args[0];
            sb.InitialCatalog = args.Length == 0 ? "QuoteStaging" : args[1];
            sb.IntegratedSecurity = true;
            sb.Pooling = true;
            sb.ApplicationName = "EODDataService.sqlnotes.info";
            ConnectionString = sb.ToString();
            return ConnectionString;
        }
        public static string QuoteDate(this DateTime date)
        {
            return string.Format("{0:yyyyMMdd}", date);
        }
        public static string StringPeriod(this EODDataInterval period)
        {
            if (period == EODDataInterval.OneMinute) return "1";
            if (period == EODDataInterval.FiveMinute) return "5";
            if (period == EODDataInterval.TenMinute) return "10";
            if (period == EODDataInterval.FifteenMinute) return "15";
            if (period == EODDataInterval.ThirtyMinute) return "30";
            if (period == EODDataInterval.OneHour) return "h";
            if (period == EODDataInterval.Day) return "d";
            if (period == EODDataInterval.Week) return "w";
            return "m";
        }
        public static double WriteToServer(this CountryBase[] data)
        {
            if (data.Length == 0)
                return 0d;
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                DateTime now = DateTime.Now;
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "EODData.SessionCountryAdd";
                command.Parameters.Add("@CountryCode", SqlDbType.VarChar, 5);
                command.Parameters.Add("@CountryName", SqlDbType.VarChar, 128);
                foreach(var item in data)
                {
                    command.Parameters[0].Value = item.Code.ToUpper().Trim();
                    command.Parameters[1].Value = item.Name.Trim();
                    command.ExecuteNonQuery();
                }
                return (DateTime.Now - now).TotalMilliseconds / data.Length;
            }
        }
    }
}
