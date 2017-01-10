using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Trade.Analyze;

namespace Trade.Interface.Analyze
{
    public partial class FormTest : Trade.Base.BaseForm
    {
        List<Exchange> exchanges = null;
        public FormTest()
        {
            InitializeComponent();
            progress.Visible = false;
            lbStatus1.Visible = false;
            lbStatus2.Visible = false;
            exchanges = Exchange.GetExchanges();
            cbExchange.Items.Clear();
            cbExchange.Items.AddRange(exchanges.ToArray());
            cbExchange.SelectedItem = exchanges.FirstOrDefault(x => { return x.ExchangeID == 16; });
            stopSearching = false;
            isSearching = false;
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            bsTestSetting.Update();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
            this.ReOpen(bsTestResult);
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (isSearching)
                stopSearching = true;
        }
        private void FormTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSearching)
                stopSearching = true;
        }
        private bool stopSearching, isSearching;

        private void Search()
        {
            Exchange ex = (Exchange)cbExchange.SelectedItem;
            Symbol symbol = null;
            List<double> ld0 = null, ld1 = null;//, ld2 = null, ld3 = null, ld4 = null, ld5 = null;
            List<double> closing = null;
            int i = 0, j = 0, c = 0;
            double d0 = 0, d1 = 0, d2 = 0, d3 = 0;
            Price lastPrice;
            DataTable dt;
            DateTime fromDate = new DateTime(2006, 1, 1);
            bool shouldContinue = false;
            try
            {

                stopSearching = false;
                isSearching = true;
                if (ex.Symbols.Count <= 100)
                    return;
                Program.RunSQL("delete from A.TestResult");
                Program.RunSQL("delete from A.SymbolTreeCustomList where TreeID = 6");
                progress.ProgressBar.Maximum = ex.Symbols.Count;
                progress.ProgressBar.Minimum = 1;
                progress.ProgressBar.Value = 1;
                progress.ProgressBar.Visible = true;
                lbStatus1.Visible = true;
                lbStatus2.Visible = true;
                for (i = 0; i < ex.Symbols.Count; i++)
                {
                    try
                    {
                        lbStatus1.Text = "...{0}/{1}...{2} Selected...".FormatString(i, progress.ProgressBar.Maximum, c);
                        symbol = ex.Symbols[i];
                        symbol.FromDate = fromDate;
                        symbol.ReleasePriceList();
                        if (stopSearching)
                            return;
                        dt = Program.GetDataSet("select 1 from A.TestResult where SymbolID = {0}".FormatString(symbol.SymbolID));
                        if (dt.Rows.Count > 0)
                            continue;
                        lbStatus2.Text = "Loading Symbol {0}-{1}...".FormatString(symbol.SymbolName, symbol.Description);
                        Application.DoEvents();
                        closing = symbol.GetValues(PriceName.Closing);
                        if (symbol.Prices.Count < 60)
                            continue;
                        lbStatus2.Text = "Analyzing Symbol {0}-{1}...".FormatString(symbol.SymbolName, symbol.Description);
                        Application.DoEvents();
                        
                        lastPrice = symbol.Prices.Last<Price>();
                        /*MA50*/
                        /*ld0 = closing.ExponentialMovingAverage(50);
                        if (!(lastPrice.High <= ld0.Last<double>()))
                            continue;*/

                        /*SMA10*/
                        ld0 = closing.SimpleMovingAverage(10);
                        if (!(lastPrice.Closing <= ld0.Last<double>()))
                            continue;
                        /*BB*/
                        var bb = closing.BollingerBands(ld0, 10, 1.5);
                        ld1 = bb[1];
                        if(!(lastPrice.Low<ld1[ld1.Count-1]))
                            continue;
                        /*MACD*/
                        var macd = closing.MACD(7, 4, 3);
                        ld0 = macd[0];
                        ld1 = macd[1];
                        if (!(ld0[ld0.Count - 1] < 0 && ld1[ld1.Count - 1] < 0))
                            continue;
                        shouldContinue = true;
                        j = 0;
                        for (int k = ld0.Count - 1; k > 0; k--)
                        {
                            if (ld0[k] >= 0 || ld1[k] >= 0)
                                break;
                            d0 = ld0[k]; d1 = ld0[k - 1];
                            d2 = ld1[k]; d3 = ld1[k - 1];
                            if (d3>=d1&& d0>d2)
                                j++;
                            if (j >= 1)
                                break;
                        }
                        if (j >= 1 && ld0[ld0.Count - 1] > ld1[ld1.Count - 1])
                            shouldContinue = false;
                        /*if(j>=2)
                            shouldContinue = false;*/
                        if (shouldContinue)
                            continue;
                        /*RSI*/
                        ld0 = closing.Gain().SimpleMovingAverage(5);
                        ld1 = closing.Loss().SimpleMovingAverage(5);
                        ld0 = ld0.RelativeStrengthIndex(ld1);
                        d0 = ld0[ld0.Count - 1];
                        d1 = ld0[ld0.Count - 2];
                        if (!(d0 <= 30))
                            continue;
                        /*UO*/
                        ld0 = symbol.Prices.UltimateOscillator(5, 10, 15);
                        d0 = ld0[ld0.Count - 1];
                        d1 = ld0[ld0.Count - 2];
                        if (!(d0 < 30))
                            continue;
                        /*CMO*/
                        ld0 = closing.ChandeMomentumOscillator(5);
                        d0 = ld0[ld0.Count - 1];
                        d1 = ld0[ld0.Count - 2];
                        if (!(d0 < -50d))
                            continue;
                        Program.RunSQL("insert into A.TestResult(SymbolID, Name, Description) values({0},{1},{2})".FormatString(symbol.SymbolID, symbol.SymbolName.ToSQLString(), symbol.Description.ToSQLString()));
                        Program.RunSQL("insert into A.SymbolTreeCustomList(TreeID, SymbolID) values(6,{0})".FormatString(symbol.SymbolID));
                        c++;
                    }
                    finally
                    {
                        symbol.ReleasePriceList();
                        ld0 = null; ld1 = null;
                        //ld2 = null; ld3 = null;
                        //ld4 = null; ld5 = null;
                        closing = null;
                        if(progress.ProgressBar.Value < progress.ProgressBar.Maximum)
                            progress.ProgressBar.Value ++;
                    }
                }
            }
            finally
            {
                stopSearching = false;
                isSearching = false;
                progress.ProgressBar.Visible = false;
                lbStatus1.Visible = false;
                lbStatus2.Visible = false;
            }
        }

        
    }
}
