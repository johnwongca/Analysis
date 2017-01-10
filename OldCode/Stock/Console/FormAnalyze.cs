using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Console
{
    public partial class FormAnalyze : Console.BaseForm
    {
        public FormAnalyze()
        {
            InitializeComponent();
        }
        private void Bind(object o)
        {
            bs.DataSource = o;
            dgv.DataSource = null;
            dgv.AutoGenerateColumns = true;
            dgv.DataSource = bs;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<Price> data = Price.ReadData(SQL.Text);
            double[] data1 = PrepareMCAD(data);
            /*ExpMA ma = new ExpMA(data1, 3, 50);
            Bind(ma.MATable);*/
            MACD macd = new MACD(data1, 3, 50, 3, 9);
        }
        private double[] PrepareMCAD(List<Price> data)
        {
            double[] ret = new double[data.Count];
            for (int i = 0; i < data.Count; i++)
                ret[i] = data[i].Closing;
            return ret;
        }
    }
}

