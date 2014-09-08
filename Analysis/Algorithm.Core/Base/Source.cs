using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public enum IntervalType { Minutes = 1, Days = 2, Weeks = 3, Months = 4, Years = 5 }
    public class Source : ISource
    {
        class RowData
        {
            public Window<DateTime> DateFrom = Window<DateTime>.NewVariable(), DateTo = Window<DateTime>.NewVariable();
            public Window<double> Open = Window<double>.NewVariable(), Low = Window<double>.NewVariable(), High = Window<double>.NewVariable(), Close = Window<double>.NewVariable();
            public Window<double> Volume = Window<double>.NewVariable();
            public Window<int> Week = Window<int>.NewVariable(-1), PartitionNumber = Window<int>.NewVariable(-1), RowNumber = Window<int>.NewVariable(-1);
            public void CopyTo(RowData d)
            {
                d.DateFrom.Value = DateFrom.Value;
                d.DateTo.Value = DateTo.Value;
                d.Open.Value = Open.Value;
                d.Low.Value = Low.Value;
                d.High.Value = High.Value;
                d.Close.Value = Close.Value;
                d.Volume.Value = Volume.Value;
                d.RowNumber.Value = RowNumber.Value;
                d.Week.Value = Week.Value;
                d.PartitionNumber.Value = PartitionNumber.Value;
            }
        }
        #region properties
        IntervalType mIntervalType;
        DateTime mStartDate=DateTime.MinValue, mEndDate = DateTime.MaxValue;
        int mSize, mInterval, mSymbolID;
        Window<DateTime> mDateFrom, mDateTo;
        Window<double> mOpen, mClose, mHigh, mLow;
        Window<double> mVolume;
        Window<int> mItemCount;

        public int SourceWindowSize
        {
            get { return mSize; }
            set
            {
                if (!IsClosed)
                    throw new Exception("Size property cannot be changed when the source is opened");
                mSize = value;
            }
        }
        public int SymbolID
        {
            get { return mSymbolID; }
            set
            {
                if (!IsClosed)
                    throw new Exception("SymbolID property cannot be changed when the source is opened");
                mSymbolID = value;
            }
        }
        public Window<DateTime> DateFrom { get { return mDateFrom; } }
        public Window<DateTime> DateTo { get { return mDateTo; } }
        public Window<double> Open { get { return mOpen; } }
        public Window<double> Close { get { return mClose; } }
        public Window<double> High { get { return mHigh; } }
        public Window<double> Low { get { return mLow; } }
        public Window<double> Volume { get { return mVolume; } }
        public Window<int> ItemCount { get { return mItemCount; } }
        public IntervalType IntervalType
        {
            get { return mIntervalType; }
            set
            {
                if (!IsClosed)
                    throw new Exception("IntervalType property cannot be changed when the source is opened");
                mIntervalType = value;
            }
        }
        public int Interval
        {
            get { return mInterval; }
            set
            {
                if (!IsClosed)
                    throw new Exception("Interval property cannot be changed when the source is opened");
                mInterval = value;
            }
        }
        public DateTime StartDate
        {
            get { return mStartDate; }
            set
            {
                if (!IsClosed)
                    throw new Exception("StartDate property cannot be changed when the source is opened");
                mStartDate = value;
            }
        }
        public DateTime EndDate
        {
            get { return mEndDate; }
            set
            {
                if (!IsClosed)
                    throw new Exception("EndDate property cannot be changed when the source is opened");
                mEndDate = value;
            }
        }
        public bool IsClosed { get { return (reader == null) || ((reader != null) && reader.IsClosed); } }
        #endregion
        #region Constructors
        void InitializeProperties(int symbolId, IntervalType intervalType, int interval, int size)
        {

            mSize = size;
            mIntervalType = intervalType;
            mInterval = interval;
            mSymbolID = symbolId;

        }
        public Source()
        {
            InitializeProperties(0, Core.IntervalType.Minutes, 1, Window<double>.DefaultDataWindowSize);
        }
        public Source(int symbolId, IntervalType intervalType, int interval, int size = Window<double>.DefaultDataWindowSize)
        {
            InitializeProperties(symbolId, intervalType, interval, size);
        }
        public Source(int symbolId, IntervalType intervalType, int interval)
        {
            InitializeProperties(symbolId, intervalType, interval, Window<double>.DefaultDataWindowSize);
        }
        
        #endregion
        public void CloseData()
        {
            if (!IsClosed)
                CloseDataReader();
        }
        public void OpenData()
        {
            if (IsClosed)
            {
                mDateFrom = new Window<DateTime>(mSize);
                mDateTo = new Window<DateTime>(mSize);
                mOpen = new Window<double>(mSize);
                mClose = new Window<double>(mSize);
                mHigh = new Window<double>(mSize);
                mLow = new Window<double>(mSize);
                mVolume = new Window<double>(mSize);
                mItemCount = new Window<int>(mSize);
                reader = Methods.RetrieveQuote(mSymbolID, mIntervalType != IntervalType.Minutes, mStartDate, mEndDate);
            }
        }

        RowData raw = new RowData(), staging1 = new RowData(), staging2 = new RowData();

        bool ReadData()
        {
            int rowNumber;
            if (IsClosed)
            {
                return false;
            }
            if (mIntervalType == Core.IntervalType.Minutes || mIntervalType == Core.IntervalType.Days)
            {
                if (reader.Read())
                {
                    staging1.Week.Value = reader.GetInt32(0);
                    staging1.DateFrom.Value = reader.GetDateTime(1);
                    staging1.DateTo.Value = staging1.DateFrom;
                    staging1.Open.Value = reader.GetFloat(2);
                    staging1.High.Value = reader.GetFloat(3);
                    staging1.Low.Value = reader.GetFloat(4);
                    staging1.Close.Value = reader.GetFloat(5);
                    staging1.Volume.Value = Convert.ToDouble(reader.GetInt64(6));
                    staging1.RowNumber.Value++;
                    return true;
                }
                else
                {
                    CloseDataReader();
                    return false;
                }
            }
            else
            {
                if (staging1.PartitionNumber != -1)
                {
                    rowNumber = staging1.RowNumber;
                    raw.CopyTo(staging1);
                    staging1.RowNumber = rowNumber;
                    staging1.RowNumber++;
                }
                while (reader.Read())
                {
                    raw.Week.Value = reader.GetInt32(0);
                    raw.DateFrom.Value = reader.GetDateTime(1);
                    raw.DateTo.Value = raw.DateFrom;
                    raw.Open.Value = reader.GetFloat(2);
                    raw.High.Value = reader.GetFloat(3);
                    raw.Low.Value = reader.GetFloat(4);
                    raw.Close.Value = reader.GetFloat(5);
                    raw.Volume.Value = reader.GetInt64(6);
                    raw.RowNumber++;
                    if (IntervalType == Core.IntervalType.Weeks)
                        raw.PartitionNumber.Value = raw.DateFrom.Value.Year * 100 + raw.Week.Value;
                    else if (IntervalType == Core.IntervalType.Months)
                        raw.PartitionNumber.Value = raw.DateFrom.Value.Year * 100 + raw.DateFrom.Value.Month;
                    else if (IntervalType == Core.IntervalType.Years)
                        raw.PartitionNumber.Value = raw.DateFrom.Value.Year;
                    if (staging1.PartitionNumber == -1)
                    {
                        raw.CopyTo(staging1);
                        continue;
                    }
                    else
                    {
                        if (raw.PartitionNumber.Value != staging1.PartitionNumber.Value)
                            return true;
                        staging1.Close.Value = raw.Close.Value;
                        staging1.DateTo.Value = raw.DateTo.Value;
                        staging1.High.Value = Math.Max(staging1.High.Value, raw.High.Value);
                        staging1.Low.Value = Math.Min(staging1.Low.Value, raw.Low.Value);
                        staging1.Volume.Value = staging1.Volume.Value + raw.Volume.Value;
                        continue;
                    }
                }

                CloseDataReader();
                return true;
            }
        }
        void PushData()
        {
            mDateFrom.Push(staging2.DateFrom);
            mDateTo.Push(staging2.DateTo);
            mOpen.Push(staging2.Open);
            mHigh.Push(staging2.High);
            mLow.Push(staging2.Low);
            mClose.Push(staging2.Close);
            mVolume.Push(staging2.Volume);
            mItemCount.Push(staging2.RowNumber);
        }
        void SetValuesInPartition()
        {
            staging2.Low.Value = Math.Min(staging2.Low.Value, staging1.Low.Value);
            staging2.High.Value = Math.Max(staging2.High.Value, staging1.High.Value);
            staging2.Close.Value = staging1.Close.Value;
            staging2.Volume.Value = staging2.Volume.Value + staging1.Volume.Value;
            staging2.DateTo.Value = staging1.DateTo.Value;
            staging2.RowNumber.Value++;
        }
        void InitialValuesInPartition()
        {
            staging1.CopyTo(staging2);
            staging2.RowNumber.Value = 1;
        }
        public bool Read(bool pushAfterRead = true)
        {
            bool hasDataProcessed = false;
            while (ReadData())
            {
                hasDataProcessed = true;
                if (Interval == 1)
                {
                    InitialValuesInPartition();
                    PushData();
                    return true;
                }
                // Interval > 1
                if (staging1.RowNumber % Interval == 0)
                {
                    InitialValuesInPartition();
                    continue;
                }
                SetValuesInPartition();
                if (staging1.RowNumber.Value % Interval == Interval - 1)
                {
                    PushData();
                    return true;
                }
            }
            if (hasDataProcessed)
            {
                if (Interval > 1)
                {
                    PushData();
                    return true;
                }
            }
            //PushData();
            return false;
        }
        SqlDataReader reader = null;
        public void Dispose()
        {
            CloseDataReader();
        }
        void CloseDataReader()
        {
            if (reader != null)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader = null;
                }
            }
        }
    }

}
