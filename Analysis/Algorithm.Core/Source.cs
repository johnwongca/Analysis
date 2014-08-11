using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public enum SourceType { Minutes = 1, Days = 2, Weeks = 3, Months = 4, Years = 5 }
    public class Source
    {
        int mSize;
        DataWindow mDateFrom, mDateTo, mOpen, mClose, mHigh, mLow, mVolume;
        #region properties
        public int Size { get { return mSize; } }
        public DataWindow DateFrom { get { return mDateFrom; } }
        public DataWindow DateTo { get { return mDateTo; } }
        public DataWindow Open { get { return mOpen; } }
        public DataWindow Close { get { return mClose; } }
        public DataWindow High { get { return mHigh; } }
        public DataWindow Low { get { return mLow; } }
        public DataWindow Volume { get { return mVolume; } }
        #endregion
        public Source(int size = DataWindow.DefaultDataWindowSize)
        {
            mSize = size;
            mDateFrom = new DataWindow(size);
            mDateTo = new DataWindow(size);
            mOpen = new DataWindow(size); 
            mClose = new DataWindow(size); 
            mHigh = new DataWindow(size); 
            mLow = new DataWindow(size);
            mVolume = new DataWindow(size);
        }
    }

}
