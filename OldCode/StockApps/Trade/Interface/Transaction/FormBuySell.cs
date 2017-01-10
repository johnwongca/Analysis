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
    public partial class FormBuySell : Form
    {
        string accountNumber;
        int mTransID; BuySellType mBuySellType;
        public BuySellType BuySellType
        {
            get { return mBuySellType; }
            set 
            {
                mBuySellType = value;
                mRateTo.Visible = mBuySellType == BuySellType.SellInitial;
                this.Text = mBuySellType.ToString();
                if (mBuySellType == BuySellType.SellInitial)
                    this.Text = "Sell Initial";
            }
        }
        public int TransID
        {
            get { return mTransID; }
            set 
            { 
                mTransID = value;
                DataTable t = Program.GetDataSet("select top 1 rtrim(b.Name) + ': '+rtrim(a.Name)+' '+cast(a.ID as varchar(20))+' - '+ a.Description as Description, c.Quantity, c.QuantityAvailable, c.Quantity - c.QuantityAvailable QuantityBalance, c.AccountNumber from A.Symbol a inner join A.Exchange b on a.ExchangeID = b.ID inner join A.TransSecurity c on c.SymbolID = a.ID	inner join A.Trans d on c.TransID = D.ParentID where D.TransID = " + value.ToString());
                if (t.Rows.Count == 0)
                    throw new Exception("Could not find transaction {0}.".FormatString(value));
                accountNumber = t.Rows[0]["AccountNumber"].ToString();
                lbSymbol.Text = t.Rows[0]["Description"].ToString();
                lbQty.Text = "Total Qty: {0}, Available Qty: {1}, Balance: {2}".FormatString(t.Rows[0][1], t.Rows[0][2], t.Rows[0][1]);
            }
        }
        public FormBuySell()
        {
            InitializeComponent();
        }

        private void mQuantity_TextChanged(object sender, EventArgs e)
        {
            if(mBuySellType == BuySellType.SellInitial)
                mTotal.Text = (decimal.Parse(mQuantity.Text) * Math.Min(decimal.Parse(mRateFrom.Text), decimal.Parse(mRateTo.Text)) - decimal.Parse(mTradeFee.Text) - decimal.Parse(mExchangeFee.Text)).ToString();
            else if (mBuySellType == BuySellType.Sell)
                mTotal.Text = (decimal.Parse(mQuantity.Text) * decimal.Parse(mRateFrom.Text) - decimal.Parse(mTradeFee.Text) - decimal.Parse(mExchangeFee.Text)).ToString();
            else if (mBuySellType == BuySellType.Buy)
                mTotal.Text = (decimal.Parse(mQuantity.Text) * decimal.Parse(mRateFrom.Text) + decimal.Parse(mTradeFee.Text) + decimal.Parse(mExchangeFee.Text)).ToString();
            else
                mTotal.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mTradeFee.Text = BuySell.GetTradingFee(accountNumber, mdate.Value, Decimal.Parse(mQuantity.Text)).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mBuySellType == BuySellType.Buy)
            {
                BuySell.SecurityBuy(mTransID, mdate.Value, decimal.Parse(mQuantity.Text), decimal.Parse(mRateFrom.Text), decimal.Parse(mTradeFee.Text), decimal.Parse(mExchangeFee.Text));
            }
            else if (mBuySellType == BuySellType.Sell)
            {
                BuySell.SecuritySell(mTransID, mdate.Value, decimal.Parse(mQuantity.Text), decimal.Parse(mRateFrom.Text), decimal.Parse(mTradeFee.Text), decimal.Parse(mExchangeFee.Text));
            }
            else if(mBuySellType == BuySellType.SellInitial)
            {
                BuySell.SecuritySellInitial(mTransID, mdate.Value, decimal.Parse(mQuantity.Text), decimal.Parse(mRateFrom.Text), decimal.Parse(mRateTo.Text), decimal.Parse(mTradeFee.Text), decimal.Parse(mExchangeFee.Text));
            }
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
