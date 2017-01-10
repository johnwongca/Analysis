using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Trade.Analyze;
using Trade.Base;

namespace Trade.Interface.Analyze
{
    public partial class FormCharts : Form
    {
        internal List<ChartStore> store = null;
        FormChartSettings cs = null;
        private Trade.Base.SystemMenu systemMenu;
        const int MENU_OPTIONS = 666;
        public FormCharts()
        {
            systemMenu = Trade.Base.SystemMenu.GetSystemMenu(this);
            InitializeComponent();
            systemMenu.InsertSeparator(0);
            systemMenu.InsertMenu(0, MENU_OPTIONS, "Options...");
            mSymbol = null;
            store = new List<ChartStore>();
        }
        protected override void WndProc ( ref Message msg )
        {
            if (msg.Msg == (int)Trade.Base.WindowMessages.wmSysCommand)
              {
                 switch ( msg.WParam.ToInt32() )
                 {
                    case MENU_OPTIONS:
                         if (cs == null)
                             cs = new FormChartSettings();
                         cs.formChart = this;
                         cs.Show();
                         break;
                 }
              }
              // Call base class function
              base.WndProc(ref msg);
        }
        private void FormCharts_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
        internal Symbol mSymbol;
        public Symbol Symbol
        {
            get { return mSymbol; }
        }
        public void SetSymbol(int symbolID)
        {
            mSymbol = new Symbol(symbolID);
            //mData.Clear();
            store.Clear();
            this.Text = Symbol.SymbolName + " : " + Symbol.Description;
            chart.Clear();
            chart.SetXAxisChartData(Symbol.GetObjects(PriceName.DateTime));

            Trade.Base.Canvas c1 = null;
            ChartStore st = null;
            List<double> ld0 = null, ld1 = null, ld2 = null, ld3 = null, ld4 = null, ld5 = null;
            List<object> temp;

            #region Volume Chart
            /*Volume Chart*/
            ChartData cd1 = new ChartData();
            cd1.Name = "Volume";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Gray;
            cd1.Type = ChartType.Bar;
            cd1.Data.Add(Symbol.GetObjects(PriceName.Volume));
            c1 = chart.AddNew(cd1);
            c1.ChartName = "Vol.";
            c1.MHeight = 5;

            st = new ChartStore() { Name = "Volume Chart", Index = 10, canvas = c1, MHeight = c1.MHeight };
            st.Data.Add(cd1);//[0]

            int days = 5;
            cd1 = new ChartData();
            cd1.Name = "EMA Volume";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.Line;
            cd1.Data.Add(Symbol.GetValues(PriceName.Volume).ExponentialMovingAverage(days).ToListOfObject());
            c1.Data.Add(cd1);

            st.Data.Add(cd1);//[1]

            days = 5;
            cd1 = new ChartData();
            cd1.Name = "ROC Volume";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Right;
            cd1.Color = System.Windows.Media.Colors.Red;
            cd1.Type = ChartType.Line;
            ld1 = Symbol.GetValues(PriceName.Volume);
            ld2 = ld1.NPeriodAgo(days);
            cd1.Data.Add(ld1.RateOfChange(ld2).ToListOfObject());
            c1.Data.Add(cd1);
            
            st.Data.Add(cd1); //[2]
            store.Add(st);

            cd1 = new ChartData();
            cd1.Name = "Horizontal Line";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Right;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.HorizontalLine;
            temp = new List<object>();
            temp.Add((object)0d);
            cd1.Data.Add(temp);
            c1.Data.Add(cd1);
            #endregion

            #region EOD Chart
            /*EOD Chart*/
            cd1 = new ChartData();
            cd1.Name = "EOD";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.DownColor = System.Windows.Media.Colors.Red;
            cd1.Type = ChartType.CandleStick;
            cd1.Data.Add(Symbol.GetObjects(PriceName.High));
            cd1.Data.Add(Symbol.GetObjects(PriceName.Opening));
            cd1.Data.Add(Symbol.GetObjects(PriceName.Closing));
            cd1.Data.Add(Symbol.GetObjects(PriceName.Low));
            cd1.Data.Add(Symbol.GetObjects(PriceName.MedianPrice));
            c1 = chart.AddNew(cd1);
            c1.MHeight = 10;
            c1.ChartName = "EOD";
            st = new ChartStore() { Name = "EOD Chart", Index = 20, canvas = c1, MHeight = c1.MHeight };
            st.Data.Add(cd1);//[0]
            

            days = 50;
            cd1 = new ChartData();
            cd1.Name = "EMA" + days.ToString();
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.Line;
            ld1 = Symbol.GetValues(PriceName.Closing).ExponentialMovingAverage(days);
            cd1.Data.Add(ld1.ToListOfObject());
            c1.Data.Add(cd1);

            st.Data.Add(cd1);//[1]

            days = 10;
            cd1 = new ChartData();
            cd1.Name = "SMA" + days.ToString();
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.DarkOrange;
            cd1.Type = ChartType.Line;
            ld1 = Symbol.GetValues(PriceName.Closing).ExponentialMovingAverage(days);
            cd1.Data.Add(ld1.ToListOfObject());
            c1.Data.Add(cd1);

            st.Data.Add(cd1);//[2]

            List<List<double>> t1 = null;
            t1 = Symbol.GetValues(PriceName.Closing).BollingerBands(ld1, days, 1.6);
            cd1 = new ChartData();
            cd1.Name = "BB" + days.ToString();
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Opacity = 0.1;
            cd1.Type = ChartType.Stripe;
            cd1.Data.Add(t1[0].ToListOfObject());
            cd1.Data.Add(t1[1].ToListOfObject());
            c1.Data.Add(cd1);

            st.Data.Add(cd1);
            store.Add(st);

            #endregion

            #region MACD
            /*MACD Chart*/
            t1 = Symbol.GetValues(PriceName.Closing).MACD(6, 4, 3);
            cd1 = new ChartData();
            cd1.Name = "MACD4-6";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.BlueViolet;
            cd1.Type = ChartType.Line;
            cd1.Data.Add(t1[0].ToListOfObject());
            c1 = chart.AddNew(cd1);
            c1.ChartName = "MACD";
            c1.MHeight = 7;
            st = new ChartStore() { Name = "MACD Chart", Index = 30, canvas = c1, MHeight = c1.MHeight };
            st.Data.Add(cd1);//[0]

            cd1 = new ChartData();
            cd1.Name = "MACD4-6,3";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Red;
            cd1.Type = ChartType.Line;
            cd1.Data.Add(t1[1].ToListOfObject());
            c1.Data.Add(cd1);

            st.Data.Add(cd1);//[1]
            store.Add(st);

            cd1 = new ChartData();
            cd1.Name = "Horizontal Line";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.HorizontalLine;
            temp = new List<object>();
            temp.Add((object)0d);
            cd1.Data.Add(temp);
            c1.Data.Add(cd1);
            #endregion

            #region RSI Chart
            ld1 = Symbol.GetValues(PriceName.Closing);
            ld2 = ld1.Gain();
            ld3 = ld1.Loss();
            ld1 = ld2.SimpleMovingAverage(7).RelativeStrengthIndex(ld3.SimpleMovingAverage(7));

            cd1 = new ChartData();
            cd1.MinY = 0d;
            cd1.MaxY = 100d;
            cd1.Name = "RSI";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Red;
            cd1.Type = ChartType.Line;
            cd1.Data.Add(ld1.ToListOfObject());
            c1 = chart.AddNew(cd1);
            c1.ChartName = "RSI";
            c1.MHeight = 7;

            st = new ChartStore() { Name = "RSI Chart", Index = 40, canvas = c1, MHeight = c1.MHeight };
            st.Data.Add(cd1);

            store.Add(st);


            cd1 = new ChartData();
            cd1.Name = "Horizontal Line";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.HorizontalLine;
            //cd1.Opacity = 0.45;
            temp = new List<object>();
            temp.Add((object)20d);
            temp.Add((object)30d);
            temp.Add((object)70d);
            temp.Add((object)80d);
            cd1.Data.Add(temp);
            c1.Data.Add(cd1);
            #endregion

            #region Ultimate Oscillator
            ld1 = Symbol.Prices.UltimateOscillator(3, 5, 7);

            cd1 = new ChartData();
            cd1.MinY = 0d;
            cd1.MaxY = 100d;
            cd1.Name = "UO";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Red;
            //cd1.Opacity = 0.5;
            cd1.Type = ChartType.Line;
            cd1.Data.Add(ld1.ToListOfObject());
            c1 = chart.AddNew(cd1);
            c1.ChartName = "UO";
            c1.MHeight = 7;

            st = new ChartStore() { Name = "Ultimate Oscillator", Index = 50, canvas = c1, MHeight = c1.MHeight };
            st.Data.Add(cd1);//[0]

            store.Add(st);


            cd1 = new ChartData();
            cd1.Name = "Horizontal Line";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.HorizontalLine;
            //cd1.Opacity = 0.45;
            temp = new List<object>();
            temp.Add((object)20d);
            temp.Add((object)30d);
            temp.Add((object)50d);
            temp.Add((object)70d);
            temp.Add((object)80d);
            cd1.Data.Add(temp);
            c1.Data.Add(cd1);
            #endregion
            
            #region Chande Momentum Oscillator

            ld1 = Symbol.GetValues(PriceName.Closing);
            ld1 = ld1.ChandeMomentumOscillator(5);

            cd1 = new ChartData();
            cd1.MinY = -100d;
            cd1.MaxY = 100d;
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Red;
            cd1.Type = ChartType.Line;
            cd1.Data.Add(ld1.ToListOfObject());
            c1 = chart.AddNew(cd1);
            c1.ChartName = "CMO";
            c1.MHeight = 7;

            st = new ChartStore() { Name = "Chande Momentum Oscillator", Index = 60, canvas = c1, MHeight = c1.MHeight };
            st.Data.Add(cd1);

            store.Add(st);


            cd1 = new ChartData();
            cd1.Name = "Horizontal Line";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.HorizontalLine;
            //cd1.Opacity = 0.45;
            temp = new List<object>();
            temp.Add((object)-50d);
            temp.Add((object)50d);
            cd1.Data.Add(temp);
            c1.Data.Add(cd1);


            #endregion


            #region Directional Movement Indicator

            ld0 = Symbol.GetValues(PriceName.Closing);
            ld1 = Symbol.GetValues(PriceName.High).Gain().Divide(ld0).Multiply(100d); //bull
            ld2 = Symbol.GetValues(PriceName.Low).Loss().Divide(ld0).Multiply(100d); // bear

            ld5 = ld1.Subtract(ld2).Multiply(100d).Divide(ld1.Addit(ld2));



            ld3 = ld1.ExponentialMovingAverage(5);
            ld4 = ld2.ExponentialMovingAverage(5);
            ld5 = ld5.ExponentialMovingAverage(5);

            //bull
            cd1 = new ChartData();
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Green;
            cd1.Opacity = 0.2;
            cd1.Type = ChartType.Bar;
            cd1.Name = "DI+";
            cd1.Data.Add(ld1.ToListOfObject());
            c1 = chart.AddNew(cd1);
            c1.ChartName = "DMI";
            c1.MHeight = 7;

            st = new ChartStore() { Name = "Directional Movement Indicator", Index = 70, canvas = c1, MHeight = c1.MHeight };
            st.Data.Add(cd1);//[0] bull

            store.Add(st);

            //bear
            cd1 = new ChartData();
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Red;
            cd1.Opacity = 0.2;
            cd1.Type = ChartType.Bar;
            cd1.Name = "DI-";
            cd1.Data.Add(ld2.ToListOfObject());
            c1.Data.Add(cd1);
            st.Data.Add(cd1);//[1] bear

            //bull MA
            cd1 = new ChartData();
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.Line;
            cd1.Name = "MA DI+";
            cd1.Data.Add(ld3.ToListOfObject());
            c1.Data.Add(cd1);
            st.Data.Add(cd1);//[2] bull

            //Bear MA
            cd1 = new ChartData();
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Red;
            cd1.Type = ChartType.Line;
            cd1.Name = "MA DI-";
            cd1.Data.Add(ld4.ToListOfObject());
            c1.Data.Add(cd1);
            st.Data.Add(cd1);//[3] bear

            //Normalize
            cd1 = new ChartData();
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Right;
            cd1.Color = System.Windows.Media.Colors.DarkOliveGreen;
            cd1.Type = ChartType.Line;
            cd1.Name = "ADX";
            cd1.Data.Add(ld5.ToListOfObject());
            c1.Data.Add(cd1);
            st.Data.Add(cd1);//[4] Normalize


            //Signal
            cd1 = new ChartData();
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Right;
            cd1.Color = System.Windows.Media.Colors.Purple;
            cd1.Type = ChartType.Line;
            cd1.Name = "ADX-S";
            cd1.Data.Add(ld5.ExponentialMovingAverage(3).ToListOfObject());
            c1.Data.Add(cd1);
            st.Data.Add(cd1);//[5] Signal

            cd1 = new ChartData();
            cd1.Name = "Horizontal Line";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Right;
            cd1.Color = System.Windows.Media.Colors.DarkOliveGreen;
            cd1.Type = ChartType.HorizontalLine;
            
            temp = new List<object>();
            temp.Add((object)0d);
            cd1.Data.Add(temp);
            c1.Data.Add(cd1);


            #endregion



            #region Stochastic Oscillator
            /*
            ld1 = Symbol.GetValues(PriceName.Closing);
            ld2 = ld1;
            ld2 = ld1.Gain();
            ld3 = ld1.Loss();
            ld2 = ld2.SimpleMovingAverage(10).RelativeStrengthIndex(ld3.SimpleMovingAverage(10));
            
            ld1 = ld2.StochasticOscillator(ld2.Lowest(5), ld2.Highest(5));

            cd1 = new ChartData();
            cd1.Name = "SO-%k";
            cd1.MinY = 0d;
            cd1.MaxY = 100d;
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.BlueViolet;
            //cd1.Opacity = 0.5;
            cd1.Type = ChartType.Line;
            cd1.Data.Add(ld1.ToListOfObject());
            c1 = chart.AddNew(cd1);
            c1.ChartName = "SO";
            c1.MHeight = 7;

            st = new ChartStore() { Name = "SO", Index = 60, canvas = c1, MHeight = c1.MHeight };
            st.Data.Add(cd1);


            ld1 = ld1.ExponentialMovingAverage(3);



            store.Add(st);
            cd1 = new ChartData();
            cd1.Name = "SO-%D";
            cd1.MinY = 0d;
            cd1.MaxY = 100d;
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Red;
            //cd1.Opacity = 0.5;
            cd1.Type = ChartType.Line;
            cd1.Data.Add(ld1.ToListOfObject());
            c1.Data.Add(cd1);

            cd1 = new ChartData();
            cd1.Name = "Horizontal Line";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.HorizontalLine;
            //cd1.Opacity = 0.45;
            temp = new List<object>();
            temp.Add((object)20d);
            temp.Add((object)30d);
            temp.Add((object)70d);
            temp.Add((object)80d);
            cd1.Data.Add(temp);
            c1.Data.Add(cd1);
            */
            #endregion

            #region Commodity Channel Index
            /*
            ld1 = Symbol.GetValues(PriceName.TypicalClosing);
            ld1 = ld1.CommodityChannelIndex(ld1.SimpleMovingAverage(14), 14);

            cd1 = new ChartData();
            cd1.Name = "CCI";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Red;
            cd1.Type = ChartType.Line;
            cd1.Data.Add(ld1.ToListOfObject());
            c1 = chart.AddNew(cd1);
            c1.ChartName = "CCI";
            c1.MHeight = 7;

            st = new ChartStore() { Name = "Commodity Channel Index", Index = 80, canvas = c1, MHeight = c1.MHeight };
            st.Data.Add(cd1);

            store.Add(st);


            cd1 = new ChartData();
            cd1.Name = "Horizontal Line";
            cd1.ZeroLocation = ZeroYLocation.Bottom;
            cd1.YAxisLocation = YAxisLocation.Left;
            cd1.Color = System.Windows.Media.Colors.Blue;
            cd1.Type = ChartType.HorizontalLine;
            //cd1.Opacity = 0.45;
            temp = new List<object>();
            temp.Add((object)0d);
            temp.Add((object)-100d);
            temp.Add((object)100d);
            cd1.Data.Add(temp);
            c1.Data.Add(cd1);

            */
            #endregion
            chart.ShowCharts(true);
            if (cs != null)
                cs.ApplyCurrentProfile();
        }
        
    }
    public class ChartStore
    {
        public int Index = 0;
        public string Name = "";
        public Trade.Base.Canvas canvas = null;
        public double MHeight = 10;
        public List<ChartData> Data = new List<ChartData>();
    }
}
