using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace Trade.Interface.Transaction
{
    public enum BuySellType { Buy, Sell, SellInitial }
    class BuySell
    {
        public static decimal GetTradingFee(string accountNumber, DateTime date, decimal Quantity)
        {
            return (decimal)Program.GetDataSet("select A.CalculateTradeFee({0}, {1}, {2})".FormatString(accountNumber.ToSQLString(), date.ToODBCStringShort().ToSQLString(), Quantity)).Rows[0][0];
        }
        public static int SecurityBuyInitial(DateTime date, string accountNumber, int symbolID, decimal quantity, decimal rateFrom, decimal rateTo, decimal tradeFee, decimal exchangeFee)
        {
            return (int)Program.GetDataSet("exec [A].[SecurityBuyInitial] @Date={0}, @AccountNumber={1}, @SymbolID={2}, @Quantity={3}, @RateFrom={4}, @RateTo={5}, @TradingFee={6}, @ExchangeFee={7}".FormatString(date.ToODBCStringShort().ToSQLString(), accountNumber.Trim().ToSQLString(), symbolID, quantity, rateFrom, rateTo, tradeFee, exchangeFee)).Rows[0][0];
        }
        public static void SecurityBuy(int transID, DateTime date, decimal quantity, decimal rate, decimal tradeFee, decimal exchangeFee)
        {
            Program.GetDataSet("exec [A].[SecurityBuy] @TransID={0}, @Date={1}, @Quantity={2}, @Rate={3}, @TradingFee={4}, @ExchangeFee={5}".FormatString(transID, date.ToODBCStringShort().ToSQLString(), quantity, rate, tradeFee, exchangeFee));
            return;
        }
        public static void SecurityBuyRemove(int transID)
        {
            Program.GetDataSet("exec [A].[SecurityBuyRemove] @TransID={0}".FormatString(transID));
            return;
        }
        public static void SecuritySellInitial(int transID, DateTime date, decimal quantity, decimal rateFrom, decimal rateTo, decimal tradeFee, decimal exchangeFee)
        {
            Program.GetDataSet("exec [A].[SecuritySellInitial] @TransID ={0}, @Date={1}, @Quantity={2}, @RateFrom={3}, @RateTo={4}, @TradingFee={5}, @ExchangeFee={6}".FormatString(transID, date.ToODBCStringShort().ToSQLString(), quantity, rateFrom, rateTo, tradeFee, exchangeFee));
            return;
        }
        public static void SecuritySell(int transID, DateTime date, decimal quantity, decimal rate, decimal tradeFee, decimal exchangeFee)
        {
            Program.GetDataSet("exec [A].[SecuritySell] @TransID={0}, @Date={1}, @Quantity={2}, @Rate={3}, @TradingFee={4}, @ExchangeFee={5}".FormatString(transID, date.ToODBCStringShort().ToSQLString(), quantity, rate, tradeFee, exchangeFee));
            return;
        }
        public static void SecuritySellRemove(int transID)
        {
            Program.GetDataSet("exec [A].[SecuritySellRemove] @TransID={0}".FormatString(transID));
            return;
        }

        private static FormBuyInitial formBuyInitial = null;
        public static void BuyInitial(int symbolID)
        {
            if(formBuyInitial==null)
                formBuyInitial = new FormBuyInitial();
            formBuyInitial.SymbolID = symbolID;
            formBuyInitial.Show();
        }
        public static void BuyRemove(int transID)
        {
            if (MessageBox.Show("Are you should you want to cancel purchase transaciton {0}?".FormatString(transID), "Confirm", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;
            Program.RunSQL("exec [A].[SecurityBuyRemove] {0}".FormatString(transID));
        }

        private static FormBuySell formBuySell = null;
        public static void Buy(int transID)
        {
            if(formBuySell == null)
                formBuySell = new FormBuySell();
            formBuySell.BuySellType = BuySellType.Buy;
            formBuySell.TransID = transID;
            formBuySell.Show();
        }
        public static void Sell(int transID)
        {
            if (formBuySell == null)
                formBuySell = new FormBuySell();
            formBuySell.BuySellType = BuySellType.Sell;
            formBuySell.TransID = transID;
            formBuySell.Show();
        }
        public static void SellInitial(int transID)
        {
            if (formBuySell == null)
                formBuySell = new FormBuySell();
            formBuySell.BuySellType = BuySellType.SellInitial;
            formBuySell.TransID = transID;
            formBuySell.Show();
        }
        public static void SellRemove(int transID)
        {
            if (MessageBox.Show("Are you should you want to cancel sales transaciton {0}?".FormatString(transID), "Confirm", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;
            Program.RunSQL("exec [A].[SecuritySellRemove] {0}".FormatString(transID));
        }
    }
    class Account
    {
        public string AccountNumber;
        public string Provider;
        public string CurrencyType;
        public decimal Amount;
        public decimal Payable;
        public decimal Balance;
        public override string ToString()
        {
            return "{0}({1}): Amount: {2}, Payable:{3}, Balance: {4}".FormatString(AccountNumber, CurrencyType, Amount, Payable, Balance);
        }
        public static List<Account> GetAccounts()
        {
            return Program.GetDataSet("select * from A.Account where Active = 1 order by 1").AsEnumerable().Select(x => { return new Account() { AccountNumber = x.Field<string>("AccountNumber"), Provider = x.Field<string>("Provider"), CurrencyType = x.Field<string>("CurrencyType"), Amount = x.Field<decimal>("Amount"), Payable = x.Field<decimal>("Payable"), Balance = x.Field<decimal>("Balance")}; }).ToList<Account>();
        }
    }
}
