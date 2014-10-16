using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fasterflect;
namespace Algorithm.Core.Forms
{
    public partial class ChartForm : FormBase
    {
        bool instanceStart = true;
        public long Token;
        IntervalType IntervalType = IntervalType.Minutes;
        int Interval = 1, SymbolID = -1, startLocation = -1, numberOfRows = 100, CursorSize;
        IndicatorClass IndicatorClass = null;
        ChartDetailForm details = null;
        bool isUserSearchText = true, stopLoading = true;
        DataTable data = new DataTable() { TableName = "shit"};
        DataView dataView = new DataView();
        DateTime StartDate;
        public bool IsLoadingData
        {
            get { return labelIsLoading.Visible; }
        }
        public ChartForm()
        {
            Token = Methods.GetToken();
            InitializeComponent();
            labelIsLoading.Visible = false;
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
            hScrollBar.Minimum = 1;
            details = new ChartDetailForm();
            details.Owner = this;
            details.OnSearchConfirm += new EventHandler(OnSearchConfirm);
            nInterval.NumericUpDownControl.Minimum = 1;
            nInterval.NumericUpDownControl.Maximum = 99999999;
            nInterval.NumericUpDownControl.Value = Interval;
            dpStartDate.Value = DateTime.Now.AddYears(-1);
            nScale.NumericUpDownControl.Minimum = 1;
            nScale.NumericUpDownControl.Maximum = 1000;
            nScale.NumericUpDownControl.Value = numberOfRows;
            hScrollBar.Minimum = 1;
            hScrollBar.Maximum = 1;
            hScrollBar.Value = 1;
            stopLoading = false;
            ReloadData();
            instanceStart = false;
        }
        void SetCursorLocation()
        {
            
            if (startLocation<=0)
		        startLocation = 200000000;
	        if (CursorSize < startLocation + numberOfRows)
		        startLocation = CursorSize  - numberOfRows;
            if (startLocation < 1)
                startLocation = 0;
            Console.WriteLine("Start scrolling {0}", startLocation);
            dataView.RowFilter = "___RowNumber___>=" + startLocation.ToString() + " and ___RowNumber___<" + (startLocation + numberOfRows).ToString();
            Console.WriteLine("Scrolling {0} done.{1}--{2}", startLocation, dataView.ToTable().Rows[0]["___RowNumber___"], dataView.ToTable().Rows[dataView.ToTable().Rows.Count - 1]["___RowNumber___"]);
        }
        void ReloadData(bool forceReloading = false)
        {
            try
            {
                labelIsLoading.Visible = true;
                Application.DoEvents();
                if (stopLoading)
                    return;
                if (SymbolID < 0)
                    return;
                //string cursorName = Methods.GetCursorName(SymbolID, IntervalType, Interval, StartDate);
                //if (forceReloading)
                //    cursorName.CursorRemove();
                //CursorSize = cursorName.CursorSize();


                using (Indicator indicator = (Indicator)Activator.CreateInstance(IndicatorClass.Class))
                {
                    indicator.SetDefaultValues();
                    indicator.StartDate = StartDate;
                    indicator.SymbolID = SymbolID;
                    indicator.Interval = Interval;
                    indicator.IntervalType = IntervalType;
                    //indicator.WriteToServer();
                    Console.WriteLine("Start fetching {0}", SymbolID);
                    data = indicator.WriteToDataTable(data);
                    Console.WriteLine("Fetching {0} done. Size = {1}", SymbolID, data.Rows.Count);
                    dataView.Table = data;
                }
                CursorSize = data.Rows.Count;//cursorName.CursorSize();
                SetCursorLocation();
                hScrollBar.Minimum = 0;
                hScrollBar.Maximum = Math.Max(1, CursorSize);
                hScrollBar.Value = startLocation;
            }
            finally
            {
                labelIsLoading.Visible = false;
            }
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

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            StartDate = ((DateTimePicker)sender).Value;
            ReloadData();
        }

        private void nScale_ValueChanged(object sender, EventArgs e)
        {
            if (instanceStart)
                return;
            numberOfRows = Convert.ToInt32(((ToolStripNumericUpDown)sender).NumericUpDownControl.Value);
            SetCursorLocation();
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (IsLoadingData)
                return;
            startLocation = hScrollBar.Value;
            //ReloadData();
            
            SetCursorLocation();
            
        }

        private void miRemoveAllCursors_Click(object sender, EventArgs e)
        {
            Methods.CursorRemoveAll();
        }
    }
}
