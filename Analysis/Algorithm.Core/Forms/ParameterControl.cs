using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algorithm.Core.Forms
{
    public partial class ParameterControl : UserControl
    {
        public ParameterControl()
        {
            InitializeComponent();
        }
        public string PropertyName { get; set; }
        public int IntValue { get { return Convert.ToInt32(Value.Value); } }
        public double DoubleValue { get { return Convert.ToDouble(Value.Value); } }
        public IndicatorClass IndicatorClass { get; set; }
    }
}
