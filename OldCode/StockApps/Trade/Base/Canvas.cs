using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;

namespace Trade.Base
{
    public partial class Canvas : System.Windows.Forms.UserControl
    {
        ChartBuffer chartBuffer;
        Line lineX, lineY;
        System.Windows.Shapes.Rectangle rSelected = null;
        public System.Windows.Controls.Label leftCursorLabel, rightCursorLabel;
        public System.Windows.Controls.Label lName;
        public Canvas()
        {
            chartBuffer = new ChartBuffer();
            lineX = new System.Windows.Shapes.Line();
            lineY = new System.Windows.Shapes.Line();
            MHeight = 10;
            isPainting = false;
            m_ItemWidth = MINIMUM_WIDTH;
            InitializeComponent();
            delegateMouseMove  = new System.Windows.Input.MouseEventHandler(OnCanvasMouseMove);
            delegateMouseEnter = new System.Windows.Input.MouseEventHandler(OnCanvasMouseEnter);
            delegateMouseLeave = new System.Windows.Input.MouseEventHandler(OnCanvasMouseLeave);
            delegateMouseDown = new System.Windows.Input.MouseButtonEventHandler(OnCanvasMouseDown);
            delegateMouseLeftButtonDown = new System.Windows.Input.MouseButtonEventHandler(OnCanvasMouseLeftButtonDown);
            delegateMouseLeftButtonUp = new System.Windows.Input.MouseButtonEventHandler(OnCanvasMouseLeftButtonUp);
            delegateMouseRightButtonDown = new System.Windows.Input.MouseButtonEventHandler(OnCanvasMouseRightButtonDown);
            delegateMouseRightButtonUp = new System.Windows.Input.MouseButtonEventHandler(OnCanvasMouseRightButtonUp);
            delegateMouseUp = new System.Windows.Input.MouseButtonEventHandler(OnCanvasMouseUp);
            delegateMouseWheel = new System.Windows.Input.MouseWheelEventHandler(OnCanvasMouseWheel);
            delegateCanvasKeyDown = new System.Windows.Input.KeyEventHandler(OnCanvasKeyDown);
            delegateCanvasKeyUp = new System.Windows.Input.KeyEventHandler(OnCanvasKeyUp);
            AddEvent(wpfCanvas.canvas);
            m_Data = new List<ChartData>();
            m_Area = new System.Windows.Rect(0, 0, 0, 0);
            
            lineX.Stroke = System.Windows.Media.Brushes.Black;
            lineX.StrokeThickness = 1;
            lineX.X1 = 0;
            lineX.Y1 = 0;
            lineX.X2 = C.ActualWidth;
            lineX.Y2 = 0;
            lineX.SnapsToDevicePixels = true;

            
            lineY.Stroke = System.Windows.Media.Brushes.Black;
            lineY.StrokeThickness = 1;
            lineY.X1 = 0;
            lineY.Y1 = 0;
            lineY.X2 = 0;
            lineY.Y2 = C.ActualHeight;
            lineY.SnapsToDevicePixels = true;
            lineX.Opacity = 0.3;
            lineY.Opacity = 0.3;
            C.Children.Add(lineY);
            C.Children.Add(lineX);
            lineX.Tag = "LineX";
            lineY.Tag = "LineY";
            leftCursorLabel = new System.Windows.Controls.Label();
            leftCursorLabel.FontSize = 10;
            //leftCursorLabel.FontWeight = System.Windows.FontWeights.Bold;
            leftCursorLabel.Foreground = System.Windows.Media.Brushes.Blue;
            leftCursorLabel.Tag = "leftCursorLabel";
            leftCursorLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            leftCursorLabel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            leftCursorLabel.Padding = new Thickness(1, 1, 0, 0);
            leftCursorLabel.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
            leftCursorLabel.VerticalContentAlignment = VerticalAlignment.Center;
            leftCursorLabel.Visibility = Visibility.Hidden;
            //leftCursorLabel.Opacity = 0.7;
            C.Children.Add(leftCursorLabel);
            rightCursorLabel = new System.Windows.Controls.Label();
            rightCursorLabel.FontSize = 10;
            //rightCursorLabel.FontWeight = System.Windows.FontWeights.Bold;
            rightCursorLabel.Foreground = System.Windows.Media.Brushes.Blue;
            rightCursorLabel.Tag = "leftCursorLabel";
            rightCursorLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            rightCursorLabel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            rightCursorLabel.Padding = new Thickness(0, 0, 0, 0);
            rightCursorLabel.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;
            rightCursorLabel.VerticalContentAlignment = VerticalAlignment.Center;
            rightCursorLabel.Visibility = Visibility.Hidden;
            //rightCursorLabele.Opacity = 0.7;
            C.Children.Add(rightCursorLabel);
            rSelected = new System.Windows.Shapes.Rectangle();
            rSelected.Stroke = System.Windows.Media.Brushes.Red;
            rSelected.StrokeThickness = 3;
            rSelected.Opacity = 0.5;
            rSelected.Tag = "selected";
            rSelected.Visibility = Visibility.Hidden;

            lName = new System.Windows.Controls.Label();
            lName.Tag = "Name";
            lName.FontSize = 10;
            lName.Foreground = System.Windows.Media.Brushes.Black;
            lName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            lName.VerticalAlignment = VerticalAlignment.Top;
            lName.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            lName.VerticalContentAlignment = VerticalAlignment.Top;
            lName.Padding = new Thickness(0,0,0,0);
            C.Children.Add(lName);
            lName.SetValue(System.Windows.Controls.Canvas.RightProperty, 0d);
            lName.SetValue(System.Windows.Controls.Canvas.TopProperty, 0d);
            C.Children.Add(rSelected);
            timer1.Enabled = false;
            timer1.Interval = 500;
        }
        public string ChartName
        {
            get { return lName.Content.ToString(); }
            set { lName.Content = value; }
        }
        public event ChartXY ChartXY;
        string OnChartXY(double x, double y, string label, Trade.Base.Canvas c)
        {
            EventChartXYArgs p = new EventChartXYArgs(x, y, label, c);
            if (ChartXY != null)
                ChartXY(this, p);
            return p.Label;
        }
        void PaintXY(double x, double y)
        {
            lineX.Y1 = y;
            lineX.Y2 = y;
            lineY.X1 = x;
            lineY.X2 = x;
            lineY.Y2 = C.ActualHeight;
            lineX.X2 = C.ActualWidth;
            if (x < 0 || x > C.ActualWidth)
                lineY.Visibility = Visibility.Hidden;
            else
                lineY.Visibility = Visibility.Visible;
            if (y < 0 || y > C.ActualHeight)
                lineX.Visibility = Visibility.Hidden;
            else
                lineX.Visibility = Visibility.Visible;
            leftCursorLabel.Visibility = Visibility.Hidden;
            rightCursorLabel.Visibility = Visibility.Hidden;
            if (Data.Count == 0)
                return;
            else
            {
                if (Data.Exists(xx => { return (xx.Type == ChartType.XAxis); }))
                {
                    if (lineY.Visibility == Visibility.Visible && x > XMargin && x <= C.ActualWidth - XMargin)
                    {
                        leftCursorLabel.Opacity = 0.7;
                        x = x - XMargin;
                        x = Current + Math.Floor(x / ItemWidth);
                        DateTime t = (DateTime)Data[0].Data[0][(int)x];
                        string label = OnChartXY(x, double.NaN, t.ToString("yyyy-MM-dd"), this);
                        if (label != "")
                        {
                            leftCursorLabel.Content = label;
                            leftCursorLabel.SetValue(System.Windows.Controls.Canvas.LeftProperty, ConvertX((x - Current) * ItemWidth));
                            leftCursorLabel.SetValue(System.Windows.Controls.Canvas.TopProperty, 29d);
                            leftCursorLabel.Visibility = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    if (y >= YMargin && y <= C.ActualHeight - YMargin)
                    {
                        OnChartXY(double.NaN, double.NaN, "Left", this);
                        OnChartXY(double.NaN, double.NaN, "Right", this);
                        y = C.ActualHeight - YMargin - y;
                        if (Data.Exists(xx => { return (xx.YAxisLocation == YAxisLocation.Left) && !((xx.Type == ChartType.XAxis || xx.Type == ChartType.HorizontalLine || xx.Type == ChartType.VerticalLine)); }))
                        {
                            if (!(leftMaxY - leftMinY <= 0 || C.ActualHeight - 2*YMargin <=0))
                                OnChartXY(double.NaN, leftMinY + y * (leftMaxY - leftMinY) / (C.ActualHeight - 2 * YMargin), "Left", this);
                        }
                        if (Data.Exists(xx => { return (xx.YAxisLocation == YAxisLocation.Right) && !((xx.Type == ChartType.XAxis || xx.Type == ChartType.HorizontalLine || xx.Type == ChartType.VerticalLine)); }))
                        {
                            if (!(rightMaxY - rightMinY <= 0 || C.ActualHeight - 2 * YMargin <= 0))
                                OnChartXY(double.NaN, rightMinY + y * (rightMaxY - rightMinY) / (C.ActualHeight - 2 * YMargin), "Right", this);
                        }
                    }
                }
            }

        }
        public System.Windows.Controls.Canvas C
        {
            get { return wpfCanvas.canvas; }
        }

        private void AddEvent(FrameworkElement o)
        {
            o.MouseMove += delegateMouseMove;
            o.MouseEnter += delegateMouseEnter;
            o.MouseLeave += delegateMouseLeave;
            o.MouseDown += delegateMouseDown;
            o.MouseLeftButtonDown += delegateMouseLeftButtonDown;
            o.MouseLeftButtonUp += delegateMouseLeftButtonUp;
            o.MouseRightButtonDown += delegateMouseRightButtonDown;
            o.MouseRightButtonUp += delegateMouseRightButtonUp;
            o.MouseUp += delegateMouseUp;
            o.MouseWheel += delegateMouseWheel;
            o.KeyDown += delegateCanvasKeyDown;
            o.KeyUp += delegateCanvasKeyUp;
        }
        private void RemoveEvent(FrameworkElement o)
        {
            o.MouseMove -= delegateMouseMove;
            o.MouseEnter -= delegateMouseEnter;
            o.MouseLeave -= delegateMouseLeave;
            o.MouseDown -= delegateMouseDown;
            o.MouseLeftButtonDown -= delegateMouseLeftButtonDown;
            o.MouseLeftButtonUp -= delegateMouseLeftButtonUp;
            o.MouseRightButtonDown -= delegateMouseRightButtonDown;
            o.MouseRightButtonUp -= delegateMouseRightButtonUp;
            o.MouseUp -= delegateMouseUp;
            o.MouseWheel -= delegateMouseWheel;

            o.KeyDown -= delegateCanvasKeyDown;
            o.KeyUp -= delegateCanvasKeyUp;
        }

        //Private Variable
        private System.Windows.Input.MouseEventHandler delegateMouseMove;
        private System.Windows.Input.MouseEventHandler delegateMouseEnter;
        private System.Windows.Input.MouseEventHandler delegateMouseLeave;
        private System.Windows.Input.MouseButtonEventHandler delegateMouseDown;
        private System.Windows.Input.MouseButtonEventHandler delegateMouseLeftButtonDown;
        private System.Windows.Input.MouseButtonEventHandler delegateMouseLeftButtonUp;
        private System.Windows.Input.MouseButtonEventHandler delegateMouseRightButtonDown;
        private System.Windows.Input.MouseButtonEventHandler delegateMouseRightButtonUp;
        private System.Windows.Input.MouseButtonEventHandler delegateMouseUp;
        private System.Windows.Input.MouseWheelEventHandler delegateMouseWheel;
        private System.Windows.Input.KeyEventHandler delegateCanvasKeyDown;
        private System.Windows.Input.KeyEventHandler delegateCanvasKeyUp;
        //Event caller        
        void OnCanvasMouseMove(Object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (CanvasMouseMove != null)
                CanvasMouseMove(this, e);
        }
        void OnCanvasMouseEnter(Object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (CanvasMouseEnter != null)
                CanvasMouseEnter(this, e);
        }
        void OnCanvasMouseLeave(Object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (CanvasMouseLeave != null)
                CanvasMouseLeave(this, e);
        }
        void OnCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CanvasMouseDown != null)
                CanvasMouseDown(this, e);
        }
        void OnCanvasMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CanvasMouseUp != null)
                CanvasMouseUp(this, e);
        }
        void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CanvasMouseLeftButtonDown != null)
                CanvasMouseLeftButtonDown(this, e);
        }
        void OnCanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CanvasMouseLeftButtonUp != null)
                CanvasMouseLeftButtonUp(this, e);
            Selected = !Selected;
        }
        void OnCanvasMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CanvasMouseRightButtonDown != null)
                CanvasMouseRightButtonDown(this, e);
        }
        void OnCanvasMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CanvasMouseRightButtonUp != null)
                CanvasMouseRightButtonUp(this, e);
        }
        void OnCanvasMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (CanvasMouseWheel != null)
                CanvasMouseWheel(this, e);
        }
        public void OnCanvasKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (CanvasKeyDown != null)
                CanvasKeyDown(this, e);
        }
        void OnCanvasKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (CanvasKeyUp != null)
                CanvasKeyUp(this, e);
        }
        //Event
        [Browsable(true), Category("Canvas - Mouse")]
        public event System.Windows.Input.MouseEventHandler CanvasMouseMove;
        [Browsable(true), Category("Canvas - Mouse")]
        public event System.Windows.Input.MouseEventHandler CanvasMouseEnter;
        [Browsable(true), Category("Canvas - Mouse")]
        public event System.Windows.Input.MouseEventHandler CanvasMouseLeave;
        [Browsable(true), Category("Canvas - Mouse")]
        public event System.Windows.Input.MouseButtonEventHandler CanvasMouseDown;
        [Browsable(true), Category("Canvas - Mouse")]
        public event System.Windows.Input.MouseButtonEventHandler CanvasMouseUp;
        [Browsable(true), Category("Canvas - Mouse")]
        public event System.Windows.Input.MouseButtonEventHandler CanvasMouseLeftButtonDown;
        [Browsable(true), Category("Canvas - Mouse")]
        public event System.Windows.Input.MouseButtonEventHandler CanvasMouseLeftButtonUp;
        [Browsable(true), Category("Canvas - Mouse")]
        public event System.Windows.Input.MouseButtonEventHandler CanvasMouseRightButtonDown;
        [Browsable(true), Category("Canvas - Mouse")]
        public event System.Windows.Input.MouseButtonEventHandler CanvasMouseRightButtonUp;
        [Browsable(true), Category("Canvas - Mouse")]
        public event System.Windows.Input.MouseWheelEventHandler CanvasMouseWheel;
        [Browsable(true), Category("Canvas - Keboard")]
        public event System.Windows.Input.KeyEventHandler CanvasKeyDown;
        [Browsable(true), Category("Canvas - Keboard")]
        public event System.Windows.Input.KeyEventHandler CanvasKeyUp;

        private void mouseCapture_PositionChanged(object sender, Trade.Base.ScreenMousePositionMouseChangeArgs e)
        {
            ChangeXYPosition(e.Position);
        }
        [Browsable(false)]
        public bool EnableXYPosition
        {
            get { return mouseCapture.Enabled; }
            set { mouseCapture.Enabled = value; }
        }
        [Browsable(false)]
        public int XYPositionRefreshInterval
        {
            get { return mouseCapture.RefreshInterval; }
            set { mouseCapture.RefreshInterval = value; }
        }
        public void ChangeXYPosition(System.Drawing.Point screenPoint)
        {
            try
            {
                System.Drawing.Point p = this.PointToClient(screenPoint);
                PaintXY((double)p.X, (double)p.Y);
            }
            catch { }
        }
        public void ChangeXYPosition(System.Windows.Point screenPoint)
        {
            try
            {
                System.Drawing.Point p = this.PointToClient(new System.Drawing.Point((int)screenPoint.X,(int)screenPoint.Y));
                PaintXY((double)p.X, (double)p.Y);
            }
            catch { }
        }
        const int MINIMUM_WIDTH = 1;
        const double M1 = 4;
        const double M2 = 16;
        const double MTextWith = 20;
        const double YMargin = 5;
        const double XMargin = 70;
        bool isPainting;
        bool IsCreating
        {
            get { return C.ActualWidth == 0; }
        }
        List<ChartData> m_Data;
        public List<ChartData> Data
        {
            get { return m_Data; }
        }
        int m_ItemWidth, m_Current; //, m_ItemCount;
        [Browsable(true), Category("Chart")]
        public int ItemWidth
        {
            get { return m_ItemWidth; }
            set
            {
                int v = value < MINIMUM_WIDTH ? MINIMUM_WIDTH : value;
                if (m_ItemWidth != v)
                {
                    m_ItemWidth = v;
                    if (IsCreating) return;
                    //m_ItemCount = (int)System.Math.Floor(Area.Width / m_ItemWidth);
                    RepaintChart();
                }
            }
        }
        [Browsable(false)]
        public int ItemCount
        {
            get { return (int)System.Math.Floor(Area.Width / m_ItemWidth); }
            /*set
            {
                m_ItemCount = value;
                if (IsCreating) return;
                ItemWidth = (int)System.Math.Floor(Area.Width / m_ItemCount);
            }*/
        }
        [Browsable(false)]
        internal double MHeight;
        [Browsable(false)]
        internal object Tag1 = null;
        [Browsable(false)]
        internal object Tag2 = null;
        [Browsable(false)]
        public int Current
        {
            get { return m_Current; }
            set
            {
                if (m_Current != value)
                {
                    m_Current = value;
                    RepaintChart();
                }
            }
        }
        System.Windows.Rect m_Area;
        protected System.Windows.Rect Area
        {
            get
            {
                if (IsCreating)
                    return m_Area;
                m_Area.X = XMargin;
                m_Area.Y = YMargin;
                m_Area.Width = C.ActualWidth - XMargin * 2;
                m_Area.Height = C.ActualHeight - YMargin * 2;
                //m_ItemCount = (int)System.Math.Floor(m_Area.Width / m_ItemWidth);
                return m_Area;
            }
        }
        double leftMinY, leftMaxY;
        ZeroYLocation leftZeroLocation = ZeroYLocation.Bottom;
        double rightMinY, rightMaxY;
        ZeroYLocation rightZeroLocation = ZeroYLocation.Bottom;
        bool calculatingLeftY = false;
        bool calculatingRightY = false;
        void CalculateY(ChartData cd)
        {
            if (cd.Type == ChartType.VerticalLine)
                return;
            if (cd.Type == ChartType.HorizontalLine)
                return;
            if (cd.Type == ChartType.XAxis)
                return;
            int j = 0;
            foreach (var d in cd.Data)
            {
                for (int i = Current; i < Current + ItemCount; i++)
                {
                    if (i < d.Count)
                    {
                        if (d[i] is double)
                        {
                            if (i == Current && j == 0)
                            {
                                if (cd.YAxisLocation == YAxisLocation.Left && calculatingLeftY)
                                {
                                    leftZeroLocation = cd.ZeroLocation;
                                    leftMinY = (double)d[i];
                                    leftMaxY = (double)d[i];
                                    calculatingLeftY = false;
                                }
                                else if (cd.YAxisLocation == YAxisLocation.Right && calculatingRightY)
                                {
                                    rightZeroLocation = cd.ZeroLocation;
                                    rightMinY = (double)d[i];
                                    rightMaxY = (double)d[i];
                                    calculatingRightY = false;
                                }
                            }
                            if (cd.YAxisLocation == YAxisLocation.Left)
                            {
                                leftMinY = leftMinY < (double)d[i] ? leftMinY : (double)d[i];
                                leftMaxY = leftMaxY > (double)d[i] ? leftMaxY : (double)d[i];
                            }
                            else
                            {
                                rightMinY = rightMinY < (double)d[i] ? rightMinY : (double)d[i];
                                rightMaxY = rightMaxY > (double)d[i] ? rightMaxY : (double)d[i];
                            }
                        }
                    }
                    else
                        break;
                }
                j++;
            }
            if (!double.IsNaN(cd.MinY))
            {
                if (cd.YAxisLocation == YAxisLocation.Left)
                    leftMinY = cd.MinY;
                else
                    rightMinY = cd.MinY;
            }
            if (!double.IsNaN(cd.MaxY))
            {
                if (cd.YAxisLocation == YAxisLocation.Left)
                    leftMaxY = cd.MaxY;
                else
                    rightMaxY = cd.MaxY;
            }
        }
        double ConvertY(double y, ZeroYLocation loc, YAxisLocation locY)
        {
            double minY, maxY;
            if (locY == YAxisLocation.Left)
            {
                minY = leftMinY;
                maxY = leftMaxY;
            }
            else
            {
                minY = rightMinY;
                maxY = rightMaxY;
            }
            System.Windows.Rect a = Area;
            if (loc == ZeroYLocation.Bottom)
                return (a.Y + a.Height) - ((y - minY) * a.Height) / (maxY - minY);
            if (loc == ZeroYLocation.Top)
                return a.Y + ((y - minY) * a.Height) / (maxY - minY);
            double b = Math.Abs(minY) > Math.Abs(maxY) ? Math.Abs(minY) : Math.Abs(maxY);
            return (a.Y + a.Height) - ((y + b) * Area.Height) / (b * 2);
        }
        double ConvertY(double y, YAxisLocation locY)
        {
            return ConvertY(y, ZeroYLocation.Bottom, locY);
        }
        double ConvertX(double x)
        {
            System.Windows.Rect a = Area;
            return a.X + x;
        }
        System.Windows.Point Convert(double x, double y, ZeroYLocation loc, YAxisLocation locY)
        {
            return new System.Windows.Point(ConvertX(x), ConvertY(y, loc, locY));
        }
        System.Windows.Point Convert(double x, double y, YAxisLocation locY)
        {
            return new System.Windows.Point(ConvertX(x), ConvertY(y, locY));
        }
        void PaintLeftAxis()
        {
            if (leftMaxY - leftMinY == 0)
                return;
            double b1 = (C.ActualHeight - 2 * YMargin) / M2;
            double b2 = (leftMaxY - leftMinY) / b1;
            /*get the best interval in b*/
            double a = 1, b = 1;
            if (b > b2)
            {
                while (true)
                {
                    if (b / a > b2)
                    {
                        if (b / (a * 2) > b2)
                        {
                            if (b / (a * 5) > b2)
                            {
                                a = a * 10;
                            }
                            else
                            {
                                b = b / (a * 2);
                                break;
                            }
                        }
                        else
                        {
                            b = b / a;
                            break;
                        }
                    }
                    else
                    {
                        b = b / (a * 5 / 10);
                        break;
                    }

                }
            }
            else if (b < b2)
            {
                while (true)
                {
                    if (b * a < b2)
                    {
                        if (b * a * 2 < b2)
                        {
                            if (b * a * 5 < b2)
                            {
                                a = a * 10;
                            }
                            else
                            {
                                b = b * a * 5;
                                break;
                            }
                        }
                        else
                        {
                            b = b * a * 2;
                            break;
                        }
                    }
                    else
                    {
                        b = b * a;
                        break;
                    }
                }
            }
            b1 = (C.ActualHeight - 2 * YMargin) * b / (leftMaxY - leftMinY);
            if (b1 / 5 >= M1)
                b1 = b1 / 5;
            else if (b1 / 4 >= M1)
                b1 = b1 / 4;
            else if (b1 / 2 >= M1)
                b1 = b1 / 2;
            b1 = (leftMaxY - leftMinY) * b1 / (C.ActualHeight - 2 * YMargin);
            //GeometryGroup g = new GeometryGroup();
            GeometryGroup g = chartBuffer.GetGroup();
            for (double c = Math.Floor(leftMinY / b) * b; c <= Math.Ceiling(leftMaxY / b) * b; c = c + b)
            {
                //g.Children.Add(new LineGeometry(Convert(-15, c, leftZeroLocation, YAxisLocation.Left), Convert(-2, c, leftZeroLocation, YAxisLocation.Left)));
                g.Children.Add(chartBuffer.GetLine(Convert(-15, c, leftZeroLocation, YAxisLocation.Left), Convert(-2, c, leftZeroLocation, YAxisLocation.Left)));
                System.Windows.Controls.Label l = new System.Windows.Controls.Label();
                l.FontSize = 10;
                l.Content = c.ToString();
                l.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                l.VerticalAlignment = VerticalAlignment.Top;
                l.MaxWidth = XMargin - 15;
                l.Width = l.MaxWidth;
                l.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;
                l.VerticalContentAlignment = VerticalAlignment.Top;
                l.SetValue(System.Windows.Controls.Canvas.LeftProperty, 1.0);
                l.SetValue(System.Windows.Controls.Canvas.TopProperty, ConvertY(c, YAxisLocation.Left) - 10);
                C.Children.Add(l);
                b2 = c;
                while (true)
                {
                    b2 = b2 + b1;
                    if (b2 >= c + b)
                        break;
                    //g.Children.Add(new LineGeometry(Convert(-10, b2, leftZeroLocation, YAxisLocation.Left), Convert(-2, b2, leftZeroLocation, YAxisLocation.Left)));
                    g.Children.Add(chartBuffer.GetLine(Convert(-10, b2, leftZeroLocation, YAxisLocation.Left), Convert(-2, b2, leftZeroLocation, YAxisLocation.Left)));
                }
            }


            //System.Windows.Shapes.Path p = new System.Windows.Shapes.Path();
            System.Windows.Shapes.Path p = chartBuffer.GetPath();
            p.Stroke = System.Windows.Media.Brushes.Black;
            p.StrokeThickness = 1;
            p.SnapsToDevicePixels = true;
            p.Data = g;
            C.Children.Add(p);
        }
        void PaintRightAxis()
        {
            if (rightMaxY - rightMinY == 0)
                return;
            double b1 = (C.ActualHeight - 2 * YMargin) / M2;
            double b2 = (rightMaxY - rightMinY) / b1;
            /*get the best interval in b*/
            double a = 1, b = 1;
            if (b > b2)
            {
                while (true)
                {
                    if (b / a > b2)
                    {
                        if (b / (a * 2) > b2)
                        {
                            if (b / (a * 5) > b2)
                            {
                                a = a * 10;
                            }
                            else
                            {
                                b = b / (a * 2);
                                break;
                            }
                        }
                        else
                        {
                            b = b / a;
                            break;
                        }
                    }
                    else
                    {
                        b = b / (a * 5 / 10);
                        break;
                    }

                }
            }
            else if (b < b2)
            {
                while (true)
                {
                    if (b * a < b2)
                    {
                        if (b * a * 2 < b2)
                        {
                            if (b * a * 5 < b2)
                            {
                                a = a * 10;
                            }
                            else
                            {
                                b = b * a * 5;
                                break;
                            }
                        }
                        else
                        {
                            b = b * a * 2;
                            break;
                        }
                    }
                    else
                    {
                        b = b * a;
                        break;
                    }
                }
            }
            b1 = (C.ActualHeight - 2 * YMargin) * b / (rightMaxY - rightMinY);
            if (b1 / 5 >= M1)
                b1 = b1 / 5;
            else if (b1 / 4 >= M1)
                b1 = b1 / 4;
            else if (b1 / 2 >= M1)
                b1 = b1 / 2;
            b1 = (rightMaxY - rightMinY) * b1 / (C.ActualHeight - 2 * YMargin);
            //GeometryGroup g = new GeometryGroup();
            GeometryGroup g = chartBuffer.GetGroup();
            for (double c = Math.Floor(rightMinY / b) * b; c <= Math.Ceiling(rightMaxY / b) * b; c = c + b)
            {
                //g.Children.Add(new LineGeometry(Convert(C.ActualWidth - XMargin * 2 + 2, c, rightZeroLocation, YAxisLocation.Right), Convert(C.ActualWidth - XMargin * 2 + 15, c, rightZeroLocation, YAxisLocation.Right)));
                g.Children.Add(chartBuffer.GetLine(Convert(C.ActualWidth - XMargin * 2 + 2, c, rightZeroLocation, YAxisLocation.Right), Convert(C.ActualWidth - XMargin * 2 + 15, c, rightZeroLocation, YAxisLocation.Right)));
                System.Windows.Controls.Label l = new System.Windows.Controls.Label();
                l.FontSize = 10;
                l.Content = c.ToString();
                l.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                l.VerticalAlignment = VerticalAlignment.Top;
                l.MaxWidth = XMargin - 15;
                l.Width = l.MaxWidth;
                l.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                l.VerticalContentAlignment = VerticalAlignment.Top;
                l.SetValue(System.Windows.Controls.Canvas.RightProperty, 1.0);
                l.SetValue(System.Windows.Controls.Canvas.TopProperty, ConvertY(c, YAxisLocation.Right) - 10);
                C.Children.Add(l);
                b2 = c;
                while (true)
                {
                    b2 = b2 + b1;
                    if (b2 >= c + b)
                        break;
                    //g.Children.Add(new LineGeometry(Convert(C.ActualWidth - XMargin * 2 + 2, b2, rightZeroLocation, YAxisLocation.Right), Convert(C.ActualWidth - XMargin * 2 + 10, b2, rightZeroLocation, YAxisLocation.Right)));
                    g.Children.Add(chartBuffer.GetLine(Convert(C.ActualWidth - XMargin * 2 + 2, b2, rightZeroLocation, YAxisLocation.Right), Convert(C.ActualWidth - XMargin * 2 + 10, b2, rightZeroLocation, YAxisLocation.Right)));
                }
            }


            //System.Windows.Shapes.Path p = new System.Windows.Shapes.Path();
            System.Windows.Shapes.Path p = chartBuffer.GetPath();
            p.Stroke = System.Windows.Media.Brushes.Black;
            p.StrokeThickness = 1;
            p.SnapsToDevicePixels = true;
            p.Data = g;
            C.Children.Add(p);
        }
        public void RepaintChart()
        {
            /*LastRepaintRequestTime = DateTime.Now;
            NeedRepaint = true;*/
            timer1.Enabled = false;
            timer1.Interval = 300;
            timer1.Enabled = true;
            
        }
        public void RepaintChart1()
        {
            if (!DrawChart)
                return;
            if (isPainting)
                return;
            if (IsCreating)
                return;
            try
            {
                isPainting = true;
                Clear();
                if (Data.Count == 0)
                    return;
                if (Current >= Data[0].Length - 5)
                    Current = Data[0].Length - 5;
                if (Current <= 0)
                    Current = 0;
                calculatingLeftY = true;
                calculatingRightY = true;
                leftMinY = 0; leftMaxY = 0;
                rightMinY = 0; rightMaxY = 0;
                foreach (ChartData cd in Data)
                    CalculateY(cd);
                PaintLeftAxis();
                PaintRightAxis();
                foreach (ChartData cd in Data)
                {
                    if (cd.Type == ChartType.CandleStick)
                        PaintCandleStick(cd);
                    else if (cd.Type == ChartType.Bar)
                        PaintBar(cd);
                    else if (cd.Type == ChartType.HorizontalLine)
                        PaintHorizontalLine(cd);
                    else if (cd.Type == ChartType.Line)
                        PaintLine(cd);
                    else if (cd.Type == ChartType.Stripe)
                        PaintStripe(cd);
                    else if (cd.Type == ChartType.VerticalLine)
                        PaintVerticalLine(cd);
                    else if (cd.Type == ChartType.XAxis)
                        PaintXAxis(cd);
                }
            }
            finally
            {
                isPainting = false;
            }
            return;
        }
        void PaintCandleStick(ChartData cd)
        {
            if (cd.YAxisLocation == YAxisLocation.Left)
            {
                if (leftMaxY - leftMinY == 0)
                    return;
            }
            else
                if (rightMaxY - rightMinY == 0)
                    return;

            double x = 0;
            double[] v = new double[4];
            bool fill = false;
            System.Windows.Media.Brush b;
            for (int i = Current; i < Current + ItemCount - 1; i++)
            {
                if (i >= cd.Data[0].Count)
                    break;
                x = i - Current;
                if (i < cd.Data[0].Count)
                {
                    fill = false;
                    if (i == 0)
                    {
                        if ((double)cd.Data[2][i] >= (double)cd.Data[1][i])
                            b = new System.Windows.Media.SolidColorBrush(cd.Color);
                        else
                            b = new System.Windows.Media.SolidColorBrush(cd.DownColor);
                    }
                    else
                    {
                        if ((double)cd.Data[4][i] >= (double)cd.Data[4][i - 1])
                            b = new System.Windows.Media.SolidColorBrush(cd.Color);
                        else
                            b = new System.Windows.Media.SolidColorBrush(cd.DownColor);
                    }
                    v[0] = (double)cd.Data[0][i];
                    if ((double)cd.Data[1][i] >= (double)cd.Data[2][i])
                    {
                        fill = true;
                        v[1] = (double)cd.Data[1][i];
                        v[2] = (double)cd.Data[2][i];
                    }
                    else
                    {
                        fill = false;
                        v[1] = (double)cd.Data[2][i];
                        v[2] = (double)cd.Data[1][i];
                    }
                    v[3] = (double)cd.Data[3][i];

                    //System.Windows.Shapes.Path p = new System.Windows.Shapes.Path();
                    System.Windows.Shapes.Path p = chartBuffer.GetPath();
                    p.Stroke = b;
                    p.StrokeThickness = cd.StrokeThickness;
                    p.Opacity = cd.Opacity;
                    p.SnapsToDevicePixels = true;
                    if (fill)
                        p.Fill = b;
                    double interval = 1;
                    if (ItemWidth == 1)
                        interval = 0;
                    else if (ItemWidth > 5)
                        interval = 2;
                    else if (ItemWidth > 10)
                        interval = 3;
                    interval = ItemWidth * 0.3;
                    //LineGeometry l1 = new LineGeometry(Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, v[0], cd.ZeroLocation, cd.YAxisLocation), Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, v[1], cd.ZeroLocation, cd.YAxisLocation));
                    LineGeometry l1 = chartBuffer.GetLine(Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, v[0], cd.ZeroLocation, cd.YAxisLocation), Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, v[1], cd.ZeroLocation, cd.YAxisLocation));
                    //LineGeometry l2 = new LineGeometry(Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, v[2], cd.ZeroLocation, cd.YAxisLocation), Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, v[3], cd.ZeroLocation, cd.YAxisLocation));
                    LineGeometry l2 = chartBuffer.GetLine(Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, v[2], cd.ZeroLocation, cd.YAxisLocation), Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, v[3], cd.ZeroLocation, cd.YAxisLocation));
                    RectangleGeometry r;
                    if (l2.StartPoint.Y >= l1.EndPoint.Y)
                        //r = new RectangleGeometry(new Rect(ConvertX(x * this.ItemWidth), l1.EndPoint.Y, (double)this.ItemWidth - interval, System.Math.Abs(l2.StartPoint.Y - l1.EndPoint.Y)));
                        r = chartBuffer.GetRectangle(new Rect(ConvertX(x * this.ItemWidth), l1.EndPoint.Y, (double)this.ItemWidth - interval, System.Math.Abs(l2.StartPoint.Y - l1.EndPoint.Y)));
                    else
                        //r = new RectangleGeometry(new Rect(ConvertX(x * this.ItemWidth), l2.StartPoint.Y, (double)this.ItemWidth - interval, System.Math.Abs(l2.StartPoint.Y - l1.EndPoint.Y)));
                        r = chartBuffer.GetRectangle(new Rect(ConvertX(x * this.ItemWidth), l2.StartPoint.Y, (double)this.ItemWidth - interval, System.Math.Abs(l2.StartPoint.Y - l1.EndPoint.Y)));
                    //GeometryGroup g = new GeometryGroup();
                    GeometryGroup g = chartBuffer.GetGroup();
                    g.Children.Add(l1);
                    g.Children.Add(l2);
                    g.Children.Add(r);
                    p.Data = g;
                    C.Children.Add(p);
                    if (l1.StartPoint.Y < 0)
                        return;
                }
                else
                    break;
            }
        }
        void PaintBar(ChartData cd)
        {
            if (cd.YAxisLocation == YAxisLocation.Left)
            {
                if (leftMaxY - leftMinY == 0)
                    return;
            }
            else
                if (rightMaxY - rightMinY == 0)
                    return;
            double x = 0;
            System.Windows.Media.Brush b = new System.Windows.Media.SolidColorBrush(cd.Color);
            for (int i = Current; i < Current + ItemCount - 1; i++)
            {
                x = i - Current;
                if (i >= cd.Data[0].Count)
                    break;
                //System.Windows.Shapes.Path p = new System.Windows.Shapes.Path();
                System.Windows.Shapes.Path p = chartBuffer.GetPath();
                p.Stroke = b;
                p.StrokeThickness = cd.StrokeThickness;
                p.Opacity = cd.Opacity;
                p.SnapsToDevicePixels = true;
                p.Fill = b;
                double interval = 1;
                if (ItemWidth == 1)
                    interval = 0;
                else if (ItemWidth > 5)
                    interval = 2;
                else if (ItemWidth > 10)
                    interval = 3;
                interval = ItemWidth * 0.3;
                RectangleGeometry r;
                //r = new RectangleGeometry(new Rect(ConvertX(x * this.ItemWidth), ConvertY((double)cd.Data[0][i], cd.YAxisLocation), (double)this.ItemWidth - interval, System.Math.Abs(ConvertY(0, cd.YAxisLocation) - ConvertY((double)cd.Data[0][i], cd.YAxisLocation))));
                r = chartBuffer.GetRectangle(new Rect(ConvertX(x * this.ItemWidth), ConvertY((double)cd.Data[0][i], cd.YAxisLocation), (double)this.ItemWidth - interval, System.Math.Abs(ConvertY(0, cd.YAxisLocation) - ConvertY((double)cd.Data[0][i], cd.YAxisLocation))));
                p.Data = r;
                C.Children.Add(p);
            }
        }
        void PaintHorizontalLine(ChartData cd)
        {
            if (cd.YAxisLocation == YAxisLocation.Left)
            {
                if (leftMaxY - leftMinY == 0)
                    return;
            }
            else
                if (rightMaxY - rightMinY == 0)
                    return;
            double x = 0;
            System.Windows.Media.Brush b = new System.Windows.Media.SolidColorBrush(cd.Color);
            for (int i = 0; i < cd.Data[0].Count; i++)
            {
                x = i - Current;
                if (i >= cd.Data[0].Count)
                    break;
                //System.Windows.Shapes.Path p = new System.Windows.Shapes.Path();
                System.Windows.Shapes.Path p = chartBuffer.GetPath();
                p.Stroke = b;
                p.StrokeThickness = cd.StrokeThickness;
                p.Opacity = cd.Opacity;
                p.SnapsToDevicePixels = true;
                p.Fill = b;
                //LineGeometry l = new LineGeometry(Convert(0, (double)cd.Data[0][i], cd.YAxisLocation), Convert(C.ActualWidth - 2 * XMargin, (double)cd.Data[0][i], cd.YAxisLocation));
                LineGeometry l = chartBuffer.GetLine(Convert(0, (double)cd.Data[0][i], cd.YAxisLocation), Convert(C.ActualWidth - 2 * XMargin, (double)cd.Data[0][i], cd.YAxisLocation));
                p.Data = l;
                C.Children.Add(p);
            }
        }
        void PaintLine(ChartData cd)
        {
            if (cd.YAxisLocation == YAxisLocation.Left)
            {
                if (leftMaxY - leftMinY == 0)
                    return;
            }
            else
                if (rightMaxY - rightMinY == 0)
                    return;
            double x = 0;
            System.Windows.Media.Brush b = new System.Windows.Media.SolidColorBrush(cd.Color);
            for (int i = Current + 1; i < Current + ItemCount - 1; i++)
            {
                x = i - Current;
                if (i >= cd.Data[0].Count)
                    break;
                //System.Windows.Shapes.Path p = new System.Windows.Shapes.Path();
                System.Windows.Shapes.Path p = chartBuffer.GetPath();
                p.Stroke = b;
                p.StrokeThickness = cd.StrokeThickness;
                p.Opacity = cd.Opacity;
                p.SnapsToDevicePixels = true;
                p.Fill = b;
                double interval = 1;
                if (ItemWidth == 1)
                    interval = 0;
                else if (ItemWidth > 5)
                    interval = 2;
                else if (ItemWidth > 10)
                    interval = 3;
                interval = ItemWidth * 0.3;
                //LineGeometry l = new LineGeometry(Convert(((double)this.ItemWidth - interval) / 2 + (x - 1) * this.ItemWidth, (double)cd.Data[0][i - 1], cd.ZeroLocation, cd.YAxisLocation), Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, (double)cd.Data[0][i], cd.ZeroLocation, cd.YAxisLocation));
                LineGeometry l = chartBuffer.GetLine(Convert(((double)this.ItemWidth - interval) / 2 + (x - 1) * this.ItemWidth, (double)cd.Data[0][i - 1], cd.ZeroLocation, cd.YAxisLocation), Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, (double)cd.Data[0][i], cd.ZeroLocation, cd.YAxisLocation));
                p.Data = l;
                C.Children.Add(p);
            }
        }
        void PaintVerticalLine(ChartData cd)
        {
            if (cd.Type != ChartType.VerticalLine)
                return;
            if (cd.YAxisLocation == YAxisLocation.Left)
            {
                if (leftMaxY - leftMinY == 0)
                    return;
            }
            else
                if (rightMaxY - rightMinY == 0)
                    return;
            double x = 0;
            System.Windows.Media.Brush b = new System.Windows.Media.SolidColorBrush(cd.Color);
            for (int i = Current + 1; i < Current + ItemCount - 1; i++)
            {
                x = i - Current;
                if (i >= cd.Data[0].Count)
                    break;
                if (!((bool)cd.Data[0][i]))
                    continue;
                //System.Windows.Shapes.Path p = new System.Windows.Shapes.Path();
                System.Windows.Shapes.Path p = chartBuffer.GetPath();
                p.Stroke = b;
                p.StrokeThickness = cd.StrokeThickness;
                p.Opacity = cd.Opacity;
                p.SnapsToDevicePixels = true;
                p.Fill = b;
                double interval = 1;
                if (ItemWidth == 1)
                    interval = 0;
                else if (ItemWidth > 5)
                    interval = 2;
                else if (ItemWidth > 10)
                    interval = 3;
                interval = ItemWidth * 0.3;
                LineGeometry l;
                if (cd.YAxisLocation == YAxisLocation.Left)
                    //l = new LineGeometry(Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, leftMaxY, cd.ZeroLocation, cd.YAxisLocation), Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, leftMinY, cd.ZeroLocation, cd.YAxisLocation));
                    l = chartBuffer.GetLine(Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, leftMaxY, cd.ZeroLocation, cd.YAxisLocation), Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, leftMinY, cd.ZeroLocation, cd.YAxisLocation));
                else
                    //l = new LineGeometry(Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, rightMaxY, cd.ZeroLocation, cd.YAxisLocation), Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, rightMinY, cd.ZeroLocation, cd.YAxisLocation));
                    l = chartBuffer.GetLine(Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, rightMaxY, cd.ZeroLocation, cd.YAxisLocation), Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, rightMinY, cd.ZeroLocation, cd.YAxisLocation));
                p.Data = l;
                C.Children.Add(p);
            }
        }
        void PaintStripe(ChartData cd)
        {
            if (cd.YAxisLocation == YAxisLocation.Left)
            {
                if (leftMaxY - leftMinY == 0)
                    return;
            }
            else
                if (rightMaxY - rightMinY == 0)
                    return;
            double x = 0;
            System.Windows.Media.Brush b = new System.Windows.Media.SolidColorBrush(cd.Color);
            int totalCount = Math.Min(cd.Data[0].Count, Current + ItemCount - 1);
            System.Windows.Shapes.Polyline p = new System.Windows.Shapes.Polyline();
            p.Stroke = new System.Windows.Media.SolidColorBrush(cd.Color);
            p.StrokeThickness = cd.StrokeThickness;
            p.Opacity = cd.Opacity;
            p.SnapsToDevicePixels = true;
            p.Fill = p.Stroke;
            PointCollection pp = new PointCollection();
            for (int i = Current; i < totalCount; i++)
            {
                x = i - Current;
                double interval = 1;
                if (ItemWidth == 1)
                    interval = 0;
                else if (ItemWidth > 5)
                    interval = 2;
                else if (ItemWidth > 10)
                    interval = 3;
                interval = ItemWidth * 0.3;
                pp.Add(Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, (double)cd.Data[0][i], cd.YAxisLocation));
            }
            for (int i = totalCount - 1; i >= Current; i--)
            {
                x = i - Current;
                double interval = 1;
                if (ItemWidth == 1)
                    interval = 0;
                else if (ItemWidth > 5)
                    interval = 2;
                else if (ItemWidth > 10)
                    interval = 3;
                interval = ItemWidth * 0.3;
                pp.Add(Convert(((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth, (double)cd.Data[1][i], cd.YAxisLocation));
            }
            p.Points = pp;
            C.Children.Add(p);
        }
        void PaintXAxis(ChartData cd)
        {
            if (cd.Type != ChartType.XAxis)
                return;
            int totalCount = Math.Min(cd.Data[0].Count, Current + ItemCount - 1);
            double x = 0;
            double interval = 1;
            interval = 1;
            if (ItemWidth == 1)
                interval = 0;
            else if (ItemWidth > 5)
                interval = 2;
            else if (ItemWidth > 10)
                interval = 3;
            interval = ItemWidth * 0.3;
            DateTime? d0;
            DateTime? d1;
            System.Windows.Media.SolidColorBrush b = new System.Windows.Media.SolidColorBrush(cd.Color);
            double h = 0;
            double x0 = -100;
            double x00 = -100;
            System.Windows.Controls.Label /*Y = null,*/ M = null, D = null;
            for (int i = Current; i < totalCount; i++)
            {
                /*Y = null; */ M = null; D = null;
                x = i - Current;
                x = ((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth;
                if (x - x0 < M1)
                    continue;
                d0 = null;
                if (i > 0)
                    d0 = (DateTime)cd.Data[0][i - 1];
                d1 = (DateTime)cd.Data[0][i];
                h = 5;
                if (x - x00 > MTextWith)
                {
                    D = new System.Windows.Controls.Label();
                    D.Content = d1.Value.Day.ToString().PadLeft(2, '0');
                    x00 = x;
                }
                if (D != null)
                {
                    D.FontSize = 10;
                    D.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    D.VerticalAlignment = VerticalAlignment.Top;
                    D.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                    D.VerticalContentAlignment = VerticalAlignment.Top;
                    D.Foreground = b;
                    D.Width = MTextWith;
                    D.Height = 12;
                    D.SetValue(System.Windows.Controls.Canvas.LeftProperty, ConvertX(x));
                    D.SetValue(System.Windows.Controls.Canvas.TopProperty, 5d);
                    D.Padding = new Thickness(1, 1, 0, 0);
                    C.Children.Add(D);
                    h = 17;
                }
                System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                l.Stroke = b;
                l.StrokeThickness = cd.StrokeThickness;
                l.Opacity = cd.Opacity;
                l.X1 = ConvertX(x);
                l.Y1 = 1;
                l.X2 = l.X1;
                l.Y2 = h;
                C.Children.Add(l);
                x0 = x;
            }
            D = null; x00 = -100;
            for (int i = Current; i < totalCount; i++)
            {
                /*Y = null; */M = null;
                x = i - Current;
                x = ((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth;
                d0 = null;
                if (i > 0)
                    d0 = (DateTime)cd.Data[0][i - 1];
                d1 = (DateTime)cd.Data[0][i];
                if ((d0 == null) || (d0.Value.Month != d1.Value.Month))
                {
                    if ((x - x00 < MTextWith - 5) && (D != null))
                        C.Children.Remove(D);
                    M = new System.Windows.Controls.Label();
                    //M = chartBuffer.GetLabel();
                    if (String.Format("{0:MMM}", d1) == "Jan")
                        M.Content = d1.Value.Year.ToString().Substring(2,2);
                    else
                        M.Content = String.Format("{0:MMM}", d1);//(//.Value.Month.ToString().PadLeft(2, '0');
                    M.FontSize = 10;
                    M.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    M.VerticalAlignment = VerticalAlignment.Top;
                    M.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                    M.VerticalContentAlignment = VerticalAlignment.Top;
                    M.Foreground = b;
                    M.Width = MTextWith;
                    M.Height = 12;
                    M.SetValue(System.Windows.Controls.Canvas.LeftProperty, ConvertX(x));
                    M.SetValue(System.Windows.Controls.Canvas.TopProperty, 17d);
                    M.Padding = new Thickness(1, 1, 0, 0);
                    C.Children.Add(M);
                    System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                    l.Stroke = b;
                    l.StrokeThickness = cd.StrokeThickness;
                    l.Opacity = cd.Opacity;
                    l.X1 = ConvertX(x);
                    l.Y1 = 17;
                    l.X2 = l.X1;
                    l.Y2 = 29;
                    C.Children.Add(l);
                    x00 = x;
                    D = M;
                }
            }
            /*D = null; x00 = -100;
            for (int i = Current; i < totalCount; i++)
            {
                Y = null; M = null;
                x = i - Current;
                x = ((double)this.ItemWidth - interval) / 2 + x * this.ItemWidth;
                d0 = null;
                if (i > 0)
                    d0 = (DateTime)cd.Data[0][i - 1];
                d1 = (DateTime)cd.Data[0][i];
                if ((d0 == null) || (d0.Value.Year != d1.Value.Year))
                {
                    if ((x - x00 < MTextWith) && (D != null))
                        C.Children.Remove(D);
                    Y = new System.Windows.Controls.Label();
                    Y.Content = d1.Value.Year.ToString();
                    Y.FontSize = 10;
                    Y.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    Y.VerticalAlignment = VerticalAlignment.Top;
                    Y.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                    Y.VerticalContentAlignment = VerticalAlignment.Top;
                    Y.Foreground = b;
                    Y.Width = MTextWith + 10;
                    Y.Height = 12;
                    Y.SetValue(System.Windows.Controls.Canvas.LeftProperty, ConvertX(x));
                    Y.SetValue(System.Windows.Controls.Canvas.TopProperty, 29d);
                    Y.Padding = new Thickness(1, 1, 0, 0);
                    C.Children.Add(Y);
                    System.Windows.Shapes.Line l = new System.Windows.Shapes.Line();
                    l.Stroke = b;
                    l.StrokeThickness = cd.StrokeThickness;
                    l.Opacity = cd.Opacity;
                    l.X1 = ConvertX(x);
                    l.Y1 = 29;
                    l.X2 = l.X1;
                    l.Y2 = 42;
                    C.Children.Add(l);
                    x00 = x;
                    D = Y;
                }
            }*/
        }
        public void Clear()
        {
            var cc = C.Children.Cast<System.Windows.FrameworkElement>().Where(x => { return x.Tag == null; }).ToList();
            foreach (var c in cc)
            {
                C.Children.Remove(c);
                if (c.GetType().GetMethods(System.Reflection.BindingFlags.Public).FirstOrDefault(x => x.Name=="Dispose")!=null)
                {
                    c.GetType().InvokeMember("Dispose", System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public, null, c, null);
                }
            }
            leftMinY = 0;
            leftMaxY = 0;
            rightMinY = 0;
            rightMaxY = 0;
            chartBuffer.Reset();
            return;
        }
        private void Canvas_Resize(object sender, EventArgs e)
        {
            rSelected.Width = C.ActualWidth;
            rSelected.Height = C.ActualHeight;
            RepaintChart();
        }
        [Browsable(false)]
        public bool Selected
        {
            get { return rSelected.Visibility == Visibility.Visible; }
            set {
                    if (value)
                    {
                        rSelected.Visibility = Visibility.Visible;
                        if (ChartSelected != null)
                            ChartSelected(this, new EventArgs());
                    }
                    else
                    {
                        rSelected.Visibility = Visibility.Hidden;
                        if (ChartDeselected != null)
                            ChartDeselected(this, new EventArgs());
                    }
                }
        }
        [Browsable(true), Category("Chart")]
        public event EventHandler ChartSelected;
        [Browsable(true), Category("Chart")]
        public event EventHandler ChartDeselected;
        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseWheel(e);
        }
        public bool DrawChart = true;
        /*private DateTime LastRepaintRequestTime;
        private bool NeedRepaint = false;*/
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            RepaintChart1();
            /*if (NeedRepaint)
            {
                if (DateTime.Now > LastRepaintRequestTime)
                {
                    NeedRepaint = false;
                    RepaintChart1();
                }
            }*/
        }
    }
    public delegate void ChartXY(object sender, EventChartXYArgs e);
    public class EventChartXYArgs : EventArgs
    {
        private double x, y;
        private string mLabel;
        private Trade.Base.Canvas canvas;
        public double X { get { return x; } }
        public double Y { get { return y; } }
        public Trade.Base.Canvas Canvas
        {
            get { return canvas; }
        }
        public string Label
        {
            get { return mLabel; }
            set { mLabel = value; }
        }
        public EventChartXYArgs(double xx, double yy, string label, Trade.Base.Canvas c)
        {
            x = xx;
            y = yy;
            mLabel = label;
            canvas = c;
        }
    }
    public enum ChartType { CandleStick, Bar, Line, Stripe, VerticalLine, HorizontalLine, XAxis, Pie }
    public enum ArrowDirection { Up, Down, Left, Right }
    public enum ZeroYLocation { Bottom, Center, Top }
    public enum YAxisLocation { Left, Right }
    public class ChartData
    {
        string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        /*
         * CandleStick: double High[], double Open[], double Close[], double Low[], doulbe Median[]
         * Bar, Line: double value[], 
         * Stripe: value1[] , value2[], 
         * VerticalLine: bool[]
         * HorizontalLine: double[]
         * XAxis : DateTime[]
         */
        List<List<object>> m_Data;
        public List<List<object>> Data
        {
            get { return m_Data; }
            //set { m_Data = value; }
        }
        ChartType m_Type;
        public ChartType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        System.Windows.Media.Color m_Color;
        public System.Windows.Media.Color Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }
        System.Windows.Media.Color m_DownColor;
        public System.Windows.Media.Color DownColor
        {
            get { return m_DownColor; }
            set { m_DownColor = value; }
        }
        ZeroYLocation m_ZeroLocation;
        public ZeroYLocation ZeroLocation
        {
            get { return m_ZeroLocation; }
            set { m_ZeroLocation = value; }
        }
        public ChartData()
        {
            m_Data = new List<List<object>>();
            MinY = double.NaN;
            MaxY = double.NaN;
            m_YAxisLocation = YAxisLocation.Left;
            m_Color = System.Windows.Media.Colors.Black;
            m_DownColor = System.Windows.Media.Colors.Black;
            mOpacity = 0.9;
            StrokeThickness = 1;
        }
        double m_MinY, m_MaxY;
        public double MinY
        {
            get { return m_MinY; }
            set { m_MinY = value; }
        }
        public double MaxY
        {
            get { return m_MaxY; }
            set { m_MaxY = value; }
        }
        public int Length
        {
            get { return (Data[0]).Count; }
        }
        double mOpacity;
        public double Opacity
        {
            get { return mOpacity; }
            set { mOpacity = value; }
        }
        double mStrokeThickness;
        public double StrokeThickness
        {
            get { return mStrokeThickness; }
            set { mStrokeThickness = value; }
        }
        public YAxisLocation m_YAxisLocation;
        public YAxisLocation YAxisLocation
        {
            get { return m_YAxisLocation; }
            set { m_YAxisLocation = value; }
        }
    }

    public class ChartBuffer
    {
        ChartBufferItem<LineGeometry> lines;
        ChartBufferItem<RectangleGeometry> rectangles;
        ChartBufferItem<GeometryGroup> groups;
        ChartBufferItem<Path> paths;
        //ChartBufferItem<System.Windows.Controls.Label> labels;
        public ChartBuffer()
        {
            lines = new ChartBufferItem<LineGeometry>(100);
            rectangles = new ChartBufferItem<RectangleGeometry>(100);
            groups = new ChartBufferItem<GeometryGroup>(100);
            paths = new ChartBufferItem<Path>(100);
            //labels = new ChartBufferItem<System.Windows.Controls.Label>();
        }
        public void Reset()
        {
            lines.Reset();
            rectangles.Reset();
            groups.Reset();
            paths.Reset();
            //labels.Reset();
            foreach (Path p in paths.Buffer)
            {
                p.Data = null;
                p.Stroke = System.Windows.Media.Brushes.Black;
                p.Opacity = 0;
                p.SnapsToDevicePixels = true;
                p.Fill = System.Windows.Media.Brushes.Black;
            }
            foreach (GeometryGroup g in groups.Buffer)
            {
                g.Children.Clear();
            }
            paths.Clear();
            groups.Clear();
            rectangles.Clear();
            lines.Clear();
            /*foreach (System.Windows.Controls.Label l in labels.Buffer)
                ((System.Windows.Controls.Canvas)l.Parent).Children.Remove(l);
            */
        }
        public void Clear()
        {
            Reset();
            paths.Clear();
            groups.Clear();
            rectangles.Clear();
            lines.Clear();
            //labels.Clear();
        }
        /*public System.Windows.Controls.Label GetLabel()
        {
            return labels.Get();
        }*/
        public LineGeometry GetLine()
        {
            return lines.Get();
        }
        public LineGeometry GetLine(System.Windows.Point startPoint, System.Windows.Point endPoint)
        {
            LineGeometry r = GetLine();
            r.StartPoint = startPoint;
            r.EndPoint = endPoint;
            return r;
        }
        public RectangleGeometry GetRectangle()
        {
            return rectangles.Get();
        }
        public RectangleGeometry GetRectangle(Rect rect)
        {
            RectangleGeometry r = rectangles.Get();
            r.Rect = rect;
            return r;
        }
        public GeometryGroup GetGroup()
        {
            return groups.Get();
        }
        public Path GetPath()
        {
            return paths.Get();
        }

    }

    public class ChartBufferItem<T>
    {
        public List<T> Buffer; 
        int Current;
        public ChartBufferItem()
        {
            this.Buffer = new List<T>();
            Current = 0;
        }
        public ChartBufferItem(int capacity)
        {
            this.Buffer = new List<T>();
            Current = 0;
            for (int i = 0; i < capacity; i++)
                this.Buffer.Add(CreateItem());
        }
        public void Reset()
        {
            Current = 0;
        }
        public void Clear()
        {
            this.Buffer.Clear();
            Current = 0;
        }
        public T Get()
        {
            if(Current >= Buffer.Count - 1)
                Buffer.Add(CreateItem());
            Current++;
            return Buffer[Current - 1];            
        }
        protected T CreateItem()
        {
            return (T) System.Activator.CreateInstance(typeof(T));
        }
    }
    
}
