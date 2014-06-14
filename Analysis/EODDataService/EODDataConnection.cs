using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EODDataService.EODDataSvc;
namespace EODDataService
{
    public enum EODDataInterval { OneMinute, FiveMinute, TenMinute, FifteenMinute, ThirtyMinute, OneHour, Day, Week, Month, Top10Gain, Top10Loss, None }
    public class EODDataConnection
    {
        Data mData = null;

        public string Token { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Version { get; set; }
        public string MemberShip { get; set; }

        public EODDataConnection()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 100;
            UserName = "jhuang";
            Password = "euphoria";
            Token = "";
            MemberShip = "";
            mData = new Data();
            mData.Timeout = 30 * 60 * 1000;
        }
        public string Login()
        {
            Token = mData.Login(UserName, Password).Token;
            return Token;
        }
        public string Login(string userName, string password)
        {
            UserName = userName;
            Password = password;
            return Login();
        }
        public string Login(string version)
        {
            Token = mData.Login2(UserName, Password, version).Token;
            Version = version;
            return Token;
        }


        public CountryBase[] GetCountries()
        {
            return mData.CountryList(Token).COUNTRIES;
        }
        public EXCHANGE[] GetExchanges()
        {
            return mData.ExchangeList(Token).EXCHANGES;
        }
        public EXCHANGE[] GetExchanges(string exchange)
        {
            EXCHANGE ret = mData.ExchangeGet(Token, exchange).EXCHANGE;
            return ret == null ? new EXCHANGE[0] : new EXCHANGE[1] { ret };
        }
        public FUNDAMENTAL[] GetFundamentals(string exchange)
        {
            return mData.FundamentalList(Token, exchange).FUNDAMENTALS;
        }
        public string GetMembership()
        {
            RESPONSE ret = mData.Membership(Token);
            MemberShip = ret.MEMBERSHIP == null ? "" : ret.MEMBERSHIP;
            return MemberShip;
        }
        public QUOTE[] GetQuotes(string exchange)
        {
            return mData.QuoteList(Token, exchange).QUOTES;
        }
        public QUOTE[] GetQuotes(string exchange, string symbols)
        {
            return mData.QuoteList2(Token, exchange, symbols).QUOTES;
        }
        public QUOTE[] GetQuotes(string exchange, DateTime date)
        {
            return mData.QuoteListByDate(Token, exchange, date.QuoteDate()).QUOTES;
        }
        public QUOTE[] GetQuotes(string exchange, string symbol, DateTime startDate)
        {
            return mData.SymbolHistory(Token, exchange, symbol, startDate.QuoteDate()).QUOTES;
        }
        public QUOTE[] GetQuotes(string exchange, DateTime date, EODDataInterval interval)
        {
            return mData.QuoteListByDatePeriod(Token, exchange, date.QuoteDate(), interval.StringPeriod()).QUOTES;
        }
        public QUOTE[] GetQuotes(string exchange, string symbol, DateTime date, EODDataInterval interval)
        {
            return mData.SymbolHistoryPeriod(Token, exchange, symbol, date.QuoteDate(), interval.StringPeriod()).QUOTES;
        }
        public QUOTE[] GetQuotes(string exchange, string symbol, DateTime dateFrom, DateTime dateTo, EODDataInterval interval)
        {
            return mData.SymbolHistoryPeriodByDateRange(Token, exchange, symbol, dateFrom.QuoteDate(), dateTo.QuoteDate(), interval.StringPeriod()).QUOTES;
        }


        public SPLIT[] GetSplits(string exchange)
        {
            return mData.SplitListByExchange(Token, exchange).SPLITS;
        }
        public SPLIT[] GetSplits(string exchange, string symble)
        {
            return mData.SplitListBySymbol(Token, exchange, symble).SPLITS;
        }

        public SYMBOL[] GetSymbols(string exchange)
        {
            return mData.SymbolList(Token, exchange).SYMBOLS;
        }
        public SYMBOL[] GetSymbols(string exchange, string symbol)
        {
            SYMBOL ret = mData.SymbolGet(Token, exchange, symbol).SYMBOL;
            return ret == null ? new SYMBOL[0] : new SYMBOL[1] { ret };
        }

        public QUOTE[] GetTop10Gain(string exchange)
        {
            return mData.Top10Gains(Token, exchange).QUOTES;
        }
        public QUOTE[] GetTop10Loss(string exchange)
        {
            return mData.Top10Losses(Token, exchange).QUOTES;
        }
        public SYMBOLCHANGE[] GetSymbolChanges(string exchange)
        {
            return mData.SymbolChangesByExchange(Token, exchange).SYMBOLCHANGES;
        }
        public DATAFORMAT[] DataFormats()
        {
            return mData.DataFormats(Token).DATAFORMATS;
        }
    }
}
