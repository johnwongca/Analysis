//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Algorithm.Core
//{
//    public partial class DataBuffer : Variable, IEnumerable<double>
//    {
//        const int DefaultDataBufferSize = 1000;

//        double[] mBuffer = null;
//        int lastLocation = -1;
//        public int Size
//        {
//            get { return mBuffer.Length; }
//            set
//            {
//                mBuffer = new double[value];
//                mBuffer.FillWithNaN();
//            }
//        }
//        void CalculateInternal(params double[] args)
//        {
//            lastLocation = (int)(CountOfValueSet % Size);
//            mBuffer[lastLocation] = args[0];
//            mValue = args[0];
//        }
//        protected override void Calculate(params double[] args)
//        {
//            CalculateInternal(args);
//        }
//        public DataBuffer(int size = DefaultDataBufferSize)
//        {
//            if (size <= 0)
//                throw new Exception("Size of DataBuffer should not be zero.");
//            mBuffer = (new double[size]).FillWithNaN();
//        }
//        //public DataBuffer(double[] buffer)
//        //{
//        //    if (buffer == null)
//        //        throw new Exception("Buffer parameter should not be NULL or empty.");
//        //    if (buffer.Length == 0)
//        //        throw new Exception("Buffer parameter should not be NULL or empty.");
//        //    mBuffer = buffer;
//        //    CountOfValueSet = mBuffer.Length;
//        //}
//        public double[] Buffer { get { return this.ToArray(); } }
//        public double[] RawBuffer { get { return mBuffer; } }
//        public double First
//        {
//            get
//            {
//                if (CountOfValueSet < Size)
//                    return mBuffer[0];
//                return mBuffer[(CountOfValueSet - Size + 1) % Size];
//            }
//        }
//        public double this[int index]
//        {
//            get
//            {
//                if ((CountOfValueSet < index) || (index >= Size))
//                    return double.NaN;
//                return mBuffer[(CountOfValueSet - index) % Size];
//            }
//        }

//        public IEnumerator<double> GetEnumerator()
//        {
//            double v;
//            for (int i = 0; i < Size; i++ )
//            {
//                v = this[i];
//                if (double.IsNaN(v))
//                    break;
//                yield return v;
//            }
                
//        }

//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }

//        #region overloaded Operators
//        public static implicit operator double[](DataBuffer buffer)
//        {
//            return buffer.ToArray();
//        }
//        //public static implicit operator DataBuffer(double[] array)
//        //{
//        //    return new DataBuffer(array);
//        //}
//        #endregion
//    }
//}
