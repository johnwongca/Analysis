using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Trade.Analyze;
using Trade.Base;


namespace Trade
{
    public partial class FormTest : Trade.Base.BaseForm
    {
        public FormTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tb.Text = "Total memory = {0:,0.00} M".FormatString(Convert.ToDecimal(GC.GetTotalMemory(true))/1024/1024);
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = GC.MaxGeneration; i >= 0; i--)
            {
                GC.Collect(i, GCCollectionMode.Forced);
            }
            GC.WaitForPendingFinalizers();
            FormMain.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }

    }
}

