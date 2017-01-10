using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace Console
{
	static class Global
	{
		public static FormMain mainForm = null;
		public static Form CreateForm(Type formType)
		{
			Form f = Global.GetForm(formType);
			if (f != null)
			{
				f.Activate();
				return f;
			}
			f = (Form)Activator.CreateInstance(formType);
			f.Show();
			return f;
		}
		public static Form GetForm(Type formType)
		{
			if (Global.mainForm == null)
				return null;
			foreach (Form f in Global.mainForm.MdiChildren)
			{
                if (f.GetType() == formType)
					return f;
			}
			return null;
		}
        public static SqlConnection GetConnection(string server)
        {
            return new SqlConnection("Server=" + server + ";MultipleActiveresultSets = true;Integrated Security=SSPI");
        }
        public static SqlConnection GetConnection(string server, string database)
        {
            return new SqlConnection("Server=" + server + ";Database = " + database + ";MultipleActiveresultSets = true;Integrated Security=SSPI");
        }
        public static SqlCommand GetCommand()
        {
            return Global.GetCommand("JVS01", "Stock");
        }
        public static SqlCommand GetCommand(string server)
        {
            return Global.GetConnection(server).CreateCommand();
        }
        public static SqlCommand GetCommand(string server, string database)
        {
            return Global.GetConnection(server, database).CreateCommand();
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
            if(p.Length == 0)
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
	}
    public class BaseFormComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return Compare((Form)x, (Form)y);
        }
        public int Compare(Form x, Form y)
        {
            return x.Text.CompareTo(y.Text);
        }
    }	
}
