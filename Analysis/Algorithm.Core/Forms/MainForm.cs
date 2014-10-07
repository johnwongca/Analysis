using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Algorithm.Core;

namespace Algorithm.Core.Forms
{
    public partial class MainForm : Form
    {
        public static FormSymbol SymbolForm = null;
        public static MainForm Main = null;
        public static List<FormBase> Children = new List<FormBase>();
        public static DataTable Exchange = null;
        public static DataTable Symbol = null;
        void CreateSymbolForm()
        {
            if (SymbolForm == null)
                SymbolForm = new FormSymbol();
        }
        public static void RefreshData()
        {
            if (Exchange == null)
                Exchange = new DataTable();
            if (Symbol == null)
                Symbol = new DataTable();
            using (SqlConnection connection = Methods.GetConnection())
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "q.GetExchange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Exchange.Rows.Clear();
                        Exchange.Load(reader);
                    }
                    cmd.CommandText = "q.GetSymbol";
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Symbol.Rows.Clear();
                        Symbol.Load(reader);
                    }

                }
            }
            Exchange.CaseSensitive = false;
            Symbol.CaseSensitive = false;
        }
        public MainForm()
        {
            InitializeComponent();
            RefreshData();
            CreateSymbolForm();
            this.BringToFront();
        }

        private void miSymbols_Click(object sender, EventArgs e)
        {
            SymbolForm.Show();
        }

        private void miCloseAll_Click(object sender, EventArgs e)
        {
            foreach (FormBase f in Children.ToArray())
                f.Close();
        }
        private void miWindowItems_Click(object sender, EventArgs e)
        {
            FormBase f = ((FormBase)((ToolStripItem)sender).Tag);
            f.Show();
            f.BringToFront();
        }

        private void miWindows_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripItem item;
            miWindows.DropDownItems.Clear();
            miWindows.DropDownItems.Add("Close All", null, miCloseAll_Click);
            miWindows.DropDownItems.Add("-");
            foreach(FormBase f in Children.OrderBy(x=>x.Text))
            {
                item = miWindows.DropDownItems.Add(f.Text, null, miWindowItems_Click);
                item.Tag = f;
            }
        }

        
        private void btnChart_Click(object sender, EventArgs e)
        {
            (new ChartForm()).Show();
        }

        
    }
}
