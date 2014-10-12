using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algorithm.Core.Forms
{
    public partial class ChartForm : FormBase
    {
        public long Token;
        IntervalType IntervalType = IntervalType.Minutes;
        int Interval = 1, SymbolID = -1;
        IndicatorClass IndicatorClass = null;
        ChartDetailForm details = null;
        bool isUserSearchText = true;
        public ChartForm()
        {
            Token = Methods.GetToken();
            InitializeComponent();
            AlgorithmMenu.DropDownItems.Clear();
            for (int i = 0; i < IndicatorClass.IndicatorClasses.Count; i++)
            {
                var item = new ToolStripMenuItem(IndicatorClass.IndicatorClasses[i].IndicatorName) { Tag = i, Checked = false };
                item.Click += AlgorithmChange;
                if (IndicatorClass.IndicatorClasses[i].IndicatorName == "Default")
                {
                    item.Checked = true;
                    AlgorithmMenu.Tag = item.Tag;
                    AlgorithmMenu.Text = item.Text;
                    IndicatorClass = IndicatorClass.IndicatorClasses[i];
                }
                AlgorithmMenu.DropDownItems.Add(item);
            }

            details = new ChartDetailForm();
            details.Owner = this;
            details.OnSearchConfirm += new EventHandler(OnSearchConfirm);
            nInterval.NumericUpDownControl.Minimum = 1;
            nInterval.NumericUpDownControl.Maximum = 99999999;
            ReloadData();
        }
        void ReloadData()
        {
            Console.WriteLine("Reloaddata {0}", IndicatorClass.IndicatorName);
        }
        void OnSearchConfirm(object sender, EventArgs e)
        {
            isUserSearchText = false;
            QuoteSearch.Text = details.Symbol;
            isUserSearchText = true;
            if (SymbolID != details.SymbolID)
            {
                SymbolID = details.SymbolID;
                ReloadData();
            }
            
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
            details.Show();
        }

        private void ChartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            details.Close();
        }

        private void QuoteSearch_TextChanged(object sender, EventArgs e)
        {
            if (!isUserSearchText)
                return;
            details.SearchText = QuoteSearch.Text;
            details.SearchFound();
        }
        private void AlgorithmChange(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem i in AlgorithmMenu.DropDownItems)
                i.Checked = false;
            var item = (ToolStripMenuItem)sender;
            item.Checked = true;
            if (AlgorithmMenu.Text != item.Text)
            {
                AlgorithmMenu.Text = item.Text;
                AlgorithmMenu.Tag = item.Tag;
                IndicatorClass = IndicatorClass.IndicatorClasses[Convert.ToInt32(item.Tag)];
                ReloadData();
            }
        }

        private void IntervalChange(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem i in IntervalTypeMenu.DropDownItems)
                i.Checked = false;
            var item = (ToolStripMenuItem)sender;
            item.Checked = true;
            if (IntervalTypeMenu.Text != item.Text)
            {
                IntervalTypeMenu.Text = item.Text;
                IntervalType = (IntervalType)Enum.Parse(typeof(IntervalType), IntervalTypeMenu.Text, true);
                ReloadData();
            }
        }

        private void nInterval_ValueChanged(object sender, EventArgs e)
        {
            if(Convert.ToInt32( nInterval.Value) != Interval)
            {
                Interval = Convert.ToInt32(nInterval.Value);
                ReloadData();
            }
        }
    }
}
