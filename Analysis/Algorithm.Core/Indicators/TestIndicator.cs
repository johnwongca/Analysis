using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Algorithm.Core
{
    [Indicator, Description(@"This is the Test1 indicator")]
    public class Test1Indicator : Indicator
    {
        [InputInt(DefaultValue=100, FromValue=100, ToValue = 200, Interval = 1)]
        public int SMALongPeriod { get; set; }

        [Output]
        public SimpleMovingAverage SMALong { get; set; }
        public Test1Indicator()
            : base(200)
        {
            this.StartDate = new DateTime(2013,01,01);
        }
        public override void Start(params Window<double>[] values)
        {
            base.Start(values);
            SMALong = SimpleMovingAverage(SMALongPeriod, 2);
        }
        protected override void AfterSetValue(params Window<double>[] values)
        {
            base.AfterSetValue(values);
            SMALong.Push(Close);
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
