using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace TestWinForm
{
    public partial class Form1 : Form
    {
        public List<Quote> Q = DataAccess.GetData();
        public Form1()
        {
            InitializeComponent();
            button1_Click(null,null);
            //dataGridView1.DataMember = Q;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //List<Quote> data = DataAccess.GetData();
            quoteBindingSource.DataSource = Q;
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            ChartArea c1 = chart1.ChartAreas.Add("c1");
            c1.AxisX.MajorGrid.LineWidth = 0;
            c1.AxisY.MajorGrid.LineWidth = 0;
            ChartArea c3 = chart1.ChartAreas.Add("c3");
            ChartArea c2 = chart1.ChartAreas.Add("c2");
            Series s1 = new Series("s1") { ChartArea = "c1"};
            Series s2 = new Series("s2") { ChartArea = "c2" };
            s1.Points.AddXY(1, 1);
            s1.Points.AddXY(2, 2);
            s1.Points.AddXY(3, 3);
            s1.Points.AddXY(4, 4);
            s1.Points.AddXY(5, 5);
            s1.ChartType = SeriesChartType.Column;
            
            
            s2.Points.AddXY(1, 5);
            s2.Points.AddXY(2, 4);
            s2.Points.AddXY(3, 3);
            s2.Points.AddXY(4, 4);
            s2.Points.AddXY(5, 1);
            s2.ChartType = SeriesChartType.Line;
            chart1.Series.Add(s1);
            chart1.Series.Add(s2);
            chart1_SizeChanged(null, null);
            c1.Position.Height = 50;
            c1.Position.Width = 100;
            c1.Position.X = 0;
            c1.Position.Y = 0;
            
            c2.Position.Height = 49;
            c2.Position.Width = 100;
            c2.Position.X = 0;
            c2.Position.Y = 51;

            c3.Position.Height = 1;
            c3.BackColor = Color.Black;
            c3.Position.Width = 100;
            c3.Position.X = 0;
            c3.Position.Y = 50;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChartArea c1 = chart1.ChartAreas.Add("c3");
            Series s1 = new Series("s3") { ChartArea = "c3" };
            s1.Points.AddXY(1, 1);
            s1.Points.AddXY(2, 2);
            s1.Points.AddXY(3, 3);
            s1.Points.AddXY(4, 4);
            s1.Points.AddXY(5, 5);
            s1.ChartType = SeriesChartType.Doughnut;
            
            chart1.Series.Add(s1);
        }
        Point previousMousePointer = new Point(-1, -1);
        ChartArea currentChartArea = null;
    
        DateTime lastEvent = DateTime.Now;
        bool mouseDown = false;
        int x, y; 
        Single c3y = 0;
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            DateTime now = DateTime.Now;
            if ((now - lastEvent).TotalMilliseconds < 100)
            {
                return;
            }
            lastEvent = now;
            
            
            foreach(var s in chart1.Series)
            {
                s.MarkerStyle = MarkerStyle.None;
            }
            var ret = chart1.HitTest(e.X, e.Y);
            //propertyGrid1.SelectedObject = ret.Series;
            //propertyGrid2.SelectedObject = ret.ChartArea;

            if (ret != null)
            {
                if (((ret.ChartElementType == ChartElementType.PlottingArea)
                    && (ret.ChartArea.Name =="c3")|| mouseDown) 
                    )
                {
                    chart1.Cursor = Cursors.HSplit;
                    if (!mouseDown)
                    {
                        x = e.X; 
                        y = e.Y;
                        c3y = chart1.ChartAreas["c3"].Position.Y;
                    }
                    else
                    {
                        var c3 = chart1.ChartAreas["c3"];
                        Single a = c3y + Convert.ToSingle((e.Y - y)) * 100 / chart1.Height;
                        a = a >= 0 ? (a > 100 ? 100 : a) : 0;
                        c3.Position.Y = a;
                        var c1 = chart1.ChartAreas["c1"];
                        c1.Position.Height = c3.Position.Y;
                        var c2 = chart1.ChartAreas["c2"];
                        c2.Position.Y = c3.Position.Y + c3.Position.Height;
                        c2.Position.Height = 100 - c2.Position.Y;
                    }
                }
                else
                {
                    chart1.Cursor = Cursors.Default;
                }
                propertyGrid3.SelectedObject = ret;
                if (ret.Series != null)
                    propertyGrid1.SelectedObject = ret.Series;
                if (ret.ChartArea != null)
                    propertyGrid2.SelectedObject = ret.ChartArea;
            }
            currentChartArea = ret.ChartArea;
            if(ret.Series != null)
            {
                ret.Series.MarkerStyle = MarkerStyle.Triangle;
                ret.Series.MarkerColor = Color.Black;
                ret.Series.MarkerSize = 10;
                ret.Series.MarkerStep = 2;
                
            }

        }

        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            var ret = chart1.HitTest(e.X, e.Y);
            if ((ret.ChartElementType == ChartElementType.PlottingArea)
                   && (ret.ChartArea.Name == "c3"))
                mouseDown = true;
            else
                mouseDown = false;
        }

        private void chart1_MouseLeave(object sender, EventArgs e)
        {
            mouseDown = false;
        }
        private void chart1_SizeChanged(object sender, EventArgs e)
        {
            var ppp = Convert.ToSingle( 100) / (chart1.Height-1);
            var c3 = chart1.ChartAreas.FirstOrDefault(x => x.Name == "c3");
            if(c3!=null)
            {
                c3.Position.Height = ppp;
            }
        }

       
    }
}
