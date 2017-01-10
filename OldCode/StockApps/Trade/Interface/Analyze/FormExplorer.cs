using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Trade.Base;
using Trade.Analyze;
using Trade.Interface.Transaction;

namespace Trade.Interface.Analyze
{
    public partial class FormExplorer : Trade.Base.BaseForm
    {
        public FormExplorer()
        {
            delMiveToMenueOnClick = new EventHandler(miMiveToMenueOnClick);
            InitializeComponent();
        }
        private void WriteChart1()
        {
            if (bsSymbolTreeList.Current != null)
            {
                WriteChart((int)((DataRowView)bsSymbolTreeList.Current)["SymbolID"]);
            }
        }
        private void WriteChart2()
        {
            if (bsSymbolTreeCustomList.Current != null)
            {
                WriteChart((int)((DataRowView)bsSymbolTreeCustomList.Current)["SymbolID"]);
            }
        }
        private void bsSymbolTreeList_CurrentChanged(object sender, EventArgs e)
        {
            WriteChart1();
        }
        private void tc1_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteChart1();
        }
        private ToolStripMenuItem CreateMenuItem(TreeNode n)
        {
            ToolStripMenuItem r = new ToolStripMenuItem(n.Text);
            r.Tag = n;
            r.Click += delMiveToMenueOnClick;
            return r;
        }
        private void PopulateMenu(ToolStripMenuItem item)
        {
            if (item == null)
            {
                item = miMoveTo;
                item.Tag = dbTree2;
                foreach (TreeNode i in dbTree2.Nodes)
                {
                    ToolStripMenuItem x = CreateMenuItem(i);
                    item.DropDownItems.Add(x);
                    PopulateMenu(x);
                }
            }
            else
            {
                foreach (TreeNode i in ((TreeNode)item.Tag).Nodes)
                {
                    ToolStripMenuItem x = CreateMenuItem(i);
                    item.DropDownItems.Add(x);
                    PopulateMenu(x);
                }
            }
            
        }
        private EventHandler delMiveToMenueOnClick;
        private void miMiveToMenueOnClick(object sender, EventArgs e)
        {
            ToolStripMenuItem s = (ToolStripMenuItem)sender;
            if (s.Tag == null)
                return;
            if (!(s.Tag is TreeNode))
                return;
            TreeNode n = (TreeNode)s.Tag;
            if (!(n.Tag is DbTreeData))
                return;
            DbTreeData d = (DbTreeData)n.Tag; 
            DataGridView g = null;
            if (tbMain.SelectedTab == page1)
            {
                g = dataGridView1;
                miSeparater.Visible = false;
                miRemove.Visible = false;
            }
            else
            {
                g = dataGridView3;
                miSeparater.Visible = true;
                miRemove.Visible = true;
            }
            misMoveTo.Close();
            foreach (DataGridViewRow r in g.SelectedRows)
                Program.RunSQL("exec A.[SymbolTreeCustom_SetSecurity] " + d.ID.ToString() + "," + ((int)r.Cells[0].Value).ToString());
            this.ReOpen(bsSymbolTreeCustomList);
        }
        private void misMoveTo_Opening(object sender, CancelEventArgs e)
        {
            DataGridView g = null;
            if (tbMain.SelectedTab == page1)
            {
                g = dataGridView1;
                miSeparater.Visible = false;
                miRemove.Visible = false;
            }
            else
            {
                g = dataGridView3;
                miSeparater.Visible = true;
                miRemove.Visible = true;
            }
            if (g.SelectedRows.Count == 0)
            {
                e.Cancel = true;
                return;
            }
            miMoveTo.DropDownItems.Clear();
            PopulateMenu(null);
        }
        private void moveToToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataGridView g = dataGridView3;

        }

        private void bsSymbolTreeCustomList_CurrentChanged(object sender, EventArgs e)
        {
            WriteChart2();
        }
        private void miRemove_Click(object sender, EventArgs e)
        {
            if (bsSymbolTreeCustomList.Current == null)
                return;
            Program.RunSQL("exec A.[SymbolTreeCustom_SetSecurity] 0," + ((int)((DataRowView)bsSymbolTreeCustomList.Current)["SymbolID"]).ToString());
            this.ReOpen(bsSymbolTreeCustomList);
        }
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteChart2();
        }
        private void WriteChart(int symbolID)
        {
            SChart chart = null;
            if (tbMain.SelectedIndex == 0 && tc1.SelectedIndex == 1)
                chart = sChart1;
            if (tbMain.SelectedIndex == 1 && tc2.SelectedIndex == 1)
                chart = sChart2;
            if (chart == null)
                return;
            Symbol symbol = new Symbol(symbolID);
            Trade.Base.Canvas c1 = null;
            chart.Clear();
            chart.SetXAxisChartData(symbol.GetObjects(PriceName.DateTime));


            ChartData cd1 = new ChartData();
            cd1.Name = "Volume";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Gray;
            cd1.Type = ChartType.Bar;
            cd1.Data.Add(symbol.GetObjects(PriceName.Volume));
            c1 = chart.AddNew(cd1);
            c1.MHeight = 5;


            cd1 = new ChartData();
            cd1.Name = "Day";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.DownColor = System.Windows.Media.Colors.Red;
            cd1.Type = ChartType.CandleStick;
            cd1.Data.Add(symbol.GetObjects(PriceName.High));
            cd1.Data.Add(symbol.GetObjects(PriceName.Opening));
            cd1.Data.Add(symbol.GetObjects(PriceName.Closing));
            cd1.Data.Add(symbol.GetObjects(PriceName.Low));
            cd1.Data.Add(symbol.GetObjects(PriceName.MedianPrice));
            c1 = chart.AddNew(cd1);
            

            int days = 50;
            cd1 = new ChartData();
            cd1.Name = "EMA" + days.ToString();
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.Line;
            var ma = symbol.GetValues(PriceName.Closing).SimpleMovingAverage(days);
            cd1.Data.Add(ma.ToListOfObject());
            c1.Data.Add(cd1);

            List<List<double>> t1 = null;
            t1 = symbol.GetValues(PriceName.Closing).BollingerBands(ma, days, 1.8);
            cd1 = new ChartData();
            cd1.Name = "BB" + days.ToString();
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Opacity = 0.08;
            cd1.Type = ChartType.Stripe;
            cd1.Data.Add(t1[0].ToListOfObject());
            cd1.Data.Add(t1[1].ToListOfObject());
            c1.Data.Add(cd1);

            t1 = symbol.GetValues(PriceName.Closing).MACD(6, 4, 3);
            cd1 = new ChartData();
            cd1.Name = "MACD6-4";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.BlueViolet;
            cd1.Type = ChartType.Line;
            cd1.Data.Add(t1[0].ToListOfObject());
            c1 = chart.AddNew(cd1);

            cd1 = new ChartData();
            cd1.Name = "MACD6-4,3" ;
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Red;
            cd1.Type = ChartType.Line;
            cd1.Data.Add(t1[1].ToListOfObject());
            c1.Data.Add(cd1);

            cd1 = new ChartData();
            cd1.Name = "Horizontal Line";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.HorizontalLine;
            List<object> temp = new List<object>();
            temp.Add((object)0d);
            cd1.Data.Add(temp);
            c1.Data.Add(cd1);

            chart.ShowCharts(true);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            BindingSource bs = null;
            if (tbMain.SelectedTab == page1)
                bs = bsSymbolTreeList;
            else
                bs = bsSymbolTreeCustomList;
            if (bs.Current != null)
            {
                Program.ShowChart((int)((DataRowView)bs.Current)["SymbolID"]);
            }
        }

        private void miBuy_Click(object sender, EventArgs e)
        {
            BindingSource bs = null;
            if (tbMain.SelectedTab == page1)
                bs = bsSymbolTreeList;
            else
                bs = bsSymbolTreeCustomList;
            if (bs.Current != null)
            {
                BuySell.BuyInitial((int)((DataRowView)bs.Current)["SymbolID"]);
            }
        }

        private void miShowExtendedNewChart_Click(object sender, EventArgs e)
        {
            BindingSource bs = null;
            if (tbMain.SelectedTab == page1)
                bs = bsSymbolTreeList;
            else
                bs = bsSymbolTreeCustomList;
            if (bs.Current != null)
            {
                Program.ShowNewChart((int)((DataRowView)bs.Current)["SymbolID"]);
            }
        }

        private void miShowExtendedChart1_Click(object sender, EventArgs e)
        {
            BindingSource bs = null;
            if (tbMain.SelectedTab == page1)
                bs = bsSymbolTreeList;
            else
                bs = bsSymbolTreeCustomList;
            if (bs.Current != null)
            {
                Program.ShowChart1((int)((DataRowView)bs.Current)["SymbolID"]);
            }
        }



        
    }
}
