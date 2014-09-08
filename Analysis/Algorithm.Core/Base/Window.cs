using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public class Window<T>: IWindow<T> where T : IComparable<T>, IEquatable<T>, IConvertible
    {
        public const int DefaultDataWindowSize = 1000;
        protected long mCurrentOrdinal = -1;

        T[] mBuffer = null;
        int lastLocation = -1;
        public int Size
        {
            get { return mBuffer.Length; }
            set
            {
                mBuffer = new T[value];
            }
        }
        public long CurrentOrdinal { get { return mCurrentOrdinal; } }
        protected virtual void Initialization(params Window<T>[] values)
        {
        }
        protected virtual void BeforeSetValue(params Window<T>[] values)
        {
        }
        protected virtual void AfterSetValue(params Window<T>[] values)
        {
        }
        public virtual Window<T> Push(params Window<T>[] values)
        {
            mCurrentOrdinal++;
            if (mCurrentOrdinal == 0)
                Initialization(values);
            lastLocation = (int)(mCurrentOrdinal % Size);
            mBuffer[lastLocation] = default(T);
            BeforeSetValue(values);
            if (values.Length > 0)
                mBuffer[lastLocation] = values[0].Value;
            AfterSetValue(values);
            return this;
        }
        
        public virtual Window<T> Set(T value)
        {
            mBuffer[lastLocation] = value;
            return this;
        }
        public virtual Window<T> Set(int index, T value)
        {
            mBuffer[(mCurrentOrdinal - index) % Size] = value;
            return this;
        }
        public Window(int size = DefaultDataWindowSize)
        {
            if (size <= 0)
                throw new Exception("Size of DataWindow should not be zero.");
            mBuffer = (new T[size]);
        }

        public T[] Buffer { get { return this.ToArray<T>(); } }
        public T[] RawBuffer { get { return mBuffer; } }
        public T First
        {
            get
            {
                if (mCurrentOrdinal < Size)
                    return mBuffer[0];
                return mBuffer[(mCurrentOrdinal - Size + 1) % Size];
            }
        }
        public bool HasValue(int index)
        {
            return !((mCurrentOrdinal < index) || (index >= Size));
        }
        public T this[int index]
        {
            get
            {
                if (!HasValue(index))
                    return default(T);
                return mBuffer[(mCurrentOrdinal - index) % Size];
            }
        }
        public T Value
        {
            get { return this[0]; }
            set 
            {
                Set(value);
            }
        }
        public int ValuesInWindow
        {
            get
            {
                return (int)Math.Min(mCurrentOrdinal + 1, Size);
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Math.Min(mCurrentOrdinal+1, Size); i++)
            {
                yield return this[i];
            }

        }

        public IEnumerable<T> SubSet(int from, int numberOfItems)
        {
            for (int i = from; i < from + numberOfItems; i++)
            {
                if (!HasValue(i))
                    break;
                yield return this[i];
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region overloaded Operators
        public static implicit operator T[](Window<T> buffer)
        {
            return buffer.ToArray();
        }
        
        public static implicit operator T(Window<T> buffer)
        {
            return buffer.Value;
        }
        public static implicit operator Window<T>(T value)
        {
            return NewVariable(value);
        }
        #endregion
        public static Window<T> NewWindow(int size = DefaultDataWindowSize)
        {
            return new Window<T>(size);
        }
        public static Window<T> NewVariable()
        {
            return new Window<T>(1).Push();
        }
        public static Window<T> NewVariable(T value)
        {
            return new Window<T>(1).Push().Set(value);
        }
    }
    
}
