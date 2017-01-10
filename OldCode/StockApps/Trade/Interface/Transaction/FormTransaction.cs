using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Trade.Interface.Analyze;

namespace Trade.Interface.Transaction
{
    public partial class FormTransaction : Trade.Base.BaseForm
    {
        bool isOpening = false;
        void ReOpen1()
        {
            try
            {
                isOpening = true;
                daTransSecurity1.ClearBeforeFill = true;
                daTransSecurity1.Fill(dataSetTransaction.TransSecurity);
                daTrans1.ClearBeforeFill = true;
                if(bsSecurity.Current != null)
                    daTrans1.Fill(dataSetTransaction.Trans, (int)((DataRowView)bsSecurity.Current)["TransID"]);
                else
                    daTrans1.Fill(dataSetTransaction.Trans, -1);
            }
            finally
            {
                isOpening = false;
            }
        }
        bool initializing = false;
        public int SelectedStatus
        {
            get { return tbMain.SelectedIndex; }
        }
        public FormTransaction()
        {
            initializing = true;
            Program.fromTransactions = this;
            InitializeComponent();
            var l = this.GetDataSet("SELECT ID, Name, Description FROM A.TransProcessingStatus ORDER BY ID").AsEnumerable().Select(x => { return x.Field<string>("Name");});
            tbMain.TabPages.Clear();
            foreach (string s in l)
                tbMain.TabPages.Add(s);
            initializing = false;
            this.ReOpen1();
        }

        private void FormTransaction_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.fromTransactions = null;
        }

        private void tbMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initializing)
                return;
            this.ReOpen1();
        }

        private void cmBuy_Opening(object sender, CancelEventArgs e)
        {
            miBuyRemove.Enabled = (tbMain.SelectedIndex == 0 && bsSecurity.Current != null);
            miBuy.Enabled = ((tbMain.SelectedIndex == 0 || tbMain.SelectedIndex == 1) && bsSecurity.Current != null);
            miSellInitialize.Enabled = ((tbMain.SelectedIndex == 2 || tbMain.SelectedIndex == 3) && bsSecurity.Current != null);
            if (miSellInitialize.Enabled)
                miSellInitialize.Enabled = (Decimal.Parse(((DataRowView)bsSecurity.Current)["QuantityAvailable"].ToString()) > 0);
            miShowExtendedChart.Enabled = bsSecurity.Current != null;
            miShowExtendedChart.Enabled = bsSecurity.Current != null;
            miShowExtendedChart.Enabled = bsSecurity.Current != null;
            if (cmBuy.Items.Cast<Object>().Where(x => { return x is ToolStripMenuItem; }).Cast<ToolStripMenuItem>().FirstOrDefault(x => { return x.Enabled; }) == null)
                e.Cancel = true;
        }

        private void miBuyRemove_Click(object sender, EventArgs e)
        {
            if (bsSecurity.Current == null)
                return;
            BuySell.BuyRemove(int.Parse(((DataRowView)bsSecurity.Current)["TransID"].ToString()));
            this.ReOpen1();
        }

        private void miBuy_Click(object sender, EventArgs e)
        {
            if (bsSecurity.Current == null)
                return;
            BuySell.Buy(int.Parse(((DataRowView)bsSecurity.Current)["TransID"].ToString()));
            this.ReOpen1();
        }

        private void miSellInitialize_Click(object sender, EventArgs e)
        {
            if (bsSecurity.Current == null)
                return;
            BuySell.SellInitial(int.Parse(((DataRowView)bsSecurity.Current)["TransID"].ToString()));
            this.ReOpen1();
        }

        private void miSell_Click(object sender, EventArgs e)
        {
            if (bsTrans.Current == null)
                return;
            BuySell.Sell(int.Parse(((DataRowView)bsTrans.Current)["TransID"].ToString()));
            this.ReOpen1();
        }

        private void miSellremove_Click(object sender, EventArgs e)
        {
            if (bsTrans.Current == null)
                return;
            BuySell.SellRemove(int.Parse(((DataRowView)bsTrans.Current)["TransID"].ToString()));
            this.ReOpen1();
        }

        private void cmSell_Opening(object sender, CancelEventArgs e)
        {
            if (bsTrans.Current == null)
            {
                e.Cancel = true;
                return;
            }
            int i = TransSellDeletableOrSellable(int.Parse(((DataRowView)bsTrans.Current)["TransID"].ToString()));
            miSell.Enabled = i == 1 || i == 2;
            miSellremove.Enabled = i == 1;
            if (cmSell.Items.Cast<ToolStripMenuItem>().FirstOrDefault(x => { return x.Enabled; }) == null)
                e.Cancel = true;
        }
        private int TransSellDeletableOrSellable(int transID)
        {
            return (int)Program.GetDataSet("select A.TransSellDeletableOrSellable({0})".FormatString(transID)).Rows[0][0];
        }

        private void dg1_DoubleClick(object sender, EventArgs e)
        {
            if (bsSecurity.Current == null)
                return;
            Program.ShowChart((int)((DataRowView)bsSecurity.Current)["SymbolID"]);
        }

        private void bsSecurity_CurrentChanged(object sender, EventArgs e)
        {
            if (isOpening)
                return;
            daTrans1.ClearBeforeFill = true;
            if (bsSecurity.Current != null)
                daTrans1.Fill(dataSetTransaction.Trans, (int)((DataRowView)bsSecurity.Current)["TransID"]);
            else
                daTrans1.Fill(dataSetTransaction.Trans, -1);
        }

        private void miShowExtendedNewChart_Click(object sender, EventArgs e)
        {
            if (bsSecurity.Current == null)
                return;
            Program.ShowNewChart((int)((DataRowView)bsSecurity.Current)["SymbolID"]);
        }

        private void miShowExtendedChart1_Click(object sender, EventArgs e)
        {
            if (bsSecurity.Current == null)
                return;
            Program.ShowChart1((int)((DataRowView)bsSecurity.Current)["SymbolID"]);
        }
    }
}
