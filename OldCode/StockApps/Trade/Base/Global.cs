using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Trade.Base;

namespace Trade
{
    static partial class Program
    {
        public static string ConnectionString
        {
            get { return Properties.Settings.Default.ConnectString; }
        }
        public static SqlConnection GetConnection()
        {
            SqlConnection ret = new SqlConnection(Program.ConnectionString);
            ret.Open();
            return ret;
        }
        public static SqlConnection GetConnection(this object o)
        {
            return GetConnection();
        }
        public static SqlConnection GetConnection(this object o, string connectionString)
        {
            SqlConnection ret = new SqlConnection(connectionString);
            ret.Open();
            return ret;
        }
        public static SqlConnection GetConnection(this object o, string server, string databaseName)
        {
            SqlConnection ret = new SqlConnection("Data Source=" + server + ";Initial Catalog=" + databaseName + ";Integrated Security=True;Pooling=True;MultipleActiveResultSets=True");
            ret.Open();
            return ret;
        }
        public static SqlCommand GetCommand()
        {
            SqlCommand r = GetConnection().CreateCommand();
            r.CommandTimeout = 120;
            return r;
        }
        public static SqlCommand GetCommand(this object o)
        {
            return GetCommand();
        }
        public static SqlCommand GetCommand(this object o, string connectionString)
        {
            SqlCommand r = o.GetConnection(connectionString).CreateCommand();
            r.CommandTimeout = 120;
            return r;
        }
        public static SqlCommand GetCommand(this object o, string server, string databaseName)
        {
            SqlCommand r = o.GetConnection(server, databaseName).CreateCommand();
            r.CommandTimeout = 120;
            return r;
        }
        public static DataTable GetDataSet(string SQL)
        {
            DataTable ret = new DataTable();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(SQL, Program.ConnectionString);
            da.SelectCommand.CommandTimeout = 120;
            da.Fill(ret);
            return ret;
        }
        public static DataTable GetDataSet(this object o, string SQL)
        {

            return GetDataSet(SQL);
        }
        public static void RunSQL(string SQL)
        {
            SqlCommand c = GetCommand();
            c.CommandText = SQL;
            c.ExecuteNonQuery();
        }
        public static void RunSQL(this object o, string SQL)
        {
            RunSQL(SQL);
        }
        public static Type[] SupportedTypes = new Type[] { 
            typeof(TextBox), typeof(CheckBox),typeof(DataGridView), 
            typeof(DateTimePicker), typeof(ListBox), typeof(ListView), 
            typeof(MaskedTextBox), typeof(NumericUpDown), typeof(RadioButton),
            typeof(ListBox)};
        public static bool IsSupportedControl(this Control o)
        {
            return SupportedTypes.FirstOrDefault(x => { return x == o.GetType(); })!=null;
        }
        public static bool IsAdapter(this Component adapter)
        {
            if (adapter == null)
                return false;
            if (adapter.GetType().GetMember("GetData").ToList().Count > 0 && adapter.GetType().GetMember("Fill").ToList().Count > 0)
                return true;
            return false;
        }
        public static BindingSourceAdapter GetBindingSource(this Control o)
        {
            if (o is DataGridView)
            {
                if (((DataGridView)o).DataSource is BindingSourceAdapter)
                    return (BindingSourceAdapter)((DataGridView)o).DataSource;
                return null;
            }
            if (o is DbTree)
            {
                if (((DbTree)o).DataSource is BindingSourceAdapter)
                    return (BindingSourceAdapter)((DbTree)o).DataSource;
                return null;
            }
            if(o.DataBindings.Count == 0) return null;
            if (o.DataBindings[0].DataSource is BindingSourceAdapter)
                return (BindingSourceAdapter)(o.DataBindings[0]).DataSource;
            return null;
        }
        public static System.Data.SqlClient.SqlDataAdapter GetAdapter(this Component adapter)
        {

            Object ret = null;
            if (!adapter.IsAdapter())
                return null;
            var ad = adapter.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(x => { return x.Name == "_adapter" && x.MemberType == MemberTypes.Field; }).ToList();
            if (ad.Count != 1)
                return null;
            ret = adapter.GetType().InvokeMember(ad[0].Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, adapter, null);
            if (ret is System.Data.SqlClient.SqlDataAdapter)
                return (System.Data.SqlClient.SqlDataAdapter)ret;
            return null;

        }
        public static Type GetAdapterReturnType(this Component adapter)
        {
            if (adapter == null)
                return null;
            if (!Program.IsAdapter(adapter))
                return null;
            return adapter.GetType().GetMethods().First(x => { return x.Name == "GetData"; }).ReturnType;
        }
        public static FormMain mainForm = null;
        public static FormMain GetMainForm(this Form o)
        {
            return Program.mainForm;
        }
        public static void MainFormLabelShow(string text)
        {
            mainForm.lbOpeningStatus.Text = text;
            mainForm.lbOpeningStatus.Visible = true;
            try { System.Windows.Forms.Application.DoEvents(); System.Windows.Forms.Application.DoEvents(); }
            catch { };
        }
        public static void MainFormLabelHide()
        {
            mainForm.lbOpeningStatus.Visible = false;
            try { System.Windows.Forms.Application.DoEvents(); System.Windows.Forms.Application.DoEvents(); }
            catch { };
        }
        public static DataTable ArrayToDataTable(double[] data)
        {
            DataTable ret = new DataTable();
            ret.Columns.Add("Value", typeof(double));
            foreach (double d in data)
                ret.Rows.Add(d);
            return ret;
        }
        public static DataTable ArrayToDataTable(double[][] p)
        {
            DataTable ret = new DataTable();
            if (p.Length == 0)
                return ret;
            for (int i = 0; i < p[0].Length; i++)
                ret.Columns.Add("Value" + i.ToString(), typeof(double));
            for (int i = 0; i < p.Length; i++)
            {
                DataRow r = ret.NewRow();
                for (int j = 0; j < p[i].Length; j++)
                    r[j] = p[i][j];
                ret.Rows.Add(r);
            }
            return ret;
        }
        public static string ToSQLString(this String o)
        {
            return "'" + o.Replace("'", "''") + "'";
        }
        public static string ToODBCStringShort(this DateTime o)
        {
            return o.ToString("yyyy-MM-dd");
        }
        public static List<object> ToListOfObject(this List<double> o)
        {
            return o.Select(x => { return (object)x; }).ToList();
        }
        public static string FormatString(this string s, params object[] o)
        {
            return String.Format(s, o);
        }
        public static Interface.Transaction.FormTransaction fromTransactions = null;
        internal static Trade.Interface.Analyze.FormCharts ChartForm = null;
        public static void ShowChart(int symbolID)
        {
            if (ChartForm == null)
                ChartForm = new Trade.Interface.Analyze.FormCharts();
            if(!ChartForm.Visible)
                    ChartForm.Show();
            ChartForm.SetSymbol(symbolID);
        }
        internal static Trade.Interface.Analyze.FormCharts ChartForm1 = null;
        public static void ShowChart1(int symbolID)
        {
            if (ChartForm1 == null)
                ChartForm1 = new Trade.Interface.Analyze.FormCharts();
            if (!ChartForm1.Visible)
                ChartForm1.Show();
            ChartForm1.SetSymbol(symbolID);
        }
        public static void ShowNewChart(int symbolID)
        {
            Trade.Interface.Analyze.FormCharts chartForm = new Trade.Interface.Analyze.FormCharts();
            if (!chartForm.Visible)
                chartForm.Show();
            chartForm.SetSymbol(symbolID);
        }
    }
}
