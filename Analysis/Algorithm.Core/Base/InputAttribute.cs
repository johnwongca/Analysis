using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public class InputAttribute:Attribute
    {
        public string Name { get; set; }
        public double DefaultValue { get; set; }
        public double TestFromValue { get; set; }
        public double TestToValue { get; set; }
    }
}
