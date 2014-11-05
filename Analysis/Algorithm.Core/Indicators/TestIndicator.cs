using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Algorithm.Core
{
    [Indicator, Description(@"This is the Test1 indicator")]
    public class Test1Indicator : Indicator
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
        #region
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
        }
        protected override void AfterSetValue(params Window<double>[] values)
        {
            base.AfterSetValue(values);
            MALong.Push(Close);
            MAMedian.Push(Close);
            MAShort.Push(Close);
            MAVolume.Push(Volume);
            bollingerBands.Push(Close);
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
