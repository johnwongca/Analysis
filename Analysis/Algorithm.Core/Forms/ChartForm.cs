using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.Xml.Linq;
using Fasterflect;
namespace Algorithm.Core.Forms
{
    public partial class ChartForm : FormBase
    {
        public const string SplitterName = "Splitter";
        bool instanceStart = true;
        public long Token;
        IntervalType IntervalType = IntervalType.Days;
        int Interval = 1, SymbolID = -1, startLocation = -1, numberOfRows = 100;
        int CursorSize { get { return data.Rows.Count; } }
        IndicatorClass IndicatorClass = null;
        FlowLayoutPanel currentPanel = null;
        ChartDetailForm details = null;
        bool isUserSearchText = true, stopLoading = true;
        DataTable data = new DataTable() { TableName = "base"};
        //DataView dataView = new DataView();
        DataTable chartData = new DataTable("view");
        DateTime StartDate;
        PropertyGrid chartPropertySelected, chartPropertyAll;
        string chartName = "Default";
        XElement chartDefinition = null;
        public bool IsLoadingData
        {
            get { return labelIsLoading.Visible; }
        }
        void LoadChartList()
        {
            if (ChartList.Charts.Count == 0)
            {
                ChartList.Charts.Add(new ChartList() { Name = "Default", Definition = null });
            }
            miCharts.DropDownItems.Clear();
            foreach(ChartList c in ChartList.Charts)
            {
                var item = new ToolStripMenuItem(c.Name) { Tag = c, Checked = c.Name.ToUpper() == chartName.ToUpper() };
                item.Click += ChartChange;
                miCharts.DropDownItems.Add(item);
            }
            ChartChange(null, null);
        }
        void ValidateChart()
        {
            foreach (ToolStripMenuItem item in miCharts.DropDownItems)
            {
                if(item.Checked)
                {
                    chartDefinition = ((ChartList)(((ToolStripMenuItem)item).Tag)).Definition;
                    break;
                }
            }
            bool changed = false;
            List<XElement> removeList = new List<XElement>();
            foreach (var x1 in chartDefinition.Elements("Series"))
            {
                foreach (var x2 in x1.Elements("Series"))
                {
                    if ((x2.Attribute("Name").Value == "Price") || (x2.Attribute("Name").Value == "Volume"))
                        continue;
                    if (data.Columns.IndexOf(x2.Attribute("XValueMember").Value) < 0)
                    {
                        x2.Attribute("XValueMember").Value = "";
                        if (removeList.IndexOf(x2) < 0)
                            removeList.Add(x2);
                        changed = true;
                    }
                    if (data.Columns.IndexOf(x2.Attribute("YValueMembers").Value) < 0)
                    {
                        x2.Attribute("YValueMembers").Value = "";
                        if (removeList.IndexOf(x2) < 0)
                            removeList.Add(x2);
                        changed = true;
                    }
                }
            }
            if (changed)
            {
                foreach (var a in removeList)
                    a.Remove();
                chart1.SetDefinition(chartDefinition);
            }
        }
        private void ChartChange(object sender, EventArgs e)
        {
            ChartList c;
            XElement x = null;
            if(sender !=null)
            {
                chartName = ((ChartList)(((ToolStripMenuItem)sender).Tag)).Name;
                x = ((ChartList)(((ToolStripMenuItem)sender).Tag)).Definition;
            }
            foreach (ToolStripMenuItem item in miCharts.DropDownItems)
            {
                c = (ChartList)item.Tag;
                item.Checked = c.Name.ToUpper() == chartName.ToUpper();
                if (item.Checked)
                {
                    chartName = c.Name;
                    x = c.Definition;
                }
            }
            chartDefinition = new XElement(x);
            ValidateChart();
            chart1.SetDefinition(chartDefinition);
            chart1.DataBind();
        }
        private void AlgorithmChange(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem i in AlgorithmMenu.DropDownItems)
            {
                i.Checked = false;
                ((FlowLayoutPanel)i.Tag).Visible = false;
            }
            var item = (ToolStripMenuItem)sender;
            item.Checked = true;
            ((FlowLayoutPanel)item.Tag).Visible = true;
            if (AlgorithmMenu.Text != item.Text)
            {
                AlgorithmMenu.Text = item.Text;
                IndicatorClass = (IndicatorClass)((FlowLayoutPanel)item.Tag).Tag;
                currentPanel = (FlowLayoutPanel)item.Tag;
                ReloadData();
            }
        }
        private void Parameter_ValueChanged(object sender, EventArgs e)
        {
            ReloadData();
        }
        void LoadAlgoritms()
        {
            TabPage containter = details.ParameterContainer;
            AlgorithmMenu.DropDownItems.Clear();
            for (int i = 0; i < IndicatorClass.IndicatorClasses.Count; i++)
            {
                var item = new ToolStripMenuItem(IndicatorClass.IndicatorClasses[i].IndicatorName) { Tag = i, Checked = false };
                item.Click += AlgorithmChange;
                if (IndicatorClass.IndicatorClasses[i].IndicatorName == "Default")
                {
                    item.Checked = true;
                    AlgorithmMenu.Text = item.Text;
                    IndicatorClass = IndicatorClass.IndicatorClasses[i];
                }
                AlgorithmMenu.DropDownItems.Add(item);
                FlowLayoutPanel panel = new FlowLayoutPanel() { Visible = item.Checked};
                panel.Tag = IndicatorClass.IndicatorClasses[i];
                containter.Controls.Add(panel);
                panel.Dock = DockStyle.Fill;
                item.Tag = panel;
                ParameterControl pc;
                foreach(var p in IndicatorClass.IndicatorClasses[i].Class.GetProperties().OrderBy(x=>x.Name))
                {
                    foreach(var attr in p.GetCustomAttributes(true))
                    {
                        if(attr is InputIntAttribute)
                        {
                            pc = new ParameterControl();
                            panel.Controls.Add(pc);
                            pc.label.Text = p.Name;
                            pc.Value.Minimum = (decimal)((InputIntAttribute)attr).FromValue;
                            pc.Value.Maximum = (decimal)((InputIntAttribute)attr).ToValue;
                            pc.Value.Value = (decimal)((InputIntAttribute)attr).DefaultValue;
                            pc.PropertyName = p.Name;
                            pc.IndicatorClass = IndicatorClass.IndicatorClasses[i];
                            pc.Value.ValueChanged += new EventHandler(Parameter_ValueChanged);
                        }
                        if (attr is InputDoubleAttribute)
                        {
                            pc = new ParameterControl();
                            panel.Controls.Add(pc);
                            pc.label.Text = p.Name;
                            pc.Value.Minimum = (decimal)((InputDoubleAttribute)attr).FromValue;
                            pc.Value.Maximum = (decimal)((InputDoubleAttribute)attr).ToValue;
                            pc.Value.Value = (decimal)((InputDoubleAttribute)attr).DefaultValue;
                            pc.PropertyName = p.Name;
                            pc.IndicatorClass = IndicatorClass.IndicatorClasses[i];
                            pc.Value.ValueChanged += new EventHandler(Parameter_ValueChanged);
                        }
                    }
                }
                
            }
        }
        public ChartForm()
        {
            Token = Methods.GetToken();
            InitializeComponent();
            labelIsLoading.Visible = false;
            
            hScrollBar.Minimum = 1;
            details = new ChartDetailForm();
            details.Owner = this;
            details.chart = this.chart1;
            details.ChartForm = this;
            details.OnSearchConfirm += new EventHandler(OnSearchConfirm);
            chartPropertySelected = details.chartPropertySelected;
            chartPropertyAll = details.chartPropertyAll;
            chartPropertyAll.SelectedObject = chart1;

            LoadAlgoritms();

            nInterval.NumericUpDownControl.Minimum = 1;
            nInterval.NumericUpDownControl.Maximum = 99999999;
            nInterval.NumericUpDownControl.Value = Interval;
            dpStartDate.Value = DateTime.Now.AddYears(-1);
            nScale.NumericUpDownControl.Minimum = 1;
            nScale.NumericUpDownControl.Maximum = 1000;
            nScale.NumericUpDownControl.Value = numberOfRows;
            hScrollBar.Minimum = 0;
            hScrollBar.Maximum = 1;
            hScrollBar.Value = 1;
            nLargeChange.Value = 10;
            hScrollBar.LargeChange = 10;
            stopLoading = false;
            ReloadData();
            instanceStart = false;
            LoadChartList();
        }
        void SetCursorLocation()
        {
            
            if (startLocation<=0)
		        startLocation = 200000000;
            if (CursorSize < startLocation + numberOfRows)
                startLocation = CursorSize - numberOfRows;
            if (startLocation < 1)
                startLocation = 0;
            PopulateDataview();
            ValidateChart();
            chart1.DataBind();
        }
        void PopulateDataview()
        {
            //Console.WriteLine("Start scrolling {0}", startLocation);
            chartData.Rows.Clear();
            for (int i = startLocation; (i < startLocation + numberOfRows) && (i < data.Rows.Count); i++)
            {
                var row = chartData.NewRow();
                var s = data.Rows[i];
                for (int j = 0; j < data.Columns.Count; j++)
                    row[j] = s[j];
                chartData.Rows.Add(row);
            }
            //dataView.RowFilter = "___RowNumber___>=" + startLocation.ToString() + " and ___RowNumber___<" + (startLocation + numberOfRows).ToString();
            //Console.WriteLine("Scrolling {0} done.{1}--{2}", startLocation, dataView.ToTable().Rows[0]["___RowNumber___"], dataView.ToTable().Rows[dataView.ToTable().Rows.Count - 1]["___RowNumber___"]);
        }
        void ReloadData(bool forceReloading = false)
        {
            try
            {
                startLocation = -1;
                labelIsLoading.Visible = true;
                Application.DoEvents();
                if (stopLoading)
                    return;
                if (SymbolID < 0)
                    return;
                

                using (Indicator indicator = (Indicator)Activator.CreateInstance(IndicatorClass.Class))
                {
                    indicator.SetDefaultValues();
                    indicator.StartDate = StartDate;
                    indicator.SymbolID = SymbolID;
                    indicator.Interval = Interval;
                    indicator.IntervalType = IntervalType;
                    if(currentPanel!=null)
                    {
                        foreach(ParameterControl pc in currentPanel.Controls)
                        {
                            if (IndicatorClass.Class.GetProperty(pc.PropertyName).PropertyType == typeof(int))
                                indicator.SetPropertyValue(pc.PropertyName, pc.IntValue);
                            if (IndicatorClass.Class.GetProperty(pc.PropertyName).PropertyType == typeof(double))
                                indicator.SetPropertyValue(pc.PropertyName, pc.DoubleValue);
                        }
                    }
                    //indicator.WriteToServer();
                    //Console.WriteLine("Start fetching {0}", SymbolID);
                    DateTime ddd = DateTime.Now;
                    data = indicator.WriteToDataTable(data);
                    Console.WriteLine("Fetching {0} done. Size = {1}. Duration = {2:0.000} ms", SymbolID, data.Rows.Count, (DateTime.Now - ddd).TotalMilliseconds);
                    if(chartData.Columns.Count + 1 != data.Columns.Count )
                    {
                        chartData = data.Clone();
                        chartData.Columns.Add("___XDisplay___", typeof(string));
                    }
                    //dataView.Table = data;
                    
                }
                SetCursorLocation();
                hScrollBar.Minimum = 0;
                hScrollBar.Maximum = Math.Max(1, CursorSize - numberOfRows);
                startLocation = hScrollBar.Maximum;
                hScrollBar.Value = startLocation;
                chart1.DataSource = chartData;
                ValidateChart();
                chart1.DataBind();
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
            //Console.WriteLine("{0}/{1}", hScrollBar.Value, hScrollBar.Maximum);
            startLocation = hScrollBar.Value;
            //ReloadData();
            
            SetCursorLocation();
            
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            chartDefinition = chart1.GetDefinition();
            var item = ChartList.Charts.FirstOrDefault(x => x.Name.ToUpper() == chartName.ToUpper());
            if(item !=null)
            {
                item.Definition = chartDefinition;
                item.Save();
            }
            else
            {
                (new ChartList() { Name = chartName, Definition = chartDefinition }).Save();
                LoadChartList();
            }
        }

        private void miSaveAs_Click(object sender, EventArgs e)
        {
            if(Methods.InputBox("Save as a new chart", "ChartName", ref chartName) == DialogResult.OK)
            {
                miSave_Click(sender, e);
            }
        }

        private void nLargeChange_ValueChanged(object sender, EventArgs e)
        {
            hScrollBar.LargeChange = (int)nLargeChange.Value;
        }
        bool IsSplitter(string s)
        {
            return (s.Length > SplitterName.Length) && (s.Substring(0, SplitterName.Length).ToUpper() == SplitterName.ToUpper());
        }
        public void SetSplitter(int id = 0)
        {
            if (id == chart1.ChartAreas.Count)
                return;
            if(id == 0)
            {
                if(IsSplitter(chart1.ChartAreas[id].Name))
                {
                    chart1.ChartAreas.RemoveAt(id);
                    SetSplitter();
                }
                else
                {
                    SetSplitter(1);
                }
                return;
            }
            else if(id == chart1.ChartAreas.Count-1)
            {
                if (IsSplitter(chart1.ChartAreas[id].Name))
                {
                    chart1.ChartAreas.RemoveAt(id);
                    return;
                }
            }
            if (IsSplitter(chart1.ChartAreas[id - 1].Name) && IsSplitter(chart1.ChartAreas[id].Name))
            {
                chart1.ChartAreas.RemoveAt(id);
                SetSplitter(id);
            }
            else if ((!IsSplitter(chart1.ChartAreas[id - 1].Name)) && (!IsSplitter(chart1.ChartAreas[id].Name)))
            {
                ChartArea ca = new ChartArea(SplitterName + "-" + Guid.NewGuid().ToString());
                ca.Position.Height = 0.1f;
                ca.BackColor = Color.Black;
                ca.Position.Width = 100;
                ca.Position.Y = chart1.ChartAreas[id].Position.Y;
                chart1.ChartAreas.Insert(id, ca);
            }
            else if ((!IsSplitter(chart1.ChartAreas[id - 1].Name)) && IsSplitter(chart1.ChartAreas[id].Name))
            {
                id++;
                SetSplitter(id);
            }
            else if ((IsSplitter(chart1.ChartAreas[id - 1].Name)) && (!IsSplitter(chart1.ChartAreas[id].Name)))
            {
                id++;
                SetSplitter(id);
            }
        }
        public void RearrangeSplitters()
        {
            ChartArea current, previous = null;
            for(int i = 0; i<chart1.ChartAreas.Count; i++)
            {
                current = chart1.ChartAreas[i];
                if (i == 0)
                {
                    current.Position.Y = 0;
                    continue;
                }
                previous = chart1.ChartAreas[i - 1];
                current.Position.Y = previous.Position.Y + previous.Position.Height;
                
            }
        }
        public ChartArea GetPreviousChartArea(ChartArea current)
        {
            int i = chart1.ChartAreas.IndexOf(current);
            return i == 0 ? null : chart1.ChartAreas[i - 1];
        }
        public ChartArea GetNextChartArea(ChartArea current)
        {
            int i = chart1.ChartAreas.IndexOf(current);
            return i == chart1.ChartAreas.Count -1 ? null : chart1.ChartAreas[i + 1];
        }
        DateTime lastMouseMove = DateTime.Now;
        bool mouseDown = false;
        ChartArea splitterChartArea = null;
        private void ShowValues(HitTestResult hitTest, MouseEventArgs e)
        {
            if (hitTest == null)
                return;
            string DateTimeFormat = "{0:yyyy-MM-dd hh:mm}";
            string NumberFormat1 = "{0:0,0.00}";
            string NumberFormat2 = "{0:0,0}";
            string NumberFormat3 = "{0:0,0.0000}";
            if (hitTest.PointIndex >= 0)
            {
                if (chartData.Rows.Count > hitTest.PointIndex)
                {
                    var row = chartData.Rows[hitTest.PointIndex];
                    details.tDateFrom.Text = string.Format(DateTimeFormat, row["DateFrom"]);
                    details.tDateTo.Text = string.Format(DateTimeFormat, row["DateTo"]);
                    details.tHigh.Text = string.Format(NumberFormat1, row["High"]);
                    details.tLow.Text = string.Format(NumberFormat1, row["Low"]);
                    details.tOpen.Text = string.Format(NumberFormat1, row["Open"]);
                    details.tClose.Text = string.Format(NumberFormat1, row["Close"]);
                    details.tVolume.Text = string.Format(NumberFormat2, row["Volume"]);
                    details.tTypical.Text = string.Format(NumberFormat1, row["TypicalPrice"]);
                    //populate list views
                    var list = details.lvData;
                    list.Groups.Clear();
                    list.Items.Clear();
                    ListViewGroup group= null;
                    string fieldName;
                    foreach(ChartArea ca in chart1.ChartAreas)
                    {
                        foreach (var ii in chart1.Series.Where(x => x.ChartArea == ca.Name).Select(x => new {x, x.YValueMembers}).Where(x => x.YValueMembers != "").OrderBy(x => x.YValueMembers))
                        {
                            fieldName = ii.YValueMembers;
                            if ((fieldName != "DateFrom") && (fieldName != "DateTo") && (fieldName != "High") && (fieldName != "Low") && (fieldName != "Open")
                                && (fieldName != "Close") && (fieldName != "Volume") && (fieldName != "TypicalPrice") && (fieldName != "High,Low,Open,Close"))
                            {

                                group = null;
                                foreach(ListViewGroup g in list.Groups)
                                {
                                    if(g.Header == ca.Name)
                                    {
                                        group = g;
                                        break;
                                    }
                                }
                                if(group==null)
                                    group = list.Groups.Add(ca.Name, ca.Name);
                                var item = new ListViewItem();
                                if (ii.x.Color != Color.White)
                                    item.ForeColor = ii.x.Color;
                                else
                                    item.ForeColor = Color.Black;
                                item.Text = fieldName;
                                item.SubItems.Add(string.Format(NumberFormat3, row[fieldName]));
                                item.Group = group;
                                group.Items.Add(item);
                                list.Items.Add(item);
                            }
                        }
                    }
                }
            }
            if (hitTest.ChartArea != null)
            {
                details.tY1.Text = string.Format(NumberFormat1, hitTest.ChartArea.AxisY.PixelPositionToValue(e.Y));
                details.tY2.Text = string.Format(NumberFormat1, hitTest.ChartArea.AxisY2.PixelPositionToValue(e.Y));
            }
        }
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            DateTime now = DateTime.Now;
            if ((now - lastMouseMove).TotalMilliseconds < 50)
            {
                return;
            }
            lastMouseMove = now;
            #region move splitter
            var hitTest = chart1.HitTest(e.X, e.Y);
            ShowValues(hitTest, e);
            if(hitTest!=null)
            {
                if (
                        (hitTest.ChartElementType == ChartElementType.PlottingArea)
                    && IsSplitter(hitTest.ChartArea.Name)
                    )
                {
                    chart1.Cursor = Cursors.HSplit;
                    if (mouseDown)
                        splitterChartArea = hitTest.ChartArea;
                }
                if (splitterChartArea!=null)
                {
                    ChartArea p = GetPreviousChartArea(splitterChartArea);
                    ChartArea n = GetNextChartArea(splitterChartArea);
                    float y = Convert.ToSingle(e.Y) * 100 / chart1.Height;
                    if (y < p.Position.Y + 1)
                        y = p.Position.Y + 1;
                    if (y > n.Position.Y + n.Position.Height - 1)
                        y = n.Position.Y + n.Position.Height - 1;
                    splitterChartArea.Position.Y = y ;

                }
            }
            #endregion

            #region Draw cursor
            Point mousePoint = new Point(e.X, e.Y);
            foreach(ChartArea ca in chart1.ChartAreas)
            {
                if((ca.Name.Length > SplitterName.Length)&&(ca.Name.Substring(1, SplitterName.Length).ToUpper() == SplitterName.ToUpper()))
                    continue;
                //ca.CursorX.Interval = 0;
                ca.CursorX.LineDashStyle = ChartDashStyle.Dot;
                ca.CursorX.SetCursorPixelPosition(mousePoint, true);

                //ca.CursorY.Interval = 0;
                ca.CursorY.LineDashStyle = ChartDashStyle.Dot;
                ca.CursorY.SetCursorPixelPosition(mousePoint, true);
            }
            #endregion


        }

        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            if (splitterChartArea!=null)
            {
                var c = GetPreviousChartArea(splitterChartArea);
                c.Position.Height = splitterChartArea.Position.Y - c.Position.Y;
                c = GetNextChartArea(splitterChartArea);
                var h = c.Position.Height + c.Position.Y;
                c.Position.Y = splitterChartArea.Position.Y + splitterChartArea.Position.Height;
                c.Position.Height = h - c.Position.Y;
            }
            splitterChartArea = null;
            chart1.Cursor = Cursors.Default;
            details.chartPropertySelected.SelectedObject = chart1.HitTest(e.X, e.Y);
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
        }

        
       
       
    }
}
