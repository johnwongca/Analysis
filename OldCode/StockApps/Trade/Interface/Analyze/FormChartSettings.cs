using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Trade.Analyze;
using Trade.Base;

namespace Trade.Interface.Analyze
{
    public partial class FormChartSettings : Form
    {
        List<Control> ControlList = null;
        void InitializeControlList()
        {
            ControlList = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                            .Where(x => { return x.MemberType == MemberTypes.Field; })
                                            .Select(x => { return this.GetType().InvokeMember(x.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, this, null); })
                                            .Where(x =>
                                            {
                                                if (x is ComboBox || x is NumericUpDown)
                                                    return ((Control)x).Tag != null;
                                                return false;
                                            }
                                        )
                                .Select(x => { return (Control) x; }).Distinct().ToList<Control>();
        }
        private bool calculating = false;
        public FormCharts formChart = null;
        List<ChartStore> store
        {
            get { return formChart == null ? null : formChart.store; }
        }
        Symbol Symbol
        {
            get { return formChart == null ? null : formChart.Symbol; }
        }
        public FormChartSettings()
        {
            InitializeComponent();
            Volume_MovingAverage_AverageType.SelectedIndex = 0;
            EOD_MovingAverage_AverageType.SelectedIndex = 0;
            EOD_BB_MovingAverage_AverageType.SelectedIndex = 1;
            MACD_Short_MovingAverage_AverageType.SelectedIndex = 0;
            MACD_Long_MovingAverage_AverageType.SelectedIndex = 0;
            MACD_Signal_MovingAverage_AverageType.SelectedIndex = 0;
            RSI_Gain_MovingAverage_AverageType.SelectedIndex = 1;
            RSI_Loss_MovingAverage_AverageType.SelectedIndex = 1;
            DMI_DMPlus_MovingAverage_AverageType.SelectedIndex = 0;
            DMI_DMMinus_MovingAverage_AverageType.SelectedIndex = 0;
            DMI_ADI_MovingAverage_AverageType.SelectedIndex = 0;
            DMI_ADISignal_MovingAverage_AverageType.SelectedIndex = 0;
            InitializeControlList();
            ReadSettings();
        }
        private void FormChartSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
        private void ReadSettings()
        {
            SavedSettings t = (SavedSettings)cbProfile.SelectedItem;
            cbProfile.Items.Clear();
            foreach (SavedSettings x in SavedSettings.GetSettings())
                cbProfile.Items.Add(x);
            if (t != null)
                cbProfile.SelectedItem = cbProfile.Items.Cast<SavedSettings>().FirstOrDefault(x => { return x.Name == t.Name; });
            else
                cbProfile.SelectedItem = cbProfile.Items.Cast<SavedSettings>().FirstOrDefault(x => { return x.Name == "Default"; });
        }
        private void SaveSettings()
        {
            SavedSettings s = new SavedSettings();
            s.Name = cbProfile.Text;
            s.Delete();
            s.Items.Clear();
            foreach (var c in ControlList)
            {
                SavedSettingsItem a = new SavedSettingsItem() { Key = c.Tag.ToString() };
                if (c is ComboBox)
                    a.Value = ((ComboBox)c).SelectedIndex.ToString();
                if (c is NumericUpDown)
                    a.Value = ((NumericUpDown)c).Value.ToString();
                s.Items.Add(a);
            }
            s.Save();
            ReadSettings();
        }
        private void SetSettings(SavedSettings s)
        {
            try
            {
                calculating = true;
                Control c;
                foreach (var a in s.Items)
                {
                    c = ControlList.FirstOrDefault(x => { return x.Tag.ToString() == a.Key; });
                    if (c != null)
                    {
                        if (c is ComboBox)
                            ((ComboBox)c).SelectedIndex = Int32.Parse(a.Value);
                        else if (c is NumericUpDown)
                            ((NumericUpDown)c).Value = System.Decimal.Parse(a.Value);
                    }
                }
            }
            finally
            {
                calculating = false;
            }
        }
        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ReadSettings();
        }
        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            SavedSettings s = new SavedSettings();
            s.Name = cbProfile.Text;
            s.Delete();
            ReadSettings();
        }
        public void ApplyCurrentProfile()
        {
            ChangeVolumeChart(null, null);
            ChangeEODChart(null, null);
            ChangeMACDChart(null, null);
            ChangeRSIChart(null, null);
            ChangeUOChart(null, null);
            ChangeCMOChart(null, null);
            ChangeDMIChart(null, null);
        }
        private void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSettings((SavedSettings)cbProfile.SelectedItem);
            ApplyCurrentProfile();
        }
        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            ApplyCurrentProfile();
        }

        private List<double> GetMA(List<double> data, ComboBox maType, NumericUpDown maDays)
        {
            if (maType.SelectedIndex == 0)
                return data.ExponentialMovingAverage((int)maDays.Value);
            if (maType.SelectedIndex == 1)
                return data.SimpleMovingAverage((int)maDays.Value);
            if (maType.SelectedIndex == 2)
                return data.WeightedmovingAverage((int)maDays.Value);
            if (maType.SelectedIndex == 3)
                return data.VariableMovingAverage((int)maDays.Value);
            return null;
        }

        private void ChangeVolumeChart(object sender, EventArgs e)
        {
            if (calculating) return;
            try
            {
                calculating = true;
                if (store == null) return;
                ChartStore cs = store.FirstOrDefault(x => { return x.Index == 10; });
                if (cs == null) return;
                List<double> ld1 = null, ld0 = null, ld2 = null;
                /*Moving Average*/
                ChartData cd1 = cs.Data[1];
                ld0 = Symbol.GetValues(PriceName.Volume);
                if (Volume_MovingAverage_AverageType.SelectedIndex == 0)
                {
                    cd1.Name = "EMA Volume";
                    ld1 = ld0.ExponentialMovingAverage((int)Volume_MovingAverage_Days.Value);
                }
                else if (Volume_MovingAverage_AverageType.SelectedIndex == 1)
                {
                    cd1.Name = "SMA Volume";
                    ld1 = ld0.SimpleMovingAverage((int)Volume_MovingAverage_Days.Value);
                }
                else if (Volume_MovingAverage_AverageType.SelectedIndex == 2)
                {
                    cd1.Name = "WMA Volume";
                    ld1 = ld0.WeightedmovingAverage((int)Volume_MovingAverage_Days.Value);
                }
                else if (Volume_MovingAverage_AverageType.SelectedIndex == 3)
                {
                    cd1.Name = "VMA Volume";
                    ld1 = ld0.VariableMovingAverage((int)Volume_MovingAverage_Days.Value);
                }
                cd1.Data.Clear();
                cd1.Data.Add(ld1.ToListOfObject());

                cd1 = cs.Data[2]; //Volume Rate Of Change
                cd1.Data.Clear();

                ld1 = ld0;
                ld2 = ld1.NPeriodAgo((int)Volume_MovingAverage_Days.Value);
                cd1.Data.Add(ld1.RateOfChange(ld2).ToListOfObject());

                cs.canvas.RepaintChart();
            }
            finally
            {
                calculating = false;
            }
        }
        private void ChangeEODChart(object sender, EventArgs e)
        {
            if (calculating) return;
            try
            {
                calculating = true;
                if (store == null) return;
                ChartStore cs = store.FirstOrDefault(x => { return x.Index == 20; });
                if (cs == null) return;
                List<double> ld1 = null, ld0 = null;
                /*Moving Average*/
                ChartData cd1 = cs.Data[1];
                ld0 = Symbol.GetValues(PriceName.Closing);
                if (EOD_MovingAverage_AverageType.SelectedIndex == 0)
                {
                    cd1.Name = "EMA";
                    ld1 = ld0.ExponentialMovingAverage((int)EOD_MovingAverage_Days.Value);
                }
                else if (EOD_MovingAverage_AverageType.SelectedIndex == 1)
                {
                    cd1.Name = "SMA";
                    ld1 = ld0.SimpleMovingAverage((int)EOD_MovingAverage_Days.Value);
                }
                else if (EOD_MovingAverage_AverageType.SelectedIndex == 2)
                {
                    cd1.Name = "WMA";
                    ld1 = ld0.WeightedmovingAverage((int)EOD_MovingAverage_Days.Value);
                }
                else if (EOD_MovingAverage_AverageType.SelectedIndex == 3)
                {
                    cd1.Name = "VMA";
                    ld1 = ld0.VariableMovingAverage((int)EOD_MovingAverage_Days.Value);
                }
                cd1.Data.Clear();
                cd1.Data.Add(ld1.ToListOfObject());

                cd1 = cs.Data[2];
                if (EOD_BB_MovingAverage_AverageType.SelectedIndex == 0)
                {
                    cd1.Name = "EMA";
                    ld1 = ld0.ExponentialMovingAverage((int)EOD_BB_MovingAverage_Days.Value);
                }
                else if (EOD_BB_MovingAverage_AverageType.SelectedIndex == 1)
                {
                    cd1.Name = "SMA";
                    ld1 = ld0.SimpleMovingAverage((int)EOD_BB_MovingAverage_Days.Value);
                }
                else if (EOD_BB_MovingAverage_AverageType.SelectedIndex == 2)
                {
                    cd1.Name = "WMA";
                    ld1 = ld0.WeightedmovingAverage((int)EOD_BB_MovingAverage_Days.Value);
                }
                else if (EOD_BB_MovingAverage_AverageType.SelectedIndex == 3)
                {
                    cd1.Name = "VMA";
                    ld1 = ld0.VariableMovingAverage((int)EOD_BB_MovingAverage_Days.Value);
                }
                cd1.Data.Clear();
                cd1.Data.Add(ld1.ToListOfObject());

                cd1 = cs.Data[3];
                List<List<double>> t1 = null;
                t1 = Symbol.GetValues(PriceName.Closing).BollingerBands(ld1, (int)EOD_BB_MovingAverage_Days.Value, (double)EOD_BB_Coefficient.Value / 10d);
                cd1.Data.Clear();
                cd1.Data.Add(t1[0].ToListOfObject());
                cd1.Data.Add(t1[1].ToListOfObject());

                cd1 = cs.Data[4];
                ld0 = Symbol.GetValues(PriceName.Closing);
                ld1 = ld0.NPeriodAgo((int)EOD_PROC_Days.Value);
                cd1.Data.Clear();
                cd1.Data.Add(ld0.RateOfChange(ld1).ToListOfObject());
                cs.canvas.RepaintChart();
            }
            finally
            {
                calculating = false;
            }
        }
        private void ChangeMACDChart(object sender, EventArgs e)
        {
            if (calculating) return;
            try
            {
                calculating = true;
                if (store == null) return;
                ChartStore cs = store.FirstOrDefault(x => { return x.Index == 30; });
                if (cs == null) return;
                List<double> ld1 = null, ld0 = null, ld2 = null, ld3 = null;


                ld0 = Symbol.GetValues(PriceName.Closing);
                if (MACD_Short_MovingAverage_AverageType.SelectedIndex == 0)
                    ld1 = ld0.ExponentialMovingAverage((int)MACD_Short_MovingAverage_Days.Value);
                else if (MACD_Short_MovingAverage_AverageType.SelectedIndex == 1)
                    ld1 = ld0.SimpleMovingAverage((int)MACD_Short_MovingAverage_Days.Value);
                else if (MACD_Short_MovingAverage_AverageType.SelectedIndex == 2)
                    ld1 = ld0.WeightedmovingAverage((int)MACD_Short_MovingAverage_Days.Value);
                else if (MACD_Short_MovingAverage_AverageType.SelectedIndex == 3)
                    ld1 = ld0.VariableMovingAverage((int)MACD_Short_MovingAverage_Days.Value);

                if (MACD_Long_MovingAverage_AverageType.SelectedIndex == 0)
                    ld2 = ld0.ExponentialMovingAverage((int)MACD_Long_MovingAverage_Days.Value);
                else if (MACD_Long_MovingAverage_AverageType.SelectedIndex == 1)
                    ld2 = ld0.SimpleMovingAverage((int)MACD_Long_MovingAverage_Days.Value);
                else if (MACD_Long_MovingAverage_AverageType.SelectedIndex == 2)
                    ld2 = ld0.WeightedmovingAverage((int)MACD_Long_MovingAverage_Days.Value);
                else if (MACD_Long_MovingAverage_AverageType.SelectedIndex == 3)
                    ld2 = ld0.VariableMovingAverage((int)MACD_Long_MovingAverage_Days.Value);

                ld2 = ld1.Subtract(ld2);
                if (MACD_Signal_MovingAverage_AverageType.SelectedIndex == 0)
                    ld3 = ld2.ExponentialMovingAverage((int)MACD_Signal_MovingAverage_Days.Value);
                else if (MACD_Signal_MovingAverage_AverageType.SelectedIndex == 1)
                    ld3 = ld2.SimpleMovingAverage((int)MACD_Signal_MovingAverage_Days.Value);
                else if (MACD_Signal_MovingAverage_AverageType.SelectedIndex == 2)
                    ld3 = ld2.WeightedmovingAverage((int)MACD_Signal_MovingAverage_Days.Value);
                else if (MACD_Signal_MovingAverage_AverageType.SelectedIndex == 3)
                    ld3 = ld2.VariableMovingAverage((int)MACD_Signal_MovingAverage_Days.Value);

                ChartData cd1 = cs.Data[0];
                cd1.Data.Clear();
                cd1.Name = String.Format("MACD {0}-{1}", (int)MACD_Short_MovingAverage_Days.Value, (int)MACD_Long_MovingAverage_Days.Value);
                cd1.Data.Add(ld2.ToListOfObject());

                cd1 = cs.Data[1];
                cd1.Data.Clear();
                cd1.Name = String.Format("MACD {0}-{1}, {2}", (int)MACD_Short_MovingAverage_Days.Value, (int)MACD_Long_MovingAverage_Days.Value, (int)MACD_Signal_MovingAverage_Days.Value);
                cd1.Data.Add(ld3.ToListOfObject());
                cs.canvas.RepaintChart();
            }
            finally
            {
                calculating = false;
            }
        }
        private void ChangeRSIChart(object sender, EventArgs e)
        {
            if (calculating) return;
            try
            {
                calculating = true;
                if (store == null) return;
                ChartStore cs = store.FirstOrDefault(x => { return x.Index == 40; });
                if (cs == null) return;
                List<double> ld1 = null, ld0 = null, ld2 = null, ld3 = null;

                ld0 = Symbol.GetValues(PriceName.Closing);
                ld1 = ld0.Gain();
                ld2 = ld0.Loss();
                if (RSI_Gain_MovingAverage_AverageType.SelectedIndex == 0)
                    ld1 = ld1.ExponentialMovingAverage((int)RSI_Gain_MovingAverage_Days.Value);
                else if (RSI_Gain_MovingAverage_AverageType.SelectedIndex == 1)
                    ld1 = ld1.SimpleMovingAverage((int)RSI_Gain_MovingAverage_Days.Value);
                else if (RSI_Gain_MovingAverage_AverageType.SelectedIndex == 2)
                    ld1 = ld1.WeightedmovingAverage((int)RSI_Gain_MovingAverage_Days.Value);
                else if (RSI_Gain_MovingAverage_AverageType.SelectedIndex == 3)
                    ld1 = ld1.VariableMovingAverage((int)RSI_Gain_MovingAverage_Days.Value);

                if (RSI_Loss_MovingAverage_AverageType.SelectedIndex == 0)
                    ld2 = ld2.ExponentialMovingAverage((int)RSI_Loss_MovingAverage_Days.Value);
                else if (RSI_Loss_MovingAverage_AverageType.SelectedIndex == 1)
                    ld2 = ld2.SimpleMovingAverage((int)RSI_Loss_MovingAverage_Days.Value);
                else if (RSI_Loss_MovingAverage_AverageType.SelectedIndex == 2)
                    ld2 = ld2.WeightedmovingAverage((int)RSI_Loss_MovingAverage_Days.Value);
                else if (RSI_Loss_MovingAverage_AverageType.SelectedIndex == 3)
                    ld2 = ld2.VariableMovingAverage((int)RSI_Loss_MovingAverage_Days.Value);
                ld3 = ld1.RelativeStrengthIndex(ld2);
                ChartData cd1 = cs.Data[0];
                cd1.Data.Clear();
                cd1.Name = "RSI";
                cd1.Data.Add(ld3.ToListOfObject());
                cs.canvas.RepaintChart();
            }
            finally
            {
                calculating = false;
            }
        }
        private void ChangeUOChart(object sender, EventArgs e)
        {
            if (calculating) return;
            try
            {
                calculating = true;
                if(UO_Period1.Value >= UO_Period2.Value)
                    UO_Period2.Value = UO_Period1.Value + 1;
                if (UO_Period2.Value >= UO_Period3.Value)
                    UO_Period3.Value = UO_Period2.Value + 1;
                if (store == null) return;
                ChartStore cs = store.FirstOrDefault(x => { return x.Index == 50; });
                if (cs == null) return;
                List<double> ld1 = null;
                ld1 = Symbol.Prices.UltimateOscillator((int)UO_Period1.Value, (int)UO_Period2.Value, (int)UO_Period3.Value);
                ChartData cd1 = cs.Data[0];
                cd1.Data.Clear();
                cd1.Name = "UO";
                cd1.Data.Add(ld1.ToListOfObject());
                cs.canvas.RepaintChart();
            }
            finally
            {
                calculating = false;
            }
        }
        private void ChangeCMOChart(object sender, EventArgs e)
        {
            if (calculating) return;
            try
            {
                calculating = true;
                if (store == null) return;
                ChartStore cs = store.FirstOrDefault(x => { return x.Index == 60; });
                if (cs == null) return;
                List<double> ld1 = null;
                ld1 = Symbol.GetValues(PriceName.Closing);
                ld1 = ld1.ChandeMomentumOscillator((int)CMO_Period.Value);
                ChartData cd1 = cs.Data[0];
                cd1.Data.Clear();
                cd1.Name = "CMO";
                cd1.Data.Add(ld1.ToListOfObject());
                cs.canvas.RepaintChart();
            }
            finally
            {
                calculating = false;
            }
        }

        private void ChangeDMIChart(object sender, EventArgs e)
        {
            if (calculating) return;
            try
            {
                calculating = true;
                if (store == null) return;
                ChartStore cs = store.FirstOrDefault(x => { return x.Index == 70; });
                if (cs == null) return;
                List<double> ld0 = null, ld1 = null, ld2 = null, ld3 = null, ld4 = null, ld5 = null, ld6 = null;
                ld0 = Symbol.GetValues(PriceName.Closing);
                ld1 = Symbol.GetValues(PriceName.High).Gain().Divide(ld0).Multiply(100d); //bull
                ld2 = Symbol.GetValues(PriceName.Low).Loss().Divide(ld0).Multiply(100d); // bear

                ld5 = ld1.Subtract(ld2).Multiply(100d).Divide(ld1.Addit(ld2));



                ld3 = GetMA(ld1, DMI_DMPlus_MovingAverage_AverageType, DMI_DMPlus_MovingAverage_Days);
                ld4 = GetMA(ld2, DMI_DMMinus_MovingAverage_AverageType, DMI_DMMinus_MovingAverage_Days);
                ld5 = GetMA(ld5, DMI_ADI_MovingAverage_AverageType, DMI_ADI_MovingAverage_Days);
                ld6 = GetMA(ld5, DMI_ADISignal_MovingAverage_AverageType, DMI_ADISignal_MovingAverage_Days);

                ChartData cd1 = cs.Data[0];
                cd1.Data.Clear();
                cd1.Name = "DI+";
                cd1.Data.Add(ld1.ToListOfObject());

                cd1 = cs.Data[1];
                cd1.Data.Clear();
                cd1.Name = "DI-";
                cd1.Data.Add(ld2.ToListOfObject());

                cd1 = cs.Data[2];
                cd1.Data.Clear();
                cd1.Name = "MA DI+";
                cd1.Data.Add(ld3.ToListOfObject());

                cd1 = cs.Data[3];
                cd1.Data.Clear();
                cd1.Name = "MA DI-";
                cd1.Data.Add(ld4.ToListOfObject());

                cd1 = cs.Data[4];
                cd1.Data.Clear();
                cd1.Name = "ADX";
                cd1.Data.Add(ld5.ToListOfObject());

                cd1 = cs.Data[5];
                cd1.Data.Clear();
                cd1.Name = "ADX-S";
                cd1.Data.Add(ld6.ToListOfObject());

                cs.canvas.RepaintChart();
            }
            finally
            {
                calculating = false;
            }
        }

        
    }
    public class SavedSettings
    {
        public string Name;
        List<SavedSettingsItem> m_Items = null;
        public List<SavedSettingsItem> Items
        {
            get 
            {
                if (m_Items == null)
                    Read(Name);
                return m_Items; 
            }
        }
        public void Delete()
        {
            Delete(Name);
        }
        public void Delete(string name)
        {
            Program.RunSQL("delete from A.SavedSettings where Name = " + Name.ToSQLString());
        }
        public void Read(string Name)
        {
            this.m_Items = null;
            DataTable dt = Program.GetDataSet("select [Key],[Value] from A.SavedSettings where Name = " + Name.ToSQLString());
            m_Items = dt.AsEnumerable().Select(x => { return new SavedSettingsItem() { Key = x.Field<string>("Key"), Value = x.Field<string>("Value") }; }).ToList<SavedSettingsItem>();
            GC.Collect();
        }
        public void Save()
        {
            Save(Name);
        }
        public void Save(string name)
        {
            string sql = "update A.SavedSettings set [Value] = {2} where [Name] = {0} and [Key] = {1}\n"+
                         "if @@rowcount = 0\n" +
                         "  insert into A.SavedSettings([Name], [Key], [Value]) values({0}, {1}, {2})";
            foreach (SavedSettingsItem item in m_Items)
                Program.RunSQL(string.Format(sql, name.ToSQLString(), item.Key.ToSQLString(), item.Value.ToSQLString()));
        }
        public static List<SavedSettings> GetSettings()
        {
            DataTable dt = Program.GetDataSet("select distinct Name from  A.SavedSettings order by 1");
            return dt.AsEnumerable().Select(x => { return new SavedSettings() { Name = x.Field<string>("Name") }; }).ToList<SavedSettings>();
        }
        override public string ToString()
        {
            return Name;
        }
    }
    public class SavedSettingsItem
    {
        public string Value;
        public string Key;
    }
}
