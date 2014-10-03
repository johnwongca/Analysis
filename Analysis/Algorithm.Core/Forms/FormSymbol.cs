using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Algorithm.Core.Forms
{
    
    public partial class FormSymbol : Algorithm.Core.Forms.FormBase
    {
        DataView dvExchange, dvSymbol;
        public string Exchange, Symbol, SymbolName;
        public int SymbolID;
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
            for (int i = 0; i < MainForm.Exchange.Rows.Count; i++ )
            {
                ddExchange.DropDownItems.Add(MainForm.Exchange.Rows[i]["Exchange"].ToString()).Click += miExchange_Click;
            }
            ddExchange.Text = "All";
        }
        public FormSymbol()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            if (Modal)
                Close();
        }

        private void FormSymbol_Load(object sender, EventArgs e)
        {

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

        private void FormSymbol_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        

        private void FormSymbol_Activated(object sender, EventArgs e)
        {
            btnOk.Visible = Modal;
        }


        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void bsSymbol_CurrentChanged(object sender, EventArgs e)
        {
            if(bsSymbol.Current == null)
                return;
            if(!(bsSymbol.Current is DataRowView))
                return;
            Exchange = ((DataRowView)bsSymbol.Current)["Exchange"].ToString();
            Symbol = ((DataRowView)bsSymbol.Current)["Symbol"].ToString();
            SymbolName =((DataRowView)bsSymbol.Current)["SymbolName"].ToString();
            SymbolID = (int)((DataRowView)bsSymbol.Current)["SymbolID"];
            //Console.WriteLine("{0},{1},{2},{3}", Exchange, SymbolID, Symbol, SymbolName);
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            if (ddExchange.Text == "All")
                dvSymbol.RowFilter = "Symbol like '%" + textSearch.Text.Replace("'", "''") + "%'";
            else if (ddExchange.Text != "All") 
                dvSymbol.RowFilter = "Symbol like '%" + textSearch.Text.Replace("'", "''") + "%' and Exchange = '" + ddExchange.Text + "'";

        }

        private void gSymbol_DoubleClick(object sender, EventArgs e)
        {
            if(Modal)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }
    }
}
