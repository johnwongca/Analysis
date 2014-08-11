using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Core
{
    public abstract class IndicatorBase : DataWindow
    {
        //public int SymbolID { get; set; }
        public IndicatorBase(int size = DefaultDataWindowSize)
            : base(size)
        {

        }
    }
}
