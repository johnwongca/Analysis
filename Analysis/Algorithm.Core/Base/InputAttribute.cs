using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public class InputAttribute:Attribute
    { }
    public class InputDoubleAttribute : InputAttribute
    {
        public double DefaultValue { get; set; }
        public double FromValue { get; set; }
        public double ToValue { get; set; }
        public double Interval { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class InputIntAttribute : InputAttribute
    {
        public int DefaultValue { get; set; }
        public int FromValue { get; set; }
        public int ToValue { get; set; }
        public int Interval { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class InputIntervalTypeAttribute : InputAttribute
    {
        public IntervalType DefaultValue { get; set; }
        public IntervalType Value { get; set;}
        public int DisplayOrder { get; set; }
    }
    public class InputDateTimeAttribute : InputAttribute
    {
        public DateTime DefaultValue { get; set; }
        public DateTime Value { get; set; }
        public int DisplayOrder { get; set; }
    }
}
