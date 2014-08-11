//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//namespace Algorithm.Core
//{

//    public partial class DataBuffer : Variable, IEnumerable<double>
//    {
//        private double Aggregate(int fromOrElementBack, int to, Func<int, double, double> aggr)
//        {
//            double ret = double.NaN, v;
//            int from = fromOrElementBack;
//            if(to==-1)
//            {
//                from = 0;
//                to = fromOrElementBack;
//            }
//            for (int i = from; i <= to; i++ )
//            {
//                v = this[i];
//                if(double.IsNaN(v))
//                    break;
//                ret = aggr(from - i, v);
//            }
//            return ret;
//        }
//        private double Aggregate(int fromOrElementBack, int to, Func<double, double> aggr)
//        {
//            double ret = double.NaN, v;
//            int from = fromOrElementBack;
//            if(to==-1)
//            {
//                from = 0;
//                to = fromOrElementBack;
//            }
//            for (int i = from; i <= to; i++ )
//            {
//                v = this[i];
//                if(double.IsNaN(v))
//                    break;
//                ret = aggr(v);
//            }
//            return ret;
//        }
//        public double Sum(int fromOrElementBack, int to = -1)
//        {
//            double ret = 0;
//            return Aggregate(fromOrElementBack, to,
//                (value) =>
//                {
//                    ret += value;
//                    return ret;
//                });
//        }
//        public double Max(int fromOrElementBack, int to = -1)
//        {
            
//            double ret = double.MinValue;
//            return Aggregate(fromOrElementBack, to,
//                (value) =>
//                {
//                    ret = Math.Max(ret, value);
//                    return ret;
//                });
//        }
//        public double Min(int fromOrElementBack, int to = -1)
//        {
//            double ret = double.MaxValue;
//            return Aggregate(fromOrElementBack, to,
//                (value) =>
//                {
//                    ret = Math.Min(ret, value);
//                    return ret;
//                });
//        }
//        public double Average(int fromOrElementBack, int to = -1)
//        {
//            double ret = 0;
//            int count = 0;
//            ret = Aggregate(fromOrElementBack, to,
//                (index, value) =>
//                {
//                    count = index;
//                    ret = ret + value;
//                    return ret;
//                });
//            return ret / (count + 1);
//        }
//        public double StandardDeviation(int fromOrElementBack, int to = -1)
//        {
//            double avg = Average(fromOrElementBack, to);
//            double sum = 0;
//            int count = 0;
//            sum = Aggregate(fromOrElementBack, to,
//                (index, value) =>
//                {
//                    count = index;
//                    sum = sum + Math.Pow(value - avg, 2);
//                    return sum;
//                });

//            if (count > 0)
//                return Math.Sqrt((sum) / count.ToDouble());
//            return 0;
//        }

//        public double MeanDeviation(int fromOrElementBack, int to = -1)
//        {
//            double avg = Average(fromOrElementBack, to);
//            double sum = 0;
//            int count = 0;
//            sum = Aggregate(fromOrElementBack, to,
//                (index, value) =>
//                {
//                    count = index;
//                    sum = sum + Math.Abs(value - avg);
//                    return sum;
//                });
//            return Math.Sqrt((sum) / (count + 1).ToDouble());
//        }
//    }
//}
