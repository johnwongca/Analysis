using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public static partial class ExtensionMethods
    {
        public static double[] FillWithNaN(this double[] arg)
        {
            if (arg == null)
                return null;
            for (int i = 0; i < arg.Length; i++)
                arg[i] = double.NaN;
            return arg;
        }
        public static double ToDouble(this int i)
        {
            return Convert.ToDouble(i);
        }
    }
}
