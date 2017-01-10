using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trade.Base
{
    public partial class ScreenMousePosition : Component
    {
        Point p;
        public ScreenMousePosition()
        {
            InitializeComponent();
            p = new Point(0, 0);
            internalTimer.Enabled = !this.DesignMode;
        }
        private void internalTimer_Tick(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;
            if ((Position.X != p.X) || (Position.Y != p.Y))
            {
                var p1 = new Point(p.X, p.Y);
                p.X = Position.X;
                p.Y = Position.Y;
                if (PositionChanged != null)
                    PositionChanged(this, new ScreenMousePositionMouseChangeArgs(p1, p));
            }
        }
        public Point Position
        {
            get { return System.Windows.Forms.Cursor.Position; }
        }
        public event ScreenMousePositionMouseChangeEventHandler PositionChanged;
        public bool Enabled
        {
            get { return internalTimer.Enabled; }
            set{ internalTimer.Enabled = value; }
        }
        public int RefreshInterval
        {
            get { return internalTimer.Interval; }
            set { internalTimer.Interval = value; }
        }
    }
    public class ScreenMousePositionMouseChangeArgs : EventArgs
    {
        Point m_Position, m_PreviousPosition;
        public Point Position
        {
            get { return m_Position; }
        }
        public Point PreviousPosition
        {
            get { return m_PreviousPosition; }
        }
        public ScreenMousePositionMouseChangeArgs(Point PreviousPoint, Point currentPoint)
            : base()
        {
            m_Position = currentPoint;
            m_PreviousPosition = PreviousPoint;
        }
    }
    public delegate void ScreenMousePositionMouseChangeEventHandler(Object sender, ScreenMousePositionMouseChangeArgs e);
}
