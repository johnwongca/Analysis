using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EODDataService.EODDataSvc;

namespace EODDataService
{
    public static partial class G
    {
        
        static EODDataConnection mEODDataConnection = new EODDataConnection();
        public static string ConnectionString;
        public static int MaxConnection = 0;
        public static int MaxIdleTimeInMinute = 300;
        public static DateTime LastTaskExecutionTime = DateTime.Now;
        static DateTime LastSettingsRead = DateTime.MinValue;
        public static void RemoveSessionStatus()
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "[EODData].[RemoveSessionStatus]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        static void ReadSettings()
        {
            lock (mEODDataConnection)
            {
                if ((DateTime.Now - LastSettingsRead).TotalMinutes < 1)
                    return;
                LastSettingsRead = DateTime.Now;
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "EODData.ReadSettings";
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {

                            if (reader.GetString(0).ToUpper() == "MAXIDLETIMEINMINUTE")
                                MaxIdleTimeInMinute = Convert.ToInt32(reader.GetString(1));
                            if (reader.GetString(0).ToUpper() == "MAXCONCURRENTEXECUTION")
                                MaxConnection = Convert.ToInt32(reader.GetString(1));
                            if (reader.GetString(0).ToUpper() == "TOKEN")
                            {
                                mEODDataConnection.Token = reader.GetString(1);
                            }
                        }
                    }
                }
            }
        }
        public static EODDataConnection EODDataConnection
        {
            get 
            {
                ReadSettings();
                return mEODDataConnection;
            }
        }
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
        static EODDataReader GetReaderStructure(SqlConnection connection, string tableName, object[][] data = null, DownloadTask dt = null)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            if (dt != null)
            {
                command.CommandText = "select @@spid";
                dt.BulkCopySessionID = (short)command.ExecuteScalar();
                dt.Rows = data.Length;
            }
            command.CommandText = "select top 0 * from " + tableName;
            using (var reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                return new EODDataReader(reader.GetSchemaTable(), data);
        }
        static void WriteToServer(EODDataReader eReader, SqlConnection connection, string destinationTableName)
        {
            using (SqlBulkCopy b = new SqlBulkCopy(connection, SqlBulkCopyOptions.FireTriggers, null))
            {
                b.BatchSize = 3000;
                b.BulkCopyTimeout = 30;
                b.DestinationTableName = destinationTableName;
                b.EnableStreaming = true;
                b.WriteToServer(eReader);
            }
        }
        static double WriteToServer(string tableName, object[][] data, DownloadTask dt = null)
        {
            if (data.Length == 0)
                return 0d;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                DateTime now = DateTime.Now;
                using (EODDataReader eReader = GetReaderStructure(connection, tableName, data, dt))
                {
                    WriteToServer(eReader, connection, tableName);
                }
                return (DateTime.Now - now).TotalMilliseconds / data.Length;
            }
        }
        public static double WriteToServer(this CountryBase[] data, DownloadTask dt = null)
        {
            if (data == null)
                return 0;
            return WriteToServer("EODData.vCountry", data.Select(x => new string[2] { x.Code, x.Name }).ToArray(), dt);
        }
        public static double WriteToServer(this EXCHANGE[] data, DownloadTask dt = null)
        {
            if (data == null)
                return 0;
            return WriteToServer("EODData.vExchange", 
                data.Select(x => new object[] 
                    { 
                        x.Code, x.Name, x.Country, 
		                x.Currency, x.Declines, x.HasIntradayProduct, 
		                x.IntradayStartDate, x.IsIntraday, x.LastTradeDateTime, 
		                x.Suffix, x.TimeZone 
                    }
                ).ToArray(), dt);
        }
        public static double WriteToServer(this FUNDAMENTAL[] data, string exchange, DownloadTask dt = null)
        {
            if (data == null)
                return 0;
            return WriteToServer("EODData.vFundamental",
                data.Select(x => new object[] 
                    { 
                        exchange, x.Symbol, x.DateTime, 
                        x.Name, x.Description, x.Dividend, 
                        x.DividendDate, x.DivYield, x.DPS, 
                        x.EBITDA, x.EPS, x.Industry, 
                        x.MarketCap, x.NTA, x.PE, 
                        x.PEG, x.PtB, x.PtS, 
                        x.Sector, x.Shares, x.Yield
                    }
                ).ToArray(), dt);
        }
        public static double WriteToServer(this QUOTE[] data, string exchange, EODDataInterval interval, DownloadTask dt = null)
        {
            if (data == null)
                return 0;
            return WriteToServer("EODData.vQuote",
                data.Select(x => new object[] 
                    { 
                        exchange, x.Symbol, Convert.ToInt16(interval), x.DateTime, 
                        x.Open, x.Close, x.High, 
                        x.Low, x.Volume, x.Ask, 
                        x.Bid, x.OpenInterest
                    }
                ).ToArray(), dt);
        }
        public static double WriteToServer(this SPLIT[] data, string exchange, DownloadTask dt = null)
        {
            if (data == null)
                return 0;
            return WriteToServer("EODData.vSplit",
                data.Select(x => new object[] 
                    { 
                        exchange, x.Symbol, x.DateTime, 
                        x.Ratio
                    }
                ).ToArray(), dt);
        }
        public static double WriteToServer(this SYMBOL[] data, string exchange, DownloadTask dt = null)
        {
            if (data == null)
                return 0;
            return WriteToServer("EODData.vSymbol",
                data.Select(x => new object[] 
                    { 
                        exchange, x.Code, x.Name, 
                        x.LongName, x.DateTime
                    }
                ).ToArray(), dt);
        }
        public static double WriteToServer(this SYMBOLCHANGE[] data, string exchange, DownloadTask dt = null)
        {
            if (data == null)
                return 0;
            return WriteToServer("EODData.vSymbolChange",
                data.Select(x => new object[] 
                    { 
                        x.DateTime, x.ExchangeCode, x.OldSymbol,
                        x.NewExchangeCode, x.NewSymbol
                    }
                ).ToArray(), dt);
        }
    }
}
