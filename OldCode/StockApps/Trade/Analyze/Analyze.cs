using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using Trade.Base;
namespace Trade.Analyze
{
    public enum PriceType { Daily, Weekly, Monthly, Quarterly, HalfYear, Yearly, Days}
    public enum PriceName{Opening = 0, High = 1, Low = 2, Closing = 3, Volume = 4, Interest = 5, TypicalClosing = 6, WeightedClosing = 7, MedianPrice = 8, DateTime = 9}
    public partial class Exchange
    {
        int m_ExchangeID;
        string m_ExchangeName, m_Description;
        public int ExchangeID
        {
            get { return m_ExchangeID; }
            set { m_ExchangeID = value; }
        }
        public string ExchangeName
        {
            get { return m_ExchangeName; }
            set { m_ExchangeName = value; }
        }
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        public static List<Exchange> GetExchanges()
        {
            return Program.GetDataSet("select * from A.Exchange order by Name").AsEnumerable().Select(x => { return new Exchange(x.Field<int>("ID"), x.Field<string>("Name"), x.Field<string>("Description")); }).ToList<Exchange>();
        }
        List<Symbol> m_Symbols;
        public List<Symbol> Symbols
        {
            get 
            {
                if (m_Symbols == null)
                    m_Symbols = Symbol.GetSymbols(m_ExchangeID);
                return m_Symbols;
            }
        }
        public Exchange(int exchangeID)
        {
            DataTable dt = Program.GetDataSet("select * from A.Exchange where ID = " + exchangeID.ToString());
            if (dt.Rows.Count == 0)
                throw new Exception(String.Format("Could not found exchange id {0}", exchangeID));
            Initialization((int)dt.Rows[0]["ExchangeID"], dt.Rows[0]["Name"].ToString(), dt.Rows[0]["Description"].ToString());
        }
        public Exchange(int exchangeID, string exchangeName, string description)
        {
            Initialization(exchangeID, exchangeName, description);
        }
        void Initialization(int exchangeID, string exchangeName, string description)
        {
            m_ExchangeID = exchangeID;
            m_ExchangeName = exchangeName;
            m_Description = description;
            m_Symbols = null;
        }
        public override string ToString()
        {
            return m_ExchangeName + " - " + m_Description;
        }
    }
    public class Symbol
    {
        int m_ExchangeID, m_SymbolID;
        string m_SymbolName, m_Description;
        DateTime? m_FromDate = null;
        public DateTime? FromDate
        {
            get { return m_FromDate; }
            set { m_FromDate = value; }
        }
        public int ExchangeID
        {
            get { return m_ExchangeID; }
            set { m_ExchangeID = value; }
        }
        public int SymbolID
        {
            get { return m_SymbolID; }
            set { m_SymbolID = value; }
        }
        public string SymbolName
        {
            get { return m_SymbolName; }
            set { m_SymbolName = value; }
        }
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        Object m_Tag;
        public Object Tag
        {
            get{return m_Tag;}
            set {m_Tag = value;}
        }
        PriceType m_PriceType;
        public PriceType PriceType
        {
            get { return m_PriceType; }
            set 
            {
                bool checkPrice;
                checkPrice = m_PriceType != value;
                m_PriceType = value;
                if(checkPrice)
                    CheckPrice();
            }
        }
        int m_PriceTypeDays;
        public int PriceTypeDays
        {
            get { return m_PriceTypeDays; }
            set 
            {
                bool checkPrice;
                if (value < 1)
                {
                    checkPrice = PriceType == PriceType.Days && m_PriceTypeDays != 1;
                    m_PriceTypeDays = 1;
                }
                else
                {
                    checkPrice = PriceType == PriceType.Days && m_PriceTypeDays != value;
                    m_PriceTypeDays = value;
                }
                if (checkPrice)
                    CheckPrice();
            }
        }

        private void CheckPrice()
        {
            try
            {
                if (PriceType == PriceType.Daily || (PriceType == PriceType.Days && PriceTypeDays == 1))
                {
                    m_Prices = BasePrice;
                    return;
                }
                m_Prices = new List<Price>();
                Price p0 = null;
                int count = 0;
                DateTime firstDay = DateTime.Today;
                for (int i = BasePrice.Count - 1; i >= 0; i--)
                {
                    count++;
                    if (count == 1)
                    {
                        p0 = new Price(BasePrice[i].Date, BasePrice[i].Opening, BasePrice[i].High, BasePrice[i].Low, BasePrice[i].Closing, BasePrice[i].Volume, BasePrice[i].Interest);
                        firstDay = p0.Date;
                        switch (PriceType)
                        {
                            case PriceType.Weekly:
                                firstDay = firstDay.AddDays(-1);
                                firstDay = firstDay.AddDays(-(int)firstDay.DayOfWeek);
                                break;
                            case PriceType.Monthly:
                                firstDay = firstDay.AddDays(-(firstDay.Day - 1));
                                break;
                            case PriceType.Quarterly:
                                if (firstDay.Month >= 1 && firstDay.Month <= 3)
                                    firstDay = new DateTime(firstDay.Year, 1, 1);
                                else if (firstDay.Month >= 4 && firstDay.Month <= 6)
                                    firstDay = new DateTime(firstDay.Year, 4, 1);
                                else if (firstDay.Month >= 7 && firstDay.Month <= 9)
                                    firstDay = new DateTime(firstDay.Year, 7, 1);
                                else
                                    firstDay = new DateTime(firstDay.Year, 10, 1);
                                break;
                            case PriceType.HalfYear:
                                if (firstDay.Month >= 1 && firstDay.Month <= 6)
                                    firstDay = new DateTime(firstDay.Year, 1, 1);
                                else if (firstDay.Month >= 7 && firstDay.Month <= 12)
                                    firstDay = new DateTime(firstDay.Year, 7, 1);
                                break;
                            case PriceType.Yearly:
                                firstDay = firstDay.AddDays(-(firstDay.DayOfYear - 1));
                                break;
                            default:
                                break;
                        }



                        continue;
                    }
                    p0.High = Math.Max(p0.High, BasePrice[i].High);
                    p0.Low = Math.Min(p0.Low, BasePrice[i].Low);
                    p0.Volume = p0.Volume + BasePrice[i].Volume;
                    p0.Opening = BasePrice[i].Opening;
                    p0.Date = BasePrice[i].Date;
                    if (PriceType == PriceType.Days && count == PriceTypeDays)
                    {
                        m_Prices.Add(p0);
                        count = 0;
                    }
                    else if (PriceType != PriceType.Days && i > 0)
                    {
                        if (BasePrice[i - 1].Date < firstDay)
                        {
                            m_Prices.Add(p0);
                            count = 0;
                        }
                    }

                }
                if (count > 0)
                    m_Prices.Add(p0);
                m_Prices.Reverse();
            }
            finally
            {
                values = new List<List<double>>();
                values.Add(new List<double>()); values.Add(new List<double>()); values.Add(new List<double>());
                values.Add(new List<double>()); values.Add(new List<double>()); values.Add(new List<double>());
                values.Add(new List<double>()); values.Add(new List<double>()); values.Add(new List<double>());

                objects = new List<List<object>>();
                objects.Add(new List<object>()); objects.Add(new List<object>()); objects.Add(new List<object>());
                objects.Add(new List<object>()); objects.Add(new List<object>()); objects.Add(new List<object>());
                objects.Add(new List<object>()); objects.Add(new List<object>()); objects.Add(new List<object>());
                objects.Add(new List<object>());

                foreach (var x in this.Prices)
                {
                    values[0].Add(x.Opening);
                    values[1].Add(x.High);
                    values[2].Add(x.Low);
                    values[3].Add(x.Closing);
                    values[4].Add(x.Volume);
                    values[5].Add(x.Interest);
                    values[6].Add(x.TypicalClosing);
                    values[7].Add(x.WeightedClosing);
                    values[8].Add(x.MedianPrice);

                    objects[0].Add(x.Opening);
                    objects[1].Add(x.High);
                    objects[2].Add(x.Low);
                    objects[3].Add(x.Closing);
                    objects[4].Add(x.Volume);
                    objects[5].Add(x.Interest);
                    objects[6].Add(x.TypicalClosing);
                    objects[7].Add(x.WeightedClosing);
                    objects[8].Add(x.MedianPrice);
                    objects[9].Add(x.Date);
                }
            }
        }
        List<Price> m_BasePrice;
        public List<Price> BasePrice
        {
            get 
            {
                if (m_BasePrice == null)
                {
                    if (m_FromDate == null)
                        m_BasePrice = Price.GetPrices(m_SymbolID);
                    else
                        m_BasePrice = Price.GetPrices(m_SymbolID, m_FromDate.Value);
                }
                return m_BasePrice; 
            }
        }
        List<Price> m_Prices;
        public List<Price> Prices
        {
            get 
            {
                if (m_Prices == null)
                    CheckPrice();
                return m_Prices; 
            }
        }
        private List<List<double>> values = null;
        private List<List<object>> objects = null;
        public List<double> GetValues(PriceName name )
        {
            if (values == null)
                CheckPrice();
            return values[(int)(name)];
            
        }
        public List<object> GetObjects(PriceName name)
        {
            if (objects == null)
                CheckPrice();
            return objects[(int)(name)];
        }
        public List<DateTime> GetDates()
        {
            return Prices.Select(x => { return x.Date; }).ToList<DateTime>();
        }
        public static List<Symbol> GetSymbols(int exchangeID)
        {
            return Program.GetDataSet("select * from A.Symbol where ExchangeID = " + exchangeID.ToString()).AsEnumerable().Select(x => { return new Symbol(x.Field<int>("ExchangeID"), x.Field<int>("ID"), x.Field<string>("Name"), x.Field<string>("Description")); }).ToList<Symbol>();
        }
        public static List<Symbol> GetSymbol(string symbolName)
        {
            return Program.GetDataSet("select * from A.Symbol where Name = " + symbolName.ToSQLString()).AsEnumerable().Select(x => { return new Symbol(x.Field<int>("ExchangeID"), x.Field<int>("ID"), x.Field<string>("Name"), x.Field<string>("Description")); }).ToList<Symbol>();
        }
        public Symbol(int symbolID)
        {
            DataTable dt = Program.GetDataSet("select * from A.Symbol where ID = "+symbolID.ToString());
            if (dt.Rows.Count == 0)
                throw new Exception(String.Format("Could not found symbol id {0}", symbolID));
            Initialization((int)dt.Rows[0]["ExchangeID"], symbolID, dt.Rows[0]["Name"].ToString(), dt.Rows[0]["Description"].ToString());
        }
        public Symbol(int exchangeID, int symbolID, string symbolName, string description)
        {
            Initialization(exchangeID, symbolID, symbolName, description);
        }
        void Initialization(int exchangeID, int symbolID, string symbolName, string description)
        {
            m_ExchangeID = exchangeID;
            m_SymbolID = symbolID;
            m_SymbolName = symbolName;
            m_Description = description;
            m_Prices = null;
            m_PriceType = PriceType.Daily;
            m_PriceTypeDays = 1;
        }
        public override string ToString()
        {
            return m_SymbolName + " - " + m_Description;
        }
        public void ReleasePriceList()
        {
            if (m_Prices != null)
            {
                m_Prices.Clear();
                m_Prices = null;
            }
            if (m_BasePrice != null)
            {
                m_BasePrice.Clear();
                m_BasePrice = null;
            }
        }
    }
    public class Price 
    {
        DateTime m_Date;
        int m_Seq;
        double m_Opening, m_High, m_Low, m_Closing, m_Volume, m_Interest, m_TypicalClosing, m_WeightedClosing, m_MedianPrice;
        public DateTime Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }
        public int Seq
        {
            get { return m_Seq; }
            set { m_Seq = value; }
        }
        public double Opening
        {
            get { return m_Opening; }
            set { m_Opening = value; ReCalc(); }
        }
        public double High
        {
            get { return m_High; }
            set { m_High = value; ReCalc(); }

        }
        public double Low
        {
            get { return m_Low; }
            set { m_Low = value; ReCalc(); }
        }
        public double Closing
        {
            get { return m_Closing; }
            set { m_Closing = value; ReCalc(); }
        }
        public double Volume
        {
            get { return m_Volume; }
            set { m_Volume = value; }
        }
        public double Interest
        {
            get { return m_Interest; }
            set { m_Interest = value; }
        }
        public double TypicalClosing
        {
            get { return m_TypicalClosing; }
        }
        public double WeightedClosing
        {
            get { return m_WeightedClosing; }
        }
        public double MedianPrice
        {
            get { return m_MedianPrice; }
        }
        void ReCalc()
        {
            m_TypicalClosing = (m_High + m_Low + m_Closing) / 3.0;
            m_WeightedClosing = (m_High + m_Low + m_Closing * 2) / 4.0;
            m_MedianPrice = (m_High + m_Low) / 2.0;
        }
        Object m_Tag;
        public Object Tag
        {
            get { return m_Tag; }
            set { m_Tag = value; }
        }
        public Price()
        {
            m_Opening = 0; 
            m_High = 0; 
            m_Low = 0; 
            m_Closing = 0; 
            m_Volume = 0; 
            m_Interest = 0; 
            m_TypicalClosing = 0; 
            m_WeightedClosing = 0;
            m_MedianPrice = 0;
        }
        public Price(DateTime date, double opening, double high, double low, double closing, double volume, double Interest)
        {
            m_Date = date;
            m_Opening = opening;
            m_High = high;
            m_Low = low;
            m_Closing = closing;
            m_Volume = volume;
            m_Interest = Interest;
            ReCalc();
        }
        public static List<Price> GetPrices(int symbolID)
        {
            try
            {
                return Program.GetDataSet("select * from A.GetSecurity(" + symbolID.ToString() + ", default) order by Date").AsEnumerable().Select(x => { return new Price(x.Field<DateTime>("Date"), x.Field<double>("Opening"), x.Field<double>("High"), x.Field<double>("Low"), x.Field<double>("Closing"), x.Field<double>("Volume"), x.Field<double>("Interest")); }).ToList<Price>();
            }
            catch
            {
                return new List<Price>();
            }
        }

        public static List<Price> GetPrices(int symbolID, DateTime fromDate)
        {
            try
            {
                return Program.GetDataSet("select * from A.GetSecurity(" + symbolID.ToString() + ", "+fromDate.ToString("yyyy-MM-dd").ToSQLString()+") order by Date").AsEnumerable().Select(x => { return new Price(x.Field<DateTime>("Date"), x.Field<double>("Opening"), x.Field<double>("High"), x.Field<double>("Low"), x.Field<double>("Closing"), x.Field<double>("Volume"), x.Field<double>("Interest")); }).ToList<Price>();
            }
            catch
            {
                return new List<Price>();
            }
        }

        /*public static List<Price> GetPrice(string symbolName)
        {
            
            List<Price> ret = new List<Price>();
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter("select * from A.GetSecurity((select top 1 ID from A.Symbol where Name =" + symbolName.ToSQLString() + "), default) order by Date", Program.ConnectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow r in dt.Rows)
                ret.Add(new Price((DateTime)(r["Date"]), (double)r["Opening"], (double)r["High"], (double)r["Low"], (double)r["Closing"], (double)r["Volume"], (double)r["Interest"]));
            return ret;
        }*/
    }
    public static class Analysis
    {
        #region Member of List<double>
        public static List<double> Multiply(this List<double> o, double n)
        {
            return o.Select(x => { return x * n; }).ToList<double>();
        }
        public static List<double> Multiply(this List<double> o, List<double> o1)
        {
            return o.Select((x, index) => { return x * o1[index]; }).ToList<double>();
        }
        public static List<double> Subtract(this List<double> o, double n)
        {
            return o.Select(x => { return x - n; }).ToList<double>();
        }
        public static List<double> Subtract(this List<double> o, List<double> o1)
        {
            return o.Select((x, index) => { return x - o1[index]; }).ToList<double>();
        }
        public static List<double> Addit(this List<double> o, double n)
        {
            return o.Select(x => { return x + n; }).ToList<double>();
        }
        public static List<double> Addit(this List<double> o, List<double> o1)
        {
            return o.Select((x, index) => { return x + o1[index]; }).ToList<double>();
        }
        public static List<double> Divide(this List<double> o, double n)
        {
            return o.Select(x => { return n==0 ? 0 : x / n; }).ToList<double>();
        }
        public static List<double> Divide(this List<double> o, List<double> o1)
        {
            return o.Select((x, index) => { return o1[index] ==0? 0 : x / o1[index]; }).ToList<double>();
        }
        #endregion

        #region Basic Calculation
        public static List<double> Gain(this List<double> data)
        {
            return data.Select((x, index) => {
                if (index > 0)
                {
                    if (data[index - 1] < x)
                        return x - data[index - 1];
                }
                return 0d;
            }).ToList<double>();
        }
        public static List<double> Loss(this List<double> data)
        {
            return data.Select((x, index) =>
            {
                if (index > 0)
                {
                    if (data[index - 1] > x)
                        return data[index - 1] - x ;
                }
                return 0d;
            }).ToList<double>();
        }
        public static List<double> Lowest(this List<double> data, int days)
        {
            return data.Select((x, index) => 
            {
                for (int i = index; i > index - days ; i--)
                {
                    if (i < 0)
                        break;
                    x = Math.Min(x,data[i]);
                }
                return x;
            }).ToList<double>();
        }
        public static List<double> Highest(this List<double> data, int days)
        {
            return data.Select((x, index) =>
            {
                for (int i = index; i > index - days; i--)
                {
                    if (i < 0)
                        break;
                    x = Math.Max(x, data[i]);
                }
                return x;
            }).ToList<double>();
        }
        public static List<double> NPeriodAgo(this List<double> data, int days)
        {
            return data.Select((x, index) =>
            {
                if (index < days)
                    return data[0];
                return data[index - days];
            }).ToList<double>();
        }
        public static List<double> Sum(this List<double> data, int days)
        {
            double sum;
            sum = 0;
            return data.Select((x, index) =>
            {
                if (index < days)
                    sum += x;
                else
                    sum += x - data[index - days];
                return sum;
            }).ToList<double>();
        }
        #endregion

        #region Moving Average && MACD
        public static List<double> SimpleMovingAverage(this List<double> data, int days)
        {
            if( days < 2)
                throw new Exception("Invalid parameter days = " + days.ToString() + ".");
            List<double> ret = new List<double>();
            double d = days;
            for (int i = 0; i < data.Count; i++)
            {
                if (i < d)
                {
                    if (i == 0)
                        ret.Add(data[i] / d);
                    else
                        ret.Add(ret[i - 1] + (data[i] / d));
                }
                else
                    ret.Add(ret[i - 1] - (data[i - days] / d) + (data[i] / d));
            }
            for (int i = 0; i < days; i++)
            {
                if(i < data.Count)
                    ret[i] = data[i];
            }
            return ret;
        }
        public static List<double> ExponentialMovingAverage(this List<double> data, int days)
        {
            if (days < 2)
                throw new Exception("Invalid parameter days = " + days.ToString() + ".");
            double alpha = 2 / ((double)days + 1d);
            List<double> ret = new List<double>();
            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                    ret.Add(data[i]);
                else
                    ret.Add(alpha * data[i]+(1d - alpha)*ret[i-1]);
            }
            return ret;
        }
        public static List<double> WeightedmovingAverage(this List<double> data, int days)
        {
            if (days < 2)
                throw new Exception("Invalid parameter days = " + days.ToString() + ".");
            List<double>  ret = new List<double>();
            double numerator = 0, denominator = ((double)days*(days+1))/2d, total = 0;
            for (int i = 0; i < data.Count; i++)
            {
                numerator = numerator + ((double)days) * data[i] - total;
                total = total + data[i];
                if (i - days >= 0)
                    total = total - data[i - days];
                ret.Add(numerator / denominator);
            }
            return ret;
        }
        public static List<double> VariableMovingAverage(this List<double> data, int days)
        {
            if (days < 2)
                throw new Exception("Invalid parameter days = " + days.ToString() + ".");
            
            List<double> ret = new List<double>();
            List<double> gain = new List<double>();
            List<double> loss = new List<double>();
            double totalGain = 0, totalLoss = 0;
            double alpha = 2 / ((double)days + 1d);
            for (int i = 0; i < data.Count; i++)
            {

                if (i == 0)
                {
                    gain.Add(0d);
                    loss.Add(0d);
                    ret.Add(data[i]);
                }
                else
                {
                    alpha = data[i] - data[i - 1];
                    if (alpha > 0)
                        gain.Add(alpha);
                    else
                        gain.Add(0);
                    if (alpha < 0)
                        loss.Add(-alpha);
                    else
                        loss.Add(0);
                    totalGain = totalGain + gain[i];
                    totalLoss = totalLoss + loss[i];
                    if (i - days >= 0)
                    {
                        totalGain = totalGain - gain[i - days];
                        totalLoss = totalLoss - loss[i - days];
                    }
                    if (totalGain + totalLoss == 0)
                        alpha = 0;
                    else
                        alpha = (2d * Math.Abs((totalGain - totalLoss) / (totalGain + totalLoss))) / ((double)days + 1d);
                    ret.Add(alpha * data[i] + (1d - alpha) * ret[i - 1]);
                }
            }
            return ret;
        }
        public static List<double> WildersSmoothing(this List<double> data, List<double> ma, int days)
        {
            List<double> ret = new List<double>();
            for (int i = 0; i < data.Count; i++)
            {
                if (i == 0)
                    ret.Add(data[i]);
                else
                {
                    ret.Add(ma[i - 1] + (data[i] - ma[i - 1]) / (double)days);
                }
            }
            return ret;
        }
        /*[0]MACD [1]Signal*/
        public static List<List<double>> MACD(this List<double> data, int periodLong, int periodShort, int periodSignal)
        {
            List<List<double>> ret = new List<List<double>>();
            ret.Add(data.ExponentialMovingAverage(periodShort).Subtract(data.ExponentialMovingAverage(periodLong)));
            ret.Add(ret[0].ExponentialMovingAverage(periodSignal));
            return ret;
        }
        /*[0]MACD [1]Signal*/
        public static List<List<double>> MACD(this List<double> periodLong, List<double> periodShort, int periodSignal)
        {
            List<List<double>> ret = new List<List<double>>();
            ret.Add(periodShort.Subtract(periodLong));
            ret.Add(ret[0].ExponentialMovingAverage(periodSignal));
            return ret;
        }
        #endregion

        #region BollingerBands and Moving Standard Deviation
        /*Moving Standard Deviation*/
        public static List<double> MovingStandardDeviation(this List<double> data, List<double> ma, int days)
        {
            if (days < 2)
                throw new Exception("Invalid parameter days = " + days.ToString() + ".");
            List<double> ret = new List<double>();
            double x, y;
            for (int i = 0; i < data.Count; i++)
            {
                x = 0;
                y = 0;
                for (int j = 0; j < days; j++)
                {
                    if (i - j >= 0)
                    {
                        x = x + Math.Pow(data[i - j] - ma[i], 2);
                        y++;
                    }
                }
                ret.Add(Math.Pow(x/y, 0.5));
            }
            return ret;
        }
        /*[0]upper [1]low*/
        public static List<List<double>> BollingerBands(this List<double> data, List<double> middleBand, int days, double delta)
        {
            if (days < 2)
                throw new Exception("Invalid parameter days = " + days.ToString() + ".");
            List<List<double>> ret = new List<List<double>>();
            List<double> d = MovingStandardDeviation(data, middleBand, days).Multiply(delta);
            ret.Add(middleBand.Addit(d));
            ret.Add(middleBand.Subtract(d));
            return ret;
        }
        #endregion 
        
        public static List<double> ChandeMomentumOscillator(this List<double> data, int days)
        {
            if (days < 2)
                throw new Exception("Invalid parameter days = " + days.ToString() + ".");

            List<double> ret = new List<double>();
            List<double> gain = new List<double>();
            List<double> loss = new List<double>();
            double totalGain = 0, totalLoss = 0;
            double alpha = 2 / ((double)days + 1d);
            for (int i = 0; i < data.Count; i++)
            {

                if (i == 0)
                {
                    gain.Add(0d);
                    loss.Add(0d);
                    ret.Add(data[i]);
                }
                else
                {
                    alpha = data[i] - data[i - 1];
                    if (alpha > 0)
                        gain.Add(alpha);
                    else
                        gain.Add(0);
                    if (alpha < 0)
                        loss.Add(-alpha);
                    else
                        loss.Add(0);
                    totalGain = totalGain + gain[i];
                    totalLoss = totalLoss + loss[i];
                    if (i - days >= 0)
                    {
                        totalGain = totalGain - gain[i - days];
                        totalLoss = totalLoss - loss[i - days];
                    }
                    if (totalGain + totalLoss == 0)
                        ret.Add(0);
                    else
                        ret.Add(100d * (totalGain - totalLoss) / (totalGain + totalLoss));
                }
            }
            return ret;
        }
        /*
         * Parameters could be gain of Prices or Volumes.
         */
        public static List<double> RelativeStrengthIndex (this List<double> avgGain, List<double> avgLoss)
        {
            return avgGain.Select((x, index) => 
            {
                if (avgLoss[index] == 0)
                    return 100d;
                else
                    return 100d - (100d / (1d + (x / avgLoss[index])));
            }).ToList<double>();
        }

        public static List<double> StochasticOscillator(this List<double> data, List<double>lowestLow, List<double> highestHigh)
        {
            return data.Select((x, index) => 
            {
                if ((highestHigh[index] == lowestLow[index]))
                    return 0d;
                return (x - lowestLow[index]) * 100d / (highestHigh[index] - lowestLow[index]);
            }).ToList<double>();
        }
        /*
         * (Price) Rate-Of-Change. 
         * parameter can also be 
         * 1. moving agerage of long period close/volume and short period close/volume
         * 2. today's price/volume and N days' ago's price/volume
         */
        public static List<double> RateOfChange(this List<double> maShort, List<double> maLong)
        {
            return maLong.Select((x, index) => 
            {
                if (x == 0 || maLong[index] == 0)
                    return 0d;
                return (maShort[index] - maLong[index]) * 100d / maLong[index];
            }).ToList<double>();
        }
        /*Typical Value 7,14,28*/
        public static List<double> UltimateOscillator(this List<Price> prices, int period1, int period2, int period3)
        {
            if (!(period3 > period2 && period2 > period1))
                throw new Exception("Wrong parameter.");
            
            List<double> buyingPressure = new List<double>();
            List<double> trueRange1 = new List<double>();
            List<double> trueRange2 = new List<double>();
            List<double> trueRange3 = new List<double>();
            double trueLow = 0d, previousClosing = 0d ;
            double sumTrueRange1 = 0, sumTrueRange2 = 0, sumTrueRange3 = 0;
            double a, b, c;
            double sumBuyingPressure1 = 0, sumBuyingPressure2 = 0, sumBuyingPressure3 = 0;
            return prices.Select((x, index) =>
            {
                if (index == 0)
                {
                    trueLow = x.Low;
                    previousClosing = x.Closing;
                }
                else
                {
                    previousClosing = prices[index - 1].Closing;
                    trueLow = Math.Min(x.Closing, previousClosing);
                }
                buyingPressure.Add(x.Closing - trueLow);
                trueRange1.Add(x.High - x.Low);
                trueRange2.Add(x.High - previousClosing);
                trueRange3.Add(previousClosing - x.Low);

                sumTrueRange1 = sumTrueRange1 + trueRange1[index];
                sumTrueRange2 = sumTrueRange2 + trueRange2[index];
                sumTrueRange3 = sumTrueRange3 + trueRange3[index];

                sumBuyingPressure1 = sumBuyingPressure1 + buyingPressure[index];
                sumBuyingPressure2 = sumBuyingPressure2 + buyingPressure[index];
                sumBuyingPressure3 = sumBuyingPressure3 + buyingPressure[index];
                if (index - period1 >= 0)
                {
                    sumBuyingPressure1 = sumBuyingPressure1 - buyingPressure[index - period1];
                    sumTrueRange1 = sumTrueRange1 - trueRange1[index - period1];
                }
                if (index - period2 >= 0)
                {
                    sumBuyingPressure2 = sumBuyingPressure2 - buyingPressure[index - period2];
                    sumTrueRange2 = sumTrueRange2 - trueRange2[index - period2];
                }
                if (index - period3 >= 0)
                {
                    sumBuyingPressure3 = sumBuyingPressure3 - buyingPressure[index - period3];
                    sumTrueRange3 = sumTrueRange3 - trueRange3[index - period3];
                }
                a = sumTrueRange1 == 0 ? 0 : sumBuyingPressure1 * 4d / sumTrueRange1;
                b = sumTrueRange2 == 0 ? 0 : sumBuyingPressure2 * 2d / sumTrueRange2;
                c = sumTrueRange3 == 0 ? 0 : sumBuyingPressure3 * 1d / sumTrueRange3;
                a = (a + b + c)*100 / 7d;
                a = a > 100 ? 100d : a;
                a = a < 0 ? 0d : a;
                return a;
            }).ToList<double>();
        }

        /*
         * parateter can be 
         * data: typical price
         * ma: Moving Average of Typical Price
         * days: period
         */
        public static List<double> CommodityChannelIndex(this List<double> data, List<double> ma, int days)
        {
            return data.Select((x, i) =>
            {
                int j = i - days + 1 >= 0 ? i - days + 1 : 0;
                double y = 0;
                for (int k = j; k <= i; k++)
                    y = y + Math.Abs(ma[k] - x);
                if(y!=0)
                    return (x - ma[i])*(double)days / (0.015d * y);
                return 0;
            }).ToList<double>();
        }

    }
}