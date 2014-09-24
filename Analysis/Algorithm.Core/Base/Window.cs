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
        protected int mInputCacheSize = 0;
        protected Window<T>[] cache = null;
        public int InputCacheSize { get { return mInputCacheSize; } set { mInputCacheSize = value; } }


        public const int DefaultDataWindowSize = 1000;
        protected long mCurrentLocation = -1;

        T[] mBuffer = null;
        int position = -1;
        public int Size
        {
            get { return mBuffer.Length; }
            set
            {
                mBuffer = new T[value];
            }
        }
        public long CurrentLocation { get { return mCurrentLocation; } }
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
            
            mCurrentLocation++;
            if ((mInputCacheSize > 0) && (cache == null))
                cache = new Window<T>[values.Length];
            if (mInputCacheSize > 0)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (cache[i] == null)
                        cache[i] = new Window<T>(mInputCacheSize);
                    cache[i].Push(values[i].Value);
                }
            }
            Window<T>[] p = mInputCacheSize > 0 ? cache : values;
            if (mCurrentLocation == 0)
                Initialization(p);
            BeforeSetValue(p);
            position = (int)(mCurrentLocation % Size);
            mBuffer[position] = default(T);
            
            if (p.Length > 0)
                mBuffer[position] = p[0].Value;
            AfterSetValue(p);
            return this;
        }
        
        public virtual Window<T> Set(T value)
        {
            mBuffer[position] = value;
            return this;
        }
        public virtual Window<T> Set(int index, T value)
        {
            mBuffer[(mCurrentLocation - index) % Size] = value;
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
                if (mCurrentLocation < Size)
                    return mBuffer[0];
                return mBuffer[(mCurrentLocation - Size + 1) % Size];
            }
        }
        public bool HasValue(int index)
        {
            return !((mCurrentLocation < index) || (index >= Size));
        }
        public T this[int index]
        {
            get
            {
                if (!HasValue(index))
                    return default(T);
                return mBuffer[(mCurrentLocation - index) % Size];
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
                return (int)Math.Min(mCurrentLocation + 1, Size);
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Math.Min(mCurrentLocation+1, Size); i++)
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
