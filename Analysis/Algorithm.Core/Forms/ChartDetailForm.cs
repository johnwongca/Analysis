using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            else if (dvSymbol.Count >= 1)
            {
                if (SearchText.Length >= 1)
                {
                    for (int i = 0; i < dvSymbol.Count; i++)
                    {
                        var r = dvSymbol[i];
                        if(r.Row["Symbol"].ToString().ToUpper() == SearchText.ToUpper())
                        {
                            bsSymbol.Position = i;
                            btnOk_Click(null, null);
                            return true;
                        }
                    }
                }
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
            lExchange.Items.Clear();
            ddExchange.DropDownItems.Clear();
            ddExchange.DropDownItems.Add("All").Click += miExchange_Click;
            ddExchange.DropDownItems.Add("-");
            for (int i = 0; i < MainForm.Exchange.Rows.Count; i++)
            {
                ddExchange.DropDownItems.Add(MainForm.Exchange.Rows[i]["Exchange"].ToString()).Click += miExchange_Click;
                lExchange.Items.Add(MainForm.Exchange.Rows[i]["Exchange"].ToString());
            }
            lExchange.Text = "NASDAQ";
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
            if (textSearch.Text.Length >= 1)
            {
                for (int i = 0; i < dvSymbol.Count; i++)
                {
                    var r = dvSymbol[i];
                    if (r.Row["Symbol"].ToString().ToUpper() == textSearch.Text.ToUpper())
                    {
                        bsSymbol.Position = i;
                        break;
                    }
                }
            }
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
        private void btnScan_Click(object sender, EventArgs e)
        {
            Scan();
        }

        private void btnScanStop_Click(object sender, EventArgs e)
        {
            scanStopped = true;
        }

        #region Scan
        DataTable sourceForScan = new DataTable("ScanSource");
        DataTable scanResult = new DataTable("ScanResult");
        double Shares;
        decimal MarketCap;
        bool scanStopped = false;
        void LoadData()
        {
            ChartForm.LoadData(sourceForScan);
        }
        void Scan()
        {
            scanResultGrid.AutoGenerateColumns = true;
            scanResult.Rows.Clear();
            if(scanResult.Columns.Count == 0)
            {
                DataColumn c;
                c = scanResult.Columns.Add("Symbol", typeof(string));
                c = scanResult.Columns.Add("TypicalPrice", typeof(double));
                c = scanResult.Columns.Add("TurnOver", typeof(double));
                
                c = scanResult.Columns.Add("Over5", typeof(int));
                c = scanResult.Columns.Add("RSI", typeof(int));
                c = scanResult.Columns.Add("DaysIncrease", typeof(int));
                //c = scanResult.Columns.Add("AvgDayIncreasePCT", typeof(double));
                c = scanResult.Columns.Add("Bollinger", typeof(int));
                c = scanResult.Columns.Add("MACD", typeof(int));
                c = scanResult.Columns.Add("UltimateOscillator", typeof(int));
                c = scanResult.Columns.Add("Jump", typeof(int));
                c = scanResult.Columns.Add("MarketCap", typeof(decimal));
                c = scanResult.Columns.Add("Exchange", typeof(string));
                c = scanResult.Columns.Add("SymbolID", typeof(int));
                c = scanResult.Columns.Add("SymbolName", typeof(string));
                
            }
            
            try
            {
                btnScan.Enabled = false;
                btnScanStop.Enabled = true;
                progressBar1.Visible = true;
                bsScanResult.DataSource = scanResult;
                Application.DoEvents();
                using (DataView dv = new DataView(MainForm.Symbol))
                {
                    dv.RowFilter = "Exchange='"+lExchange.Text+"'";
                    progressBar1.Maximum = dv.Count;
                    progressBar1.Minimum = 0;
                    progressBar1.Value = 0;
                    for(int i=0; i<dv.Count; i++)
                    {
                        if (scanStopped)
                            return;
                        SymbolID = (int)(dv[i]["SymbolID"]);
                        Symbol = (string)(dv[i]["Symbol"]);
                        Exchange = (string)(dv[i]["Exchange"]);
                        SymbolName = (string)(dv[i]["SymbolName"]);
                        Shares = Convert.ToDouble(dv[i]["Shares"]);
                        MarketCap = Math.Round (Convert.ToDecimal(dv[i]["MarketCap"])/1000/1000, 2);
                        ChartForm.SymbolID = SymbolID;
                        LoadData();
                        ScanCheck();
                        progressBar1.Value = i;
                        Application.DoEvents();
                    }
                }
            }
            finally
            {
                progressBar1.Visible = false;
                btnScan.Enabled = true;
                btnScanStop.Enabled = false;
                scanStopped = false;
                
            }
            
        }
        void ScanCheck()
        {
            if (sourceForScan.Rows.Count == 0)
                return;
            double a, b, c, high, low, typicalPrice, open, close;

            DataRow s = sourceForScan.Rows[sourceForScan.Rows.Count - 1];
            DataRow s_1 = null;
            if (sourceForScan.Rows.Count > 1)
                s_1 = sourceForScan.Rows[sourceForScan.Rows.Count - 2];
            high = Convert.ToDouble(s["High"]);
            low = Convert.ToDouble(s["Low"]);
            open = Convert.ToDouble(s["Open"]);
            close = Convert.ToDouble(s["Close"]);
            typicalPrice = Convert.ToDouble(s["TypicalPrice"]);
            int DaysIncrease = 0;
            double increase;
            double previousClose = -1;
            foreach(DataRow rr in sourceForScan.Rows)
            {
                increase = Convert.ToDouble(rr["Close"]) - Convert.ToDouble(rr["Open"]);
                if (previousClose > 0)
                {
                    if (increase < 0)
                    {
                        if(previousClose < Convert.ToDouble(rr["Close"]))
                            DaysIncrease = 0;
                        if (DaysIncrease > 0)
                            DaysIncrease = 0;
                        DaysIncrease--;
                    }
                    else if (increase > 0)
                    {
                        if(previousClose > Convert.ToDouble(rr["Close"]))
                            DaysIncrease = 0;
                        if (DaysIncrease < 0)
                            DaysIncrease = 0;
                        DaysIncrease++;
                    }
                    else
                    {
                        DaysIncrease = 0;
                    }
                }
                previousClose = Convert.ToDouble(rr["Close"]);
            }
            
            DataRow t = scanResult.NewRow();
            t.BeginEdit();
            t["MarketCap"] = MarketCap;
            t["DaysIncrease"] = DaysIncrease;
            t["TurnOver"] = 0;
            if (Shares > 0)
            {
                t["TurnOver"] = Convert.ToDouble(s["Volume"]) * 100 / Shares;
            }
            t["TypicalPrice"] = typicalPrice;
            t["Exchange"] = Exchange;
            t["Symbol"] = Symbol;
            t["SymbolID"] = SymbolID;
            t["SymbolName"] = SymbolName;
            t["MACD"] = Convert.ToInt32 (Convert.ToDouble(s["MACDDivergence"]) * 100);
            a = Convert.ToDouble(s["BollingerBandsUpper"]);
            b = Convert.ToDouble(s["BollingerBandsAverage"]);
            c = Convert.ToDouble(s["BollingerBandsLower"]);
            a = a - c;
            b = b - c;
            c = typicalPrice - c;
            t["Bollinger"] = a <=double.Epsilon? 0: (int)(c * 100/a);
            a = Convert.ToDouble(s["RSI"]);
            t["RSI"] = Convert.ToInt32(a);
            t["UltimateOscillator"] = Convert.ToInt32((Convert.ToDouble(s["UltimateOscillator"])) * 100);
            t["Jump"] = 0;
            t["Over5"] = 0;
            if (s_1!=null)
            {
                if(Convert.ToDouble(s_1["High"])< Convert.ToDouble(s["Low"]))
                {
                    t["Jump"] = 1;
                }
                if (Convert.ToDouble(s_1["Low"]) > Convert.ToDouble(s["High"]))
                {
                    t["Jump"] = -1;
                }
                if (
                        (Convert.ToDouble(s_1["TypicalPrice"]) < Convert.ToDouble(s_1["MAShort"]))
                    &&
                        (Convert.ToDouble(s["TypicalPrice"]) > Convert.ToDouble(s["MAShort"]))
                    )
                {
                    t["Over5"] = 1;
                }
                
            }
            t.EndEdit();
            bool rowAdded = false;
            if(!rowAdded&&((a>=69)||(a<=31)))
            {
                scanResult.Rows.Add(t);
                rowAdded = true;
            }
            if (!rowAdded && ((int)t["Jump"]!=0))
            {
                scanResult.Rows.Add(t);
                rowAdded = true;
            }
            if (!rowAdded && ((int)t["Over5"] != 0))
            {
                scanResult.Rows.Add(t);
                rowAdded = true;
            }
            if(!rowAdded)
                t.Delete();
        }
        #endregion

        private void scanResultGrid_DoubleClick(object sender, EventArgs e)
        {
            if (bsScanResult.Current == null)
                return;
            Exchange = ((DataRowView)bsScanResult.Current)["Exchange"].ToString();
            Symbol = ((DataRowView)bsScanResult.Current)["Symbol"].ToString();
            SymbolID = (int)((DataRowView)bsScanResult.Current)["SymbolID"];
            SymbolName = ((DataRowView)bsScanResult.Current)["SymbolName"].ToString();
            if (OnSearchConfirm != null)
                OnSearchConfirm(this, EventArgs.Empty);
        }

      

    }
}
