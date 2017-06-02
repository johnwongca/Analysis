using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

using Microsoft.SqlServer.Server;

namespace etl.sqlnotes.info
{
    public static class  Common
    {
        private static Regex InvalidParameterCharacters = new Regex(@"[^A-Z0-9_\s]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static string GetSQLName(this string str)
        {
            return InvalidParameterCharacters.Replace(str.TrimEnd(' '), "_").Replace(" ", "_");
        }
        public static short GetSessionID(this SqlConnection connection)
        {
            using(SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select @@spid";
                return (short)cmd.ExecuteScalar();
            }
        }
        public static Tuple<string, string> GetServerDatabaseName(this SqlConnection connection)
        {
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "select @@servername, db_name()";
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    return new Tuple<string,string>(reader[0].ToString(), reader[1].ToString());
                }
            }
        }
        [SqlFunction(FillRowMethodName = "ExecuteShellCommand_FillRow", TableDefinition="ID int, Type nvarchar(6), Result nvarchar(max)")]
        public static IEnumerable ExecuteShellCommand(string command)
        {

            List<Tuple<int, string, string>> ret = new List<Tuple<int, string, string>>();
            int i = 1;
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => {ret.Add(new Tuple<int, string, string>(i, "Output", e.Data)); i++;};
            process.BeginOutputReadLine();
            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => { ret.Add(new Tuple<int, string, string>(i, "Error", e.Data)); i++; };
            process.BeginErrorReadLine();
            process.WaitForExit();
            process.Close();
            return ret;
        }
        public static void ExecuteShellCommand_FillRow(object o, out SqlInt32 ID, out SqlString Type, out SqlString Result)
        {
            Tuple<int, string, string> x = (Tuple<int, string, string>)o;

            ID = new SqlInt32(x.Item1);
            Type = new SqlString(x.Item2);
            Result = new SqlString(x.Item3);
        }
    }
}
