using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Algorithm.Core
{
    [Indicator, Description(@"This is the Test1 indicator")]
    public sealed class Test1Indicator : Indicator
    {
        #region MAs
        bool MAUseSMA = false;
        [InputInt(DefaultValue=200, FromValue=1, ToValue = 9000, Interval = 1)]
        
        public int MALongPeriod { get; set; }
        [Output]
        public Indicator MALong { get; set; }
        
        [InputInt(DefaultValue = 100, FromValue = 1, ToValue = 9000, Interval = 1)]
        public int MAMedianPeriod { get; set; }
        [Output]
        public Indicator MAMedian { get; set; }

        [InputInt(DefaultValue = 20, FromValue = 1, ToValue = 9000, Interval = 1)]
        public int MAShortPeriod { get; set; }
        [Output]
        public Indicator MAShort { get; set; }

        [InputInt(DefaultValue = 50, FromValue = 1, ToValue = 9000, Interval = 1)]
        public int MAVolumePeriod { get; set; }
        [Output]
        public Indicator MAVolume { get; set; }
        #endregion
        #region bollinger bands
        BollingerBands bollingerBands = null;
        [InputInt(DefaultValue = 20, FromValue = 1, ToValue = 9000, Interval = 1)]
        public int BollingerBandsPeriod { get; set; }
        [Output]
        public Window<double> BollingerBandsUpper { get { return bollingerBands.Upper; } }
        [Output]
        public Window<double> BollingerBandsLower { get { return bollingerBands.Lower; } }
        [Output]
        public Window<double> BollingerBandsAverage { get { return bollingerBands.Average; } }
        #endregion

        #region MACDs
        [InputInt(DefaultValue = 26, FromValue = 1, ToValue = 9000, Interval = 1)]
        public int MACDSlowPeriod { get; set; }
        [InputInt(DefaultValue = 12, FromValue = 1, ToValue = 9000, Interval = 1)]
        public int MACDFastPeriod { get; set; }
        [InputInt(DefaultValue = 9, FromValue = 1, ToValue = 9000, Interval = 1)]
        public int MACDSignalPeriod { get; set; }
        MACD macd = null;
        [Output]
        public Window<double> MACD { get { return macd; } }
        [Output]
        public Window<double> MACDSignal { get { return macd.MACDSignal; } }
        [Output]
        public Window<double> MACDDivergence { get { return macd.Divergence; } }
        #endregion

        #region RSI
        [InputInt(DefaultValue = 14, FromValue = 2, ToValue = 9000, Interval = 1)]
        public int RSIPeriod { get; set; }
        [Output]
        public RelativeStrengthIndex RSI { get; set; }
        #endregion
        #region Ultimate Oscillator
        [Output]
        public UltimateOscillator UltimateOscillator { get; set; }
        [InputInt(DefaultValue = 7, FromValue = 2, ToValue = 9000, Interval = 1)]
        public int UltimateOscillatorP1 { get; set; }
        [InputInt(DefaultValue = 14, FromValue = 2, ToValue = 9000, Interval = 1)]
        public int UltimateOscillatorP2 { get; set; }
        [InputInt(DefaultValue = 28, FromValue = 2, ToValue = 9000, Interval = 1)]
        public int UltimateOscillatorP3 { get; set; }
        #endregion
        public Test1Indicator()
            : base(10000)
        {
            this.StartDate = new DateTime(2013,01,01);
        }
        public override void Start(params Window<double>[] values)
        {
            base.Start(values);
            //ExponentialMovingAverage()
            if (MAUseSMA)
            {
                MALong = SimpleMovingAverage(MALongPeriod, 2);
                MAMedian = SimpleMovingAverage(MAMedianPeriod, 2);
                MAShort = SimpleMovingAverage(MAShortPeriod, 2);
                MAVolume = SimpleMovingAverage(MAVolumePeriod, 2);
            }
            else
            {
                MALong = ExponentialMovingAverage(MALongPeriod, 2);
                MAMedian = ExponentialMovingAverage(MAMedianPeriod, 2);
                MAShort = ExponentialMovingAverage(MAShortPeriod, 2);
                MAVolume = ExponentialMovingAverage(MAVolumePeriod, 2);
            }
            bollingerBands = BollingerBands(BollingerBandsPeriod,2);
            macd = base.MACD(Math.Max(Math.Max(MACDSignalPeriod, MACDSlowPeriod), MACDFastPeriod) + 2);
            macd.PeriodLong = MACDSlowPeriod;
            macd.PeriodShort = MACDFastPeriod;
            macd.SignalPeriod = MACDSignalPeriod;
            RSI = RelativeStrengthIndex(RSIPeriod, 2);
            UltimateOscillator = base.UltimateOscillator(1);
            UltimateOscillator.Source = this.Source;
            UltimateOscillator.Period1 = UltimateOscillatorP1;
            UltimateOscillator.Period2 = UltimateOscillatorP2;
            UltimateOscillator.Period3 = UltimateOscillatorP3;
            
        }
        protected override void AfterSetValue(params Window<double>[] values)
        {
            base.AfterSetValue(values);
            MALong.Push(Close);
            MAMedian.Push(Close);
            MAShort.Push(Close);
            MAVolume.Push(Volume);
            bollingerBands.Push(Close);
            macd.Push(Close);
            RSI.Push(Close);
            UltimateOscillator.Push();
        }
    }
    public partial class IndicatorBase
    {
        public Test1Indicator Test1Indicator()
        {
            return new Test1Indicator() { Source = this.Source };
        }
        public static Test1Indicator Create_Test1Indicator(ISource source = null)
        {
            return new Test1Indicator() { Source = source };
        }
    }
}
