using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Algorithm.Core.Forms
{
    public partial class ChartDetailForm : Form
    {
        DataView dvExchange, dvSymbol;
        public string Exchange, Symbol, SymbolName;
        public int SymbolID;
        public EventHandler OnSearchConfirm;
        public Chart chart;
        public ChartForm ChartForm;
        public bool SearchFound()
        {
            if (dvSymbol.Count == 1)
            {
                btnOk_Click(null, null);
                return true;
            }
            return false;
        }
        public string SearchText
        {
            get { return textSearch.Text; }
            set { textSearch.Text = value; }
        }
        void ReloadData()
        {
            MainForm.RefreshData();
            SetExchangeList();
        }
        private void miExchange_Click(object sender, EventArgs e)
        {
            ddExchange.Text = ((ToolStripItem)sender).Text;
            textSearch_TextChanged(null, null);
        }
        public void SetExchangeList()
        {
            ddExchange.DropDownItems.Clear();
            ddExchange.DropDownItems.Add("All").Click += miExchange_Click;
            ddExchange.DropDownItems.Add("-");
            for (int i = 0; i < MainForm.Exchange.Rows.Count; i++)
            {
                ddExchange.DropDownItems.Add(MainForm.Exchange.Rows[i]["Exchange"].ToString()).Click += miExchange_Click;
            }
            ddExchange.Text = "All";
        }
        public ChartDetailForm()
        {
            InitializeComponent();

            dvExchange = new DataView(MainForm.Exchange);
            bsExchange.DataSource = dvExchange;
            gExchange.AutoGenerateColumns = true;
            gExchange.DataSource = bsExchange;

            dvSymbol = new DataView(MainForm.Symbol);
            bsSymbol.DataSource = dvSymbol;
            gSymbol.AutoGenerateColumns = true;
            gSymbol.DataSource = bsSymbol;
            SetExchangeList();
        }

        private void ChartDetailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            if (ddExchange.Text == "All")
                dvSymbol.RowFilter = "Symbol like '%" + textSearch.Text.Replace("'", "''") + "%'";
            else if (ddExchange.Text != "All")
                dvSymbol.RowFilter = "Symbol like '%" + textSearch.Text.Replace("'", "''") + "%' and Exchange = '" + ddExchange.Text + "'";
        }

        

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (bsSymbol.Current == null)
                return;
            Exchange = ((DataRowView)bsSymbol.Current)["Exchange"].ToString();
            Symbol = ((DataRowView)bsSymbol.Current)["Symbol"].ToString();
            SymbolName = ((DataRowView)bsSymbol.Current)["SymbolName"].ToString();
            SymbolID = (int)((DataRowView)bsSymbol.Current)["SymbolID"];
            if (OnSearchConfirm != null)
                OnSearchConfirm(this, EventArgs.Empty);
        }

        private void textSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==(char)13)
            {
                btnOk_Click(null, null);
            }
        }

        private void gSymbol_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyValue == (char)13)
            {
                btnOk_Click(null, null);
            }
        }

        private void btnSetSplitter_Click(object sender, EventArgs e)
        {
            ChartForm.SetSplitter();
            ChartForm.RearrangeSplitters();
        }
    }
}
