using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Algorithm.Core
{
    [Description(@"Nothing yet")]
    public class MACD:Indicator
    {
        public int PeriodShort { get; set; }
        public int PeriodLong { get; set; }
        public int SignalPeriod { get; set; }
        public Window<double> Divergence { get { return this; } }
        public Window<double> MACDSignal = null;
        SimpleMovingAverage SMAShort, SMALong;
        public MACD(int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            PeriodShort = 12;
            PeriodLong = 26;
            SignalPeriod = 9;
        }
        public override void Start(params Window<double>[] values)
        {
            MACDSignal = SimpleMovingAverage(SignalPeriod, SignalPeriod + 2);
            SMAShort = SimpleMovingAverage(PeriodShort, 2);
            SMALong = SimpleMovingAverage(PeriodLong, 2);
        }
        protected override void AfterSetValue(params Window<double>[] values)
        {
            SMAShort.Push(values);
            SMALong.Push(values);
            Value = SMAShort - SMALong;
            MACDSignal.Push(this);
        }
    }
    public partial class IndicatorBase
    {
        public MACD MACD(int size = Window<double>.DefaultDataWindowSize)
        {
            return new MACD(size) { Source = this.Source };
        }
        public static MACD Create_MACD(int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new MACD(size) { Source = source };
        }
    }
}
