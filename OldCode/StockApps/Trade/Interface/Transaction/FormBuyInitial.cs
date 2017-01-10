using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trade.Interface.Transaction
{
    public partial class FormBuyInitial : Form
    {
        int mSymbol;
        public int SymbolID
        {
            get { return mSymbol; }
            set 
            {
                mSymbol = value;
                var t = Program.GetDataSet("select rtrim(b.Name) + ': '+rtrim(a.Name)+' '+cast(a.ID as varchar(20))+' - '+ a.Description from A.Symbol a inner join A.Exchange b on a.ExchangeID = b.ID where a.ID = " + value.ToString());
                if (t.Rows.Count == 0)
                    throw new Exception("Could not found symbol {0}.".FormatString(value));
                lbSymbol.Text = "Symbol:" + t.Rows[0][0].ToString();
                mAccount.Items.Clear();
                foreach (var a in Account.GetAccounts())
                    mAccount.Items.Add(a);
            }
        }
        public FormBuyInitial()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mAccount.SelectedItem == null) return;
            mTradeFee.Text = BuySell.GetTradingFee(((Account)mAccount.SelectedItem).AccountNumber, mdate.Value, Decimal.Parse(mQuantity.Text)).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BuySell.SecurityBuyInitial(mdate.Value, ((Account)mAccount.SelectedItem).AccountNumber, SymbolID, decimal.Parse(mQuantity.Text), decimal.Parse(mRateFrom.Text), decimal.Parse(mRateTo.Text), decimal.Parse(mTradeFee.Text), decimal.Parse(mExchangeFee.Text));
            this.Hide();
        }

        private void mQuantity_TextChanged(object sender, EventArgs e)
        {
            mTotal.Text = (decimal.Parse(mQuantity.Text) * Math.Max(decimal.Parse(mRateFrom.Text), decimal.Parse(mRateTo.Text)) + decimal.Parse(mTradeFee.Text) + decimal.Parse(mExchangeFee.Text)).ToString();
        }
    }
}
