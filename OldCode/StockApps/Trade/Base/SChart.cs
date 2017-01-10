using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trade.Base
{
    public partial class SChart : UserControl
    {
        ChartBufferItem<Canvas> canvasBuffer;
        private Canvas GetCanvas()
        {
            //return canvasBuffer.Get();
            return new Canvas();
        }
        private void ClearCanvas()
        {
            canvasBuffer.Reset();
            canvasBuffer.Clear();
            foreach (Canvas c in canvasBuffer.Buffer)
                c.Clear();
        }
        System.Windows.Controls.Label leftCursorLabel, rightCursorLabel;
        int lastDataIndex = -1;
        List<Trade.Base.Canvas> queue = null;
        public SChart()
        {
            canvasBuffer = new ChartBufferItem<Canvas>();
            InitializeComponent();
            m_Charts = new List<Trade.Base.Canvas>();
            queue = new List<Canvas>();
            sbCurrent.Value = 0;
            ChartData cd = new ChartData();
            cd.Name = "XAXIS";
            cd.ZeroLocation = ZeroYLocation.Bottom;
            cd.YAxisLocation = YAxisLocation.Left;
            cd.Color = System.Windows.Media.Colors.Black;
            cd.Type = ChartType.XAxis;
            cd.Data.Add(new List<object>());
            XAxisChart.Data.Add(cd);
            m_MinItemWidth = 6;
            m_MaxItemWidth = 25;
            m_DefaultItemWidth = 6;

            leftCursorLabel = new System.Windows.Controls.Label();
            leftCursorLabel.FontSize = 10;
            //leftCursorLabel.FontWeight = System.Windows.FontWeights.Bold;
            leftCursorLabel.Foreground = System.Windows.Media.Brushes.Blue;
            leftCursorLabel.Tag = "leftCursorLabel";
            leftCursorLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            leftCursorLabel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            leftCursorLabel.Padding = new System.Windows.Thickness(1, 1, 0, 0);
            leftCursorLabel.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
            leftCursorLabel.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            leftCursorLabel.Visibility = System.Windows.Visibility.Hidden;
            XAxisChart.C.Children.Add(leftCursorLabel);
            
            //leftCursorLabel.Opacity = 0.7;
            
            rightCursorLabel = new System.Windows.Controls.Label();
            rightCursorLabel.FontSize = 10;
            //rightCursorLabel.FontWeight = System.Windows.FontWeights.Bold;
            rightCursorLabel.Foreground = System.Windows.Media.Brushes.Blue;
            rightCursorLabel.Tag = "rightCursorLabel";
            rightCursorLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            rightCursorLabel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            rightCursorLabel.Padding = new System.Windows.Thickness(0, 1, 1, 0);
            rightCursorLabel.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;
            rightCursorLabel.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            rightCursorLabel.Visibility = System.Windows.Visibility.Hidden;
            XAxisChart.C.Children.Add(rightCursorLabel);
            //rightCursorLabele.Opacity = 0.7;
            sc0.Panel2Collapsed = true;
            m_CurrentIndex = -1;
            lvData.Items.Clear();
            lvData.Groups.Clear();
        }
        private void PositionXY(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Point p = ((Trade.Base.Canvas)sender).C.PointToScreen(e.GetPosition(((Trade.Base.Canvas)sender).C));
            foreach (var c in Charts)
                c.ChangeXYPosition(p);
            xAxisChart.ChangeXYPosition(p);
        }
        [Browsable(false)]
        public Trade.Base.Canvas XAxisChart
        {
            get { return xAxisChart; }
        }
        List<Trade.Base.Canvas> m_Charts;
        [Browsable(false)]
        public List<Trade.Base.Canvas> Charts
        {
            get { return m_Charts; }
        }
        public void AdjustSizes()
        {
            if (Charts.Count == 0)
                return;
            double total = Charts.Sum(x => { return x.MHeight; });
            int distance = sc1.Height;
            foreach (var c in Charts)
            {
                if (((SplitContainer)c.Tag1).Panel2 == c.Tag2)
                {
                    distance = distance - (int)(((double)sc1.Height - 2) * c.MHeight / total);
                    if (distance >= ((SplitContainer)c.Tag1).Height)
                        return;
                    ((SplitContainer)c.Tag1).SplitterDistance = distance;
                }
            }
        }
        private void DrawChart(bool draw)
        {
            if (draw)
            {
                foreach (var c in Charts)
                {
                    c.DrawChart = draw;
                }
                sc0.Visible = draw;
                foreach (var c in Charts)
                {
                    c.RepaintChart();
                }
                return;
            }
            sc0.Visible = draw;
            foreach (var c in Charts)
            {
                c.DrawChart = draw;
            }
            
        }
        public void SetXAxisChartData(List<object> d)
        {
            this.Clear();
            XAxisChart.Data[0].Data.Clear();
            XAxisChart.Data[0].Data.Add(d);
        }
        public void ShowCharts(bool toEnd)
        {
            SetSBCurrentRange();
            System.Windows.Forms.SplitContainer sc, cc;
            try
            {

                DrawChart(false);
                HideCharts();
                sc = sc1;
                
                sc.BorderStyle = BorderStyle.Fixed3D;
                sc.SplitterWidth = 1;
                sc.Panel1Collapsed = false;
                sc.SplitterDistance = sc.Height - 3;
                foreach (var c in Charts)
                {
                    if (sc.Panel2.Controls.Count == 0)
                    {
                        c.Tag1 = sc;
                        c.Tag2 = sc.Panel2;
                        sc.Panel2.Controls.Add(c);
                        c.Dock = DockStyle.Fill;
                        c.ItemWidth = ItemWidth;
                        c.Current = Current;
                    }
                    else
                    {
                        cc = sc;
                        sc = new SplitContainer(){Orientation = Orientation.Horizontal,  SplitterWidth = 1};
                        sc.BorderStyle = BorderStyle.Fixed3D;
                        cc.Panel1.Controls.Add(sc);
                        sc.SplitterDistance = sc.Height - 3;
                        sc.Dock = DockStyle.Fill;
                        sc.Panel2.Controls.Add(c);
                        c.Tag1 = sc;
                        c.Tag2 = sc.Panel2;
                        c.Dock = DockStyle.Fill;
                        c.ItemWidth = ItemWidth;
                        c.Current = Current;
                    }
                }
                if (sc.Panel1.Controls.Count == 0)
                    sc.Panel1Collapsed = true;
                AdjustSizes();
            }
            finally
            {
                DrawChart(true);
                GC.Collect();
            }
            if (toEnd)
            {
                Current = this.XAxisData.Count - ItemCount + 10;
            }
        }
        public void RepaintChart()
        {
            xAxisChart.RepaintChart();
            foreach (Trade.Base.Canvas c in Charts)
            {
                c.RepaintChart();
            }
        }
        [Browsable(true), Category("Chart")]
        public int ItemWidth
        {
            get { return xAxisChart.ItemWidth; }
            set 
            {
                int v = value;
                if (xAxisChart.ItemWidth != v)
                {
                    if (v > m_MaxItemWidth)
                        v = m_MaxItemWidth;
                    if (v < m_MinItemWidth)
                        v = m_MinItemWidth;
                    xAxisChart.ItemWidth = v;
                    foreach (var c in Charts)
                        c.ItemWidth = v;
                    tbItemSize.Text = ItemWidth.ToString();
                }
            }
        }
        [Browsable(false), Category("Chart")]
        public int ItemCount
        {
            get { return XAxisChart.ItemCount; }
        }
        [Browsable(false), Category("Chart")]
        public int Current
        {
            get { return xAxisChart.Current; }
            set
            {
                SetSBCurrentRange();
                int v = value;
                if (v <= 0) v = 0;
                if (sbCurrent.Maximum <= v)
                    v = sbCurrent.Maximum;
                if (xAxisChart.Current != v)
                {
                    xAxisChart.Current = v;
                    foreach (var c in Charts)
                        c.Current = v;
                    sbCurrent.Value = Current;
                }
            }
        }
        void SetSBCurrentRange()
        {
            int i = 0;
            if(sbCurrent.Minimum != 0)
                sbCurrent.Minimum = 0;
            if (XAxisChart.Data.Count == 0)
                i = 0;
            else if (XAxisChart.Data[0].Data.Count == 0)
                i = 0;
            else
                i = XAxisChart.Data[0].Data[0].Count - 1;
            if (i < 0)
                i = 0;
            if (sbCurrent.Maximum != i)          
                sbCurrent.Maximum = i;
        }
        int m_DefaultItemWidth;
        [Browsable(true), Category("Chart")]
        public int DefaultItemWidth
        {
            get { return m_DefaultItemWidth; }
            set { m_DefaultItemWidth = value;}
        }
        int m_MaxItemWidth;
        [Browsable(true), Category("Chart")]
        public int MaxItemWidth
        {
            get { return m_MaxItemWidth; }
            set
            {
                m_MaxItemWidth = value;
                if (ItemWidth > m_MaxItemWidth)
                {
                    ItemWidth = m_MaxItemWidth;
                    tbItemSize.Text = ItemWidth.ToString();
                }
            }
        }
        int m_MinItemWidth;
        [Browsable(true), Category("Chart")]
        public int MinItemWidth
        {
            get { return m_MinItemWidth; }
            set
            {
                m_MinItemWidth = value;
                if (ItemWidth < m_MinItemWidth)
                {
                    ItemWidth = m_MinItemWidth;
                    tbItemSize.Text = ItemWidth.ToString();
                }
            }
        }
        [Browsable(true), Category("Chart")]
        public List<object> XAxisData
        {
            get { return xAxisChart.Data[0].Data[0]; }
        }
        private void btnItemSizePlus_Click(object sender, EventArgs e)
        {
            ItemWidth++;
        }
        private void btnItemSizeMinus_Click(object sender, EventArgs e)
        {
            ItemWidth--;
        }
        private void sbCurrent_Scroll(object sender, ScrollEventArgs e)
        {
            Current = sbCurrent.Value;
        }
        public Trade.Base.Canvas AddNew(ChartData cd)
        {
            //Trade.Base.Canvas ret = new Canvas();
            Trade.Base.Canvas ret = GetCanvas();
            ret.Data.Add(cd);
            ret.CanvasMouseMove += new System.Windows.Input.MouseEventHandler(this.PositionXY);
            ret.ChartXY += new ChartXY(this.OnChartXY);
            ret.ChartSelected += new EventHandler(ChartSelected);
            ret.ChartDeselected += new EventHandler(ChartDeselected);
            Charts.Add(ret);
            return ret;
        }
        public Trade.Base.Canvas AddNewAtBeginning(ChartData cd)
        {
            //Trade.Base.Canvas ret = new Canvas();
            Trade.Base.Canvas ret = GetCanvas();
            ret.Data.Add(cd);
            ret.CanvasMouseMove += new System.Windows.Input.MouseEventHandler(this.PositionXY);
            ret.ChartXY += new ChartXY(this.OnChartXY);
            if (Charts.Count == 0)
                Charts.Add(ret);
            else
                Charts.Insert(0, ret);
            return ret;
        }
        private void OnChartXY(object sender, EventChartXYArgs e)
        {

            this.Focus();
            double k = 0;
            string s = "";
            if (e.Label == "Left")
            {
                
                if (!double.IsNaN(e.Y))
                {
                    leftCursorLabel.Visibility = System.Windows.Visibility.Visible;
                    leftCursorLabel.SetValue(System.Windows.Controls.Canvas.LeftProperty, 0d);
                    leftCursorLabel.SetValue(System.Windows.Controls.Canvas.TopProperty, 0d);
                    k = e.Y;
                    if (k > 1000)
                    {
                        k = k / 1000;
                        s = "K";
                    }
                    if (k > 1000)
                    {
                        k = k / 1000;
                        s = "M";
                    }
                    leftCursorLabel.Content = k.ToString("0.000")+s;
                }
                else
                    leftCursorLabel.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (e.Label == "Right")
            {
                if (!double.IsNaN(e.Y))
                {
                    rightCursorLabel.Visibility = System.Windows.Visibility.Visible;
                    rightCursorLabel.SetValue(System.Windows.Controls.Canvas.RightProperty, 0d);
                    rightCursorLabel.SetValue(System.Windows.Controls.Canvas.TopProperty, 0d);
                    /*k = e.Y;
                    if (k > 1000)
                    {
                        k = k / 1000;
                        s = "K";
                    }
                    if (k > 1000)
                    {
                        k = k / 1000;
                        s = "M";
                    }*/
                    rightCursorLabel.Content = FormatString(e.Y); //k.ToString("0.000") + s;
                }
                else
                    rightCursorLabel.Visibility = System.Windows.Visibility.Hidden;
            }
            DoChartXY(e);
            if (!double.IsNaN(e.X))
                ShowData((int)e.X);
        }
        private string FormatString(double k)
        {
            if (k <= 1000)
            {
                return k.ToString("0.000");
            }
            else if (k > 1000 && k < 1000000)
            {
                k = k / 1000;
                return k.ToString("0.000") + "K";
            }
            k = k / 1000000;
            return k.ToString("0.000") + "M";
        }
        public void Swap(int index1, int index2)
        {
            if (index1 < 0 || index1 >= Charts.Count || index2 < 0 || index2 >= Charts.Count || index1 == index2)
                return;
            foreach(var v in Charts)
            {
                if (v.Tag2 != null)
                {
                    try
                    {
                        v.MHeight = ((SplitterPanel)v.Tag2).Height;
                    }
                    catch { }
                }
            }


            Trade.Base.Canvas a = Charts[index1], b = Charts[index2];
            SplitContainer c = (SplitContainer)Charts[index1].Tag1; SplitterPanel d = (SplitterPanel)Charts[index1].Tag2;
            SplitContainer e = (SplitContainer)Charts[index2].Tag1; SplitterPanel f = (SplitterPanel)Charts[index2].Tag2;

            a.Tag1 = e; a.Tag2 = f;
            b.Tag1 = c; b.Tag2 = d;
            while (d.Controls.Count>0)
                d.Controls.Remove(d.Controls[0]);
            while (f.Controls.Count > 0)
                f.Controls.Remove(f.Controls[0]);

            ((SplitterPanel)Charts[index1].Tag2).Controls.Add(Charts[index1]);
            ((SplitterPanel)Charts[index2].Tag2).Controls.Add(Charts[index2]);
            Charts[index1] = b;
            Charts[index2] = a;

            AdjustSizes();
        }
        public event ChartXY ChartXY;
        void DoChartXY(EventChartXYArgs e)
        {
            if (ChartXY != null)
                ChartXY(this, e);
        }
        public void HideCharts()
        {
            ClearCanvas();
            Control c;
            while (sc1.Panel1.Controls.Count > 0)
            {
                c = sc1.Panel1.Controls[0];
                sc1.Panel1.Controls.Remove(sc1.Panel1.Controls[0]);
                c.Dispose();
            }
            while (sc1.Panel2.Controls.Count > 0)
                sc1.Panel2.Controls.Remove(sc1.Panel2.Controls[0]);
            sc1.Panel1Collapsed = true;
            
        }
        public void Clear()
        {
            DrawChart(false);
            HideCharts();
            Current = 0;
            XAxisData.Clear();
            xAxisChart.Clear();
            xAxisChart.Current = 0;
            Charts.Clear();
            leftCursorLabel.Content = "";
            rightCursorLabel.Content = "";
            xAxisChart.leftCursorLabel.Content = "";
            xAxisChart.rightCursorLabel.Content = "";
            sbCurrent.Maximum = 0;
            GC.Collect();
        }

        private void ChartSelected(object sender, EventArgs e)
        {
            queue.Add((Trade.Base.Canvas)sender);
            while (queue.Count > 2)
                queue[0].Selected = false;
        }

        private void ChartDeselected(object sender, EventArgs e)
        {
            queue.Remove((Trade.Base.Canvas)sender);
            
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            menu.Show(btnMenu, new Point(0,0));
        }

        private void miSizeNormal_Click(object sender, EventArgs e)
        {
            ItemWidth = DefaultItemWidth;
        }

        private void miSwapChart_Click(object sender, EventArgs e)
        {
            Swap(Charts.IndexOf(queue[0]), Charts.IndexOf(queue[1]));
            miDeselectAll_Click(sender, e);
        }

        private void miDeselectAll_Click(object sender, EventArgs e)
        {
            while (queue.Count > 0)
                queue[0].Selected = false;
        }

        private void menu_Opening(object sender, CancelEventArgs e)
        {
            miSwapChart.Enabled = queue.Count == 2;
            miDeselectAll.Enabled = queue.Count > 0;
        }

        private void xAxisChart_ChartSelected(object sender, EventArgs e)
        {
            xAxisChart.Selected = false;
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            this.Current += -e.Delta / 10;
            Point p = PointToScreen(new Point(e.X, e.Y));
            XAxisChart.ChangeXYPosition(p);
            foreach (var c in Charts)
                c.ChangeXYPosition(p);
        }
        public string Caption
        {
            get { return xAxisChart.Name; }
            set { xAxisChart.Name = value; }
        }
        private void lvDataCreateItem(int kk)
        {
            if (lvData.Items.Count < kk + 1)
            {
                lvData.Items.Add(new ListViewItem());
                if (lvData.Items[kk].SubItems.Count <= 1)
                    lvData.Items[kk].SubItems.Add(new ListViewItem.ListViewSubItem());
                lvData.Items[kk].Group = lvData.Groups[0];
            }
        }
        private void ShowData(int index)
        {
            if (lastDataIndex == index) return;
            if (!(index >= 0 && index < XAxisData.Count()))
                return;
            try
            {
                if (sc0.Panel2Collapsed)
                    return;
                if (lvData.Groups.Count != 1)
                    lvData.Groups.Clear();
                if (lvData.Groups.Count == 0)
                    lvData.Groups.Add(new ListViewGroup());
                lvData.Groups[0].Header = ((DateTime)XAxisData[index]).ToString("yyyy-MM-dd");
                int kk = 0;
                foreach (var ch in Charts)
                {
                    foreach (var cd in ch.Data)
                    {
                        if (cd.Type == ChartType.Bar || cd.Type == ChartType.Line || cd.Type == ChartType.Stripe || cd.Type == ChartType.CandleStick)
                        {
                            lvDataCreateItem(kk);
                            lvData.Items[kk].Text = cd.Name;
                            System.Drawing.Color c1 = System.Drawing.Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B);
                            System.Drawing.Color c2 = System.Drawing.Color.FromArgb(cd.DownColor.A, cd.DownColor.R, cd.DownColor.G, cd.DownColor.B);
                            System.Drawing.Color c3;
                            lvData.Items[kk].ForeColor = c1;
                            if (cd.Type == ChartType.Bar || cd.Type == ChartType.Line)
                                lvData.Items[kk].SubItems[1].Text = FormatString((double)cd.Data[0][index]);
                            else if (cd.Type == ChartType.CandleStick)
                            {
                                c3 = c1;
                                if (index > 0)
                                {
                                    if ((double)cd.Data[4][index] < (double)cd.Data[4][index - 1])
                                        c3 = c2;
                                }
                                lvData.Items[kk].ForeColor = c3;
                                lvData.Items[kk].SubItems[1].Text = "H: " + FormatString((double)cd.Data[0][index]);
                                kk++;
                                lvDataCreateItem(kk);
                                lvData.Items[kk].Text = cd.Name;
                                lvData.Items[kk].ForeColor = c3;
                                lvData.Items[kk].SubItems[1].Text = "O: " + FormatString((double)cd.Data[1][index]);
                                kk++;
                                lvDataCreateItem(kk);
                                lvData.Items[kk].Text = cd.Name;
                                lvData.Items[kk].ForeColor = c3;
                                lvData.Items[kk].SubItems[1].Text = "C: " + FormatString((double)cd.Data[2][index]);
                                kk++;
                                lvDataCreateItem(kk);
                                lvData.Items[kk].Text = cd.Name;
                                lvData.Items[kk].ForeColor = c3;
                                lvData.Items[kk].SubItems[1].Text = "L: " + FormatString((double)cd.Data[3][index]);
                                kk++;
                                lvDataCreateItem(kk);
                                lvData.Items[kk].Text = cd.Name;
                                lvData.Items[kk].ForeColor = c3;
                                lvData.Items[kk].SubItems[1].Text = "M: " + FormatString((double)cd.Data[4][index]);
                            }
                            else if (cd.Type == ChartType.Stripe)
                            {
                                lvData.Items[kk].Text = cd.Name;
                                lvData.Items[kk].ForeColor = c1;
                                lvData.Items[kk].SubItems[1].Text = "A: " + FormatString((double)cd.Data[0][index]);
                                kk++;
                                lvDataCreateItem(kk);
                                lvData.Items[kk].Text = cd.Name;
                                lvData.Items[kk].ForeColor = c1;
                                lvData.Items[kk].SubItems[1].Text = "B: " + FormatString((double)cd.Data[1][index]);
                            }
                            kk++;
                        }
                    }
                }
                
            }
            finally
            {
                lastDataIndex = index;
                m_CurrentIndex = index;
                if (CurrentIndexChanged != null)
                    CurrentIndexChanged(this, new EventArgs());
            }
        }
        private void miData_Click(object sender, EventArgs e)
        {
            miData.Checked = !miData.Checked;
            sc0.Panel2Collapsed = !miData.Checked;
        }
        private int m_CurrentIndex;
        [Browsable(false)]
        public int CurrentIndex
        {
            get { return m_CurrentIndex;}
        }
        [Browsable(true), Category("Chart")]
        public event EventHandler CurrentIndexChanged;

    }
    
}
