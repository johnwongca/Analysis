using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
namespace Trade
{

    /*
    public class Price
    {
        bool needRecalc = true;
        public DateTime mDate;
        public double mOpening, mHigh, mLow, mClosing, mVolume, mTypicalClosing, mWeightedClosing, mMedianPrice;
        public DateTime Date
        {
            get { return mDate; }
            set { mDate = value; }
        }
        public double Opening
        {
            get { 
                    return mOpening; 
                }
            set { 
                    needRecalc = true; 
                    mOpening = value; 
                }
        }
        public double High
        {
            get { 
                    return mHigh; 
                }
            set {
                    needRecalc = true;
                    mHigh = value; 
                }
        }
        public double Low
        {
            get { 
                    return mLow; 
                }
            set {
                    needRecalc = true;
                    mLow = value; 
                }
        }
        public double Closing
        {
            get { 
                    return mClosing; 
                }
            set {
                    needRecalc = true;
                    mClosing = value; 
                }
        }
        public double Volume
        {
            get { 
                    return mVolume; 
                }
            set { 
                    mVolume = value; 
                }
        }
        public double TypicalClosing
        {
            get { 
                    return mTypicalClosing; 
                }
        }
        public double WeightedClosing
        {
            get { 
                    return mWeightedClosing; 
                }
        }
        public double MedianPrice
        {
            get { 
                    return mMedianPrice; 
                }
        }
        public void ReCalc()
        {
            if (!needRecalc) return;
            mTypicalClosing = (mHigh + mLow + mClosing) / 3.0;
            mWeightedClosing = (mHigh + mLow + mClosing * 2) / 4.0;
            mMedianPrice = (mHigh + mLow) / 2.0;
            needRecalc = false;
        }
        public Price(DateTime date, double opening, double high, double low, double closing, double volume)
        {
            mDate = date;
            mOpening = opening;
            mHigh = high;
            mLow = low;
            mClosing = closing;
            mVolume = volume;
            mTypicalClosing = 0;
            mWeightedClosing = 0;
            mMedianPrice = 0;
            ReCalc();
        }
        public static List<Price> ReadData(string SQL)
        {
            List<Price> ret = new List<Price>();
            SqlCommand cmd = Global.GetCommand();
            cmd.Connection.Open();
            cmd.CommandText = SQL;
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
                ret.Add(new Price((DateTime)r["Date"], (double)r["Opening"], (double)r["High"], (double)r["Low"], (double)r["Closing"], (double)r["Volume"]));
            r.Close();
            cmd.Connection.Close();
            return ret;
        }
    }
    public enum BuySell { Buy, Sell, None}
    public class Analysis
    {
        public int FromPeriod, ToPeriod;
        public int Periods
        {
            get { return ToPeriod - FromPeriod + 1; }
        }
        public Analysis(int fromPeriod, int toPeriod)
        {
            FromPeriod = fromPeriod;
            ToPeriod = toPeriod;
        }
        public Analysis()
            : this(3, 50)
        {
        }
        public static double ExponentialAverage(double[] data, int period)
        {
            double alpha = 2.00000 / (Convert.ToDouble(period) + 1.00000);
            if(alpha == 1)
                return data[data.Length - 1];
            int back = Convert.ToInt32(Math.Log(0.001 * alpha) / Math.Log(1.000 - alpha));
            double ret = 0;
            for (int i = (data.Length - back -1 >=0 ? data.Length - back -1 : 0);i < data.Length; i++)
            {
                if (i == 0)
                {
                    ret = data[i];
                    continue;
                }
                ret = alpha * data[i] + (1 - alpha) * ret;
            }
            return ret;
        }
        public static double WeightedAverage(double[] data)
        {
            double numerator = 0, denominator = 0;
            for (int i = 1; i < data.Length + 1; i++)
            {
                numerator = numerator + i;
                denominator = data[i - 1] * i;
            }
            return numerator == 0 ? 0 : denominator / numerator;
        }
    }
    public class ExpMA : Analysis
    {
        public double[] Data;
        public double[][] MA;
        DataTable mMATable;
        public DataTable MATable
        {
            get
            {
                if (mMATable != null)
                    return mMATable;
                if (MA == null)
                    return null;
                mMATable = Global.ArrayToDataTable(MA);
                return mMATable;
            }
        }
        public double[][] Calculate()
        {
            mMATable = null;
            double[] alpha = new double[Periods];
            double[][] ma = new double[Data.Length][];
            for (int i = 0; i < Periods; i++)
                alpha[i] = 2.00000 / (Convert.ToDouble(i + FromPeriod) + 1.00000);
            for (int i = 0; i < Data.Length; i++)
            {
                ma[i] = new double[alpha.Length];
                for (int a = 0; a < alpha.Length; a++)
                {
                    if (i == 0)
                    {
                        ma[i][a] = Data[i];
                        continue;
                    }
                    ma[i][a] = alpha[a] * Data[i] + (1 - alpha[a]) * ma[i - 1][a];
                }
            }
            MA = ma;
            return ma;
        }
        public ExpMA(double[] data)
            : base()
        {
            Data = data;
            mMATable = null;
            MA = null;
        }
        public ExpMA(double[] data, int fromPeriod, int toPeriod)
            : this(data)
        {
            FromPeriod = fromPeriod;
            ToPeriod = toPeriod;
            Calculate();
        }
    }
    public class MACDValue
    {
        double[] mPrice, mShortMA, mLongMA, mDiff, mSignalMA, mDiffSignal, mBuy, mSell;
        int mShortPeriod, mLongPeriod, mSignalPeriod;
        public double RSI;
        public double TotalTrade;
        public double[] Price { get { return mPrice; } }
        public double[] ShortMA { get { return mShortMA; } }
        public double[] LongMA { get { return mLongMA; } }
        public double[] Diff { get { return mDiff; } }
        public double[] SignalMA { get { return mSignalMA; } }
        public double[] DiffSignal { get { return mDiffSignal; } }
        public double[] Buy { get { return mBuy; } }
        public double[] Sell { get { return mSell; } }
        public int Count { get { return Price.Length; } }
        public int ShortPeriod { get { return mShortPeriod; } }
        public int LongPeriod { get { return mLongPeriod; } }
        public int SignalPeriod { get { return mSignalPeriod; } }
        void CalculateQuality()
        {
            //calculate RSI
            bool buying = false, selling = false, bought = false;
            double sell = 0, buy = 0, sellCount = 0, buyCount = 0, avgBuy = -1, avgSell = -1;
            double totalGain = 0, totalLoss = 0, countTrade = 0;
            for (int i = Count - 50 > 0 ? Count - 50 : 0; i < Count; i++)
            {
                if (mBuy[i] > 0)
                {
                    if (selling)
                    {
                        selling = false;
                        avgSell = sell / sellCount;
                        sell = 0;
                        sellCount = 0;
                    }
                    buy = buy + mBuy[i];
                    buyCount++;
                    buying = true;
                    bought = true;
                }
                if (mSell[i] > 0)
                {
                    if (!bought)
                        continue;
                    if (buying)
                    {
                        buying = false;
                        avgBuy = buy / buyCount;
                        buy = 0;
                        buyCount = 0;
                    }
                    sell = sell + mSell[i];
                    sellCount++;
                    selling = true;
                }
                if ((avgSell != -1) && (avgBuy != -1))
                {
                    countTrade++;
                    if (avgSell > avgBuy)
                        totalGain = totalGain + avgSell - avgBuy;
                    if (avgSell < avgBuy)
                        totalLoss = totalLoss + avgBuy - avgSell;
                    avgSell = -1; avgBuy = -1;
                }
            }
            if (sellCount != 0)
                avgSell = sell / sellCount;
            if ((avgSell != -1) && (avgBuy != -1))
            {
                countTrade++;
                if (avgSell > avgBuy)
                    totalGain = totalGain + avgSell - avgBuy;
                if (sell < buy)
                    totalLoss = totalLoss + avgBuy - avgSell;
                avgSell = -1; avgBuy = -1;
            }
            TotalTrade = countTrade;
            RSI = 0;
            if (countTrade > 0 || totalGain > 0)
            {
                if (totalLoss <= 0)
                {
                    RSI = 100;
                    return;
                }
                RSI = 100.0000 - (100.0000 / (1.0000 + ((totalGain / countTrade) / (totalLoss / countTrade))));
            }
        }
        public MACDValue(double[] price, double[] shortMA, double[] longMA, int shortPeriod, int longPeriod, int signalPeriod)
        {

            mPrice = price;
            mShortMA = shortMA;
            mLongMA = longMA;
            mDiff = new double[Count];
            mSignalMA = new double[Count];
            mDiffSignal = new double[Count];
            mBuy = new double[Count];
            mSell = new double[Count];
            for (int i = 0; i < shortMA.Length; i++)
                mDiff[i] = shortMA[i] - longMA[i];
            ExpMA ma = new ExpMA(mDiff, signalPeriod, signalPeriod);
            for (int i = 0; i < Count; i++)
            {
                mBuy[i] = 0;
                mSell[i] = 0;
                mSignalMA[i] = ma.MA[i][0];
                mDiffSignal[i] = mDiff[i] - mSignalMA[i];
                if (i >= 1)
                {
                    if (mDiff[i] < 0 && mDiffSignal[i] > 0 && mDiffSignal[i - 1] < 0)
                        mBuy[i] = mPrice[i];
                    if (mDiff[i] > 0 && mDiffSignal[i] < 0 && mDiffSignal[i - 1] > 0)
                        mSell[i] = mPrice[i];
                }
            }
            ma = null;
            mShortPeriod = shortPeriod;
            mLongPeriod = longPeriod;
            mSignalPeriod = signalPeriod;
            CalculateQuality();
        }
    }
    */
    /*MACDRSIValue
    public class MACDRSIValue
    {
        
        public MACDValue Buy, Sell;
        public double RSI;
        public double TotalTrade;
        public void Calculate()
        {
            double sell = -1, buy = -1;
            double totalGain = 0, totalLoss = 0, countTrade = 0;
            bool canBuy = true, canSell = false;
            for (int i = Buy.Count - 100 > 0 ? Buy.Count - 100 : 0; i < Buy.Count; i++)
            {
                if (canBuy && Buy.Buy[i] > 0)
                {
                    buy = Buy.Buy[i];
                    canBuy = false;
                    canSell = true;
                }
                if (canSell && Sell.Sell[i] > 0)
                {
                    sell = Sell.Sell[i];
                    canBuy = true;
                    canSell = false;
                }
                if ((sell != -1) && (buy != -1))
                {
                    countTrade++;
                    if(sell > buy)
                        totalGain = totalGain + sell - buy;
                    if (sell < buy)
                        totalLoss = totalLoss + buy - sell;
                    buy = -1; sell = -1;
                }
            }
            TotalTrade = countTrade;
            RSI = 0;
            if (countTrade > 0 || totalGain > 0)
            {
                if (totalLoss <= 0)
                {
                    RSI = 100;
                    return;
                }
                RSI = 100.0000 - (100.0000 / (1.0000 + ((totalGain / countTrade) / (totalLoss / countTrade))));
            }
        }
        public MACDRSIValue(MACDValue buy, MACDValue sell)
        {
            Buy = buy;
            Sell = sell;
            Calculate();
        }
    }*/
    /*
    public class MACDValueComparer : IComparer<MACDValue>
    {
        int mOrder;
        public int Order
        {
            get { return mOrder == 1 ? 1: -1; }
        }
        int comp(MACDValue x, MACDValue y)
        {
            if (x == null && y == null)
                return 0;
            if (x != null && y == null)
                return 1;
            if (x == null && y != null)
                return -1;
            if (x.TotalTrade > y.TotalTrade) return 1;
            if (x.TotalTrade == y.TotalTrade) return 0;
            if (x.TotalTrade < y.TotalTrade) return -1;
            //if (x.RSI > y.RSI) return 1;
            //if (x.RSI == y.RSI) return 0;
            //if (x.RSI < y.RSI) return -1;
            return 0;
        }
        public MACDValueComparer(int order)
        {
            mOrder = order == 1 ? 1 : -1;
        }
        public int Compare(MACDValue x, MACDValue y)
        {
            return comp(x, y) * mOrder;
        }
    }
    public class MACD : Analysis
    {
        public double[] Data;
        public int SignalForm, SignalTo;
        public int SignalPeriods
        {
            get { return SignalTo - SignalForm + 1; }
        }
        //List<MACDRSIValue> mResult;
        //public List<MACDRSIValue> Result { get { return mResult; } }
        public ExpMA MA = null;
        public MACD(double[] data)
            : base()
        {
            Data = data;
            SignalForm = 3;
            SignalTo = 9;
            //mResult = new List<MACDRSIValue>();
        }
        public MACD(double[] data, int fromPeriod, int toPeriod)
            : this(data)
        {
            FromPeriod = fromPeriod;
            ToPeriod = toPeriod;
        }
        public MACD(double[] data, int fromPeriod, int toPeriod, int signalFrom, int signalTo)
            : this(data, fromPeriod, toPeriod)
        {
            FromPeriod = fromPeriod;
            ToPeriod = toPeriod;
            SignalForm = signalFrom;
            SignalTo = signalTo;
            Calulate();
        }
        public void Calulate()
        {
            List<MACDValue> temp = new List<MACDValue>();
            int next, x, y;
            this.MA = new ExpMA(Data, FromPeriod, ToPeriod);
            temp.Clear();
            for (int i = SignalForm; i <= SignalTo; i++)
            {
                for (int j = FromPeriod; j <= ToPeriod; j++)
                {
                    next = Convert.ToInt32(Convert.ToDouble(j) * 2.000);
                    if (next > ToPeriod)
                        break;
                    while (true)
                    {
                        if (next > ToPeriod)
                            break;
                        double[] shortMA = new double[Data.Length];
                        double[] longMA = new double[Data.Length];
                        x = j - FromPeriod;
                        y = next - FromPeriod;
                        for (int k = 0; k < MA.MA.Length; k++)
                        {
                            shortMA[k] = MA.MA[k][x];
                            longMA[k] = MA.MA[k][y];
                        }
                        MACDValue v = new MACDValue(Data, shortMA, longMA, j, next, i);
                        if(v.TotalTrade > 1)
                            temp.Add(v);
                        v = null;
                        next++;
                    }
                }
            }
            /*MACDRSIValue v;
            MACDRSIValueComparer comp = new MACDRSIValueComparer(-1);
            int max = 100; 
            mResult.Clear();
            for (int i = 0; i < temp.Count; i++) //buy
            {
                for (int j = 0; j < temp.Count; j++) //sell
                {
                    v = new MACDRSIValue(temp[i], temp[j]);
                    if(v.TotalTrade > 1 && v.RSI > 80)
                        mResult.Add(v);
                    if (mResult.Count > max*2)
                    {
                        mResult.Sort(comp);
                        mResult.RemoveRange(max, mResult.Count - max);
                    }
                    v = null;
                }
            }
            if (mResult.Count > max)
            {
                mResult.Sort(comp);
                mResult.RemoveRange(max, mResult.Count - max);
            }
            temp.Sort(new MACDValueComparer(-1));
            temp.Clear();
            temp = null;
            GC.Collect();
        }
    }
    */
}