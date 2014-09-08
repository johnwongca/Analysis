using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public interface IComparableEnumerable<T> : IEnumerable<T> where T : IComparable<T>, IEquatable<T>, IConvertible
    {
    }
}
