using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public class DataWindow : IEnumerable<double>
    {
        public const int DefaultDataWindowSize = 1000;
        protected long mCurrentOrdinal = -1;

        double[] mBuffer = null;
        int lastLocation = -1;
        public int Size
        {
            get { return mBuffer.Length; }
            set
            {
                mBuffer = new double[value];
                mBuffer.FillWithNaN();
            }
        }
        public long CurrentOrdinal { get { return mCurrentOrdinal; } }
        protected virtual void Initialization(double value)
        {
        }
        protected virtual void BeforeSetValue(double value)
        {
            
        }
        protected virtual void AfterSetValue(double value)
        {

        }
        public virtual DataWindow Set(double value)
        {
            mCurrentOrdinal++;
            if (mCurrentOrdinal == 0)
                Initialization(value);
            lastLocation = (int)(mCurrentOrdinal % Size);
            mBuffer[lastLocation] = double.NaN;
            BeforeSetValue(value);
            mBuffer[lastLocation] = value;
            AfterSetValue(value);
            return this;
        }

        public DataWindow(int size = DefaultDataWindowSize)
        {
            if (size <= 0)
                throw new Exception("Size of DataWindow should not be zero.");
            mBuffer = (new double[size]).FillWithNaN();
        }
       
        public double[] Buffer { get { return this.ToArray(); } }
        public double[] RawBuffer { get { return mBuffer; } }
        public double First
        {
            get
            {
                if (mCurrentOrdinal < Size)
                    return mBuffer[0];
                return mBuffer[(mCurrentOrdinal - Size + 1) % Size];
            }
        }
        public double this[int index]
        {
            get
            {
                if ((mCurrentOrdinal < index) || (index >= Size))
                    return double.NaN;
                return mBuffer[(mCurrentOrdinal - index) % Size];
            }
        }
        public double Value
        {
            get { return this[0]; }
        }
        public IEnumerator<double> GetEnumerator()
        {
            double v;
            for (int i = 0; i < Size; i++)
            {
                v = this[i];
                if (double.IsNaN(v))
                    break;
                yield return v;
            }

        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region overloaded Operators
        //public static implicit operator double[](DataWindow buffer)
        //{
        //    return buffer.ToArray();
        //}
        public static implicit operator double(DataWindow buffer)
        {
            return buffer.Value;
        }
        #endregion
    }
}
