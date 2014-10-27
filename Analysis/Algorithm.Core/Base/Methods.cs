using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Algorithm.Core
{
    public static partial class Methods
    {
        public static List<string> Cursors = new List<string>();
        private static DateTime MinDateTime = new DateTime(2010, 1, 1);
        private static Regex InvalidParameterCharacters = new Regex(@"[^A-Z0-9_\s]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
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
            command.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dateFrom<MinDateTime ? MinDateTime: dateFrom;
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
            return t.IsSubclassOf(typeof(Window<double>)) || t.IsSubclassOf(typeof(Window<DateTime>)) || t.IsSubclassOf(typeof(Window<int>))
                || t == typeof(Window<double>) || t == typeof(Window<DateTime>) || t == typeof(Window<int>);
        }
        public static string GetSQLName(this string str)
        {
            return InvalidParameterCharacters.Replace(str.TrimEnd(' '), "_").Replace(" ", "_");
        }
        public static void WriteToServer(this IndicatorReader reader, string tableName, string databaseName = "tempdb")
        {
            IndicatorWriter writer = new IndicatorWriter() { TableName = tableName, Reader = reader, DatabaseName = databaseName };
            writer.WriteToServer();
        }
        public static double Gain(this Window<double> data, int periodback = 1)
        {
            if (data.HasValue(periodback) && data[0] > data[periodback])
                return data[0] - data[periodback];
            return 0d;
        }
        public static double Loss(this Window<double> data, int periodback = 1)
        {
            if (data.HasValue(periodback) && data[0] < data[periodback])
                return data[periodback] - data[0];
            return 0d;
        }
        public static bool CrossAbove(this Window<double> current, Window<double> compareTo, int period = 1)
        {
            if(period<1)
                return false;
            if (!current.HasValue(period))
                return false;
            if (!compareTo.HasValue(period))
                return false;
            return (current[period] <= compareTo[period]) && (current[0] > compareTo[0]);
        }
        public static bool CrossBelow(this Window<double> current, Window<double> compareTo, int period = 1)
        {
            if (period < 1)
                return false;
            if (!current.HasValue(period))
                return false;
            if (!compareTo.HasValue(period))
                return false;
            return (current[period] >= compareTo[period]) && (current[0] < compareTo[0]);
        }
        public static double RelativeStrength(this double input1, double input2)
        {
            return input2 == 0 ? 100d : (100d / (1d + (input1 / input2)));
        }
        public static double RelativeStrength(this Window<double> input1, double input2)
        {
            return input1.Value.RelativeStrength(input2);
        }
        public static double RelativeStrength(this Window<double> input1, Window<double> input2)
        {
            return input1.Value.RelativeStrength(input2.Value);
        }
        public static long GetToken()
        {
            using(SqlConnection connection = GetConnection())
            {
                using(SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "q.GetToken";
                    command.CommandType = CommandType.StoredProcedure;
                    return Convert.ToInt64(command.ExecuteScalar());
                }
            }
        }
        public static Dictionary<string, InputAttribute> GetInputAttributes(this Type t)
        {
            Dictionary<string, InputAttribute>  ret = new Dictionary<string, InputAttribute>();
            InputAttribute i;
            foreach (var p in t.GetProperties())
            {
                i = (InputAttribute)(p.GetCustomAttributes(true).FirstOrDefault(x => x is InputAttribute));
                if (i != null)
                {
                    ret.Add(p.Name, i);
                }
            }
            return ret;
        }
        public static string GetCursorName(int symbolID, IntervalType intervalType, int interval, DateTime startDate)
        {
            return string.Format("C_{0}_{1}_{2}_{3}{4:00}{5:00}", symbolID, intervalType, interval, startDate.Year, startDate.Month, startDate.Day);
        }
        public static int CursorFetch(this string CursorName, DataTable table, int startLocation = 0, int rows = 50)
        {
            using(SqlConnection connection = GetConnection())
            {
                using(SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "q.CursorFetch";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CursorName", SqlDbType.VarChar, 128).Value = CursorName;
                    cmd.Parameters.Add("@StartLocation", SqlDbType.Int).Value = startLocation;
                    cmd.Parameters.Add("@NumberOfRows", SqlDbType.Int).Value = rows;
                    var p = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    p.Direction = ParameterDirection.ReturnValue;
                    using(SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader == null)
                            return -1;
                        table.Load(reader);
                        return Convert.ToInt32(p.Value);
                    }
                }
            }
        }
        public static int CursorSize(this string CursorName)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "q.CursorSize";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CursorName", SqlDbType.VarChar, 128).Value = CursorName;
                    var p = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    p.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();
                    return Convert.ToInt32(p.Value);

                }
            }
        }
        public static void CursorRemove(this string CursorName)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "q.CursorRemove";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CursorName", SqlDbType.VarChar, 128).Value = CursorName;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void CursorRemoveAll()
        {
            foreach(string s in Cursors)
            {
                try
                {
                    CursorRemove(s);
                }
                catch { }
            }
        }
        public static XElement GetDefinition(this Chart chart)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chart.Serializer.Format = System.Windows.Forms.DataVisualization.Charting.SerializationFormat.Xml;
                chart.Serializer.NonSerializableContent = "*.Points";
                chart.Serializer.Save(ms);
                return XElement.Load(ms);
            }
        }
        public static void SetDefinition(this Chart chart, XElement definition)
        {
            if (definition == null)
                return;
            using (MemoryStream ms = new MemoryStream())
            {
                definition.Save(ms);
                ms.Position = 0;
                chart.Serializer.Load(ms);
            }
        }
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

    }
}
