using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Algorithm.Core
{
    [Description(@"Ultimate Oscillator, Developed by Larry Williams in 1976 and featured in Stocks & Commodities Magazine in 1985, the Ultimate Oscillator is a momentum oscillator designed to capture momentum across three different timeframes. The multiple timeframe objective seeks to avoid the pitfalls of other oscillators. Many momentum oscillators surge at the beginning of a strong advance and then form bearish divergence as the advance continues. This is because they are stuck with one time frame. The Ultimate Oscillator attempts to correct this fault by incorporating longer timeframes into the basic formula. Williams identified a buy signal a based on a bullish divergence and a sell signal based on a bearish divergence.
It requires 3 input
1:High, 2:Close, 3: Low
BP(Buying Pressure) = Close - Minimum(Low or Prior Close).
 
TR(True Range) = Maximum(High or Prior Close)  -  Minimum(Low or Prior Close)

Average7 = (7-period BP Sum) / (7-period TR Sum)
Average14 = (14-period BP Sum) / (14-period TR Sum)
Average28 = (28-period BP Sum) / (28-period TR Sum)

UO = 100 x [(4 x Average7)+(2 x Average14)+Average28]/(4+2+1)
more information, please check:
http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:ultimate_oscillator
http://ta.mql4.com/indicators/oscillators/ultimate")]
    public class UltimateOscillator:Indicator
    {
        int mPeriod1 = 7, mPeriod2 = 14, mPeriod3 = 28;
        Window<double> bp1, bp2, bp3, tr1, tr2, tr3;

        public int Period1 { get { return mPeriod1; } set { mPeriod1 = value; } }
        public int Period2 { get { return mPeriod2; } set { mPeriod2 = value; } }
        public int Period3 { get { return mPeriod3; } set { mPeriod3 = value; } }
        public UltimateOscillator(int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {

        }
        protected override void Initialization(params Window<double>[] values)
        {
            base.Initialization(values);
            
        }
        public override void Start(params Window<double>[] values)
        {
            bp1 = Sum(Period1, 1); bp1.InputCacheSize = Period1 + 1;
            bp2 = Sum(Period2, 1); bp2.InputCacheSize = Period2 + 1;
            bp3 = Sum(Period3, 1); bp3.InputCacheSize = Period3 + 1;
            tr1 = Sum(Period1, 1); tr1.InputCacheSize = Period1 + 1;
            tr2 = Sum(Period2, 1); tr2.InputCacheSize = Period2 + 1;
            tr3 = Sum(Period3, 1); tr3.InputCacheSize = Period3 + 1;
        }
        protected override void AfterSetValue(params Window<double>[] values)
        {
            double a1, a2, a3, previousClose = 0, bp, tr ;
            previousClose = CurrentLocation == 0 ? Close.Value : Close[1];
            tr = Math.Max(High.Value, previousClose) - Math.Min(Low.Value, previousClose);
            bp = Close.Value - Math.Min(Low.Value, previousClose);
            tr1.Push(tr); tr2.Push(tr); tr3.Push(tr);
            bp1.Push(bp); bp2.Push(bp); bp3.Push(bp);
            a1 = tr1 == 0 ? 0 : bp1 / tr1;
            a2 = tr2 == 0 ? 0 : bp2 / tr2;
            a3 = tr3 == 0 ? 0 : bp3 / tr3;
            Value = 100d * ((4d * a1) + (2d * a2) + a3) / 7d;
            //Value = bp1.Value;
        }
    }
    public partial class IndicatorBase
    {
        public UltimateOscillator UltimateOscillator(int size = Window<double>.DefaultDataWindowSize)
        {
            return new UltimateOscillator(size) { Source = this.Source };
        }
        public static UltimateOscillator Create_UltimateOscillator(int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new UltimateOscillator(size) { Source = source };
        }
    }
}
