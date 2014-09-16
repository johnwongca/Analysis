using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public abstract partial class IndicatorBase : Window<double>, ISource 
    {
        ISource mSource;

        public ISource Source
        {
            get { return mSource; }
            set { mSource = value; }
        }
        #region Implement ISource
        [Output]
        public Window<DateTime> DateFrom { get { return mSource.DateFrom; } }
        [Output]
        public Window<DateTime> DateTo { get { return mSource.DateTo; } }
        [Output]
        public Window<double> Open { get { return mSource.Open; } }
        [Output]
        public Window<double> High { get { return mSource.High; } }
        [Output]
        public Window<double> Low { get { return mSource.Low; } }
        [Output]
        public Window<double> Close { get { return mSource.Close; } }
        [Output]
        public Window<double> Volume { get { return mSource.Volume; } }
        
        [Output]
        public Window<int> ItemCount { get { return mSource.ItemCount; } }

        [Output]
        public Window<double> TypicalPrice { get { return mSource.TypicalPrice; } }
        public int SymbolID { get { return mSource.SymbolID; } set { mSource.SymbolID = value; } }
        public IntervalType IntervalType { get { return mSource.IntervalType; } set { mSource.IntervalType = value; } }
        public int Interval { get { return mSource.Interval; } set { mSource.Interval = value; } }
        public int SourceWindowSize { get { return mSource.SourceWindowSize; } set { mSource.SourceWindowSize = value; } }
        public DateTime StartDate { get { return mSource.StartDate; } set { mSource.StartDate = value; } }
        public DateTime EndDate { get { return mSource.EndDate; } set { mSource.EndDate = value; } }
        public bool IsClosed { get { return mSource.IsClosed; } }

        public void OpenData()
        {
            mSource.OpenData();
        }
        public void CloseData()
        {
            mSource.CloseData();
        }
        public bool Read(bool pushAfterRead = true)
        {
            bool ret = mSource.Read();
            if (ret && pushAfterRead)
                Push();
            return ret;
        }
        public void Dispose()
        {
            mSource.Dispose();
        }
        #endregion
        public IndicatorBase(int size = DefaultDataWindowSize)
            : base(size)
        {
            mSource = new Source();
            mSource.SourceWindowSize = size;
        }
        
    }
}
