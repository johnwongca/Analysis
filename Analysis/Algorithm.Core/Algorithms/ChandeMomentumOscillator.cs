using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Algorithm.Core
{
    [Description(@"Chande Momentum Oscillator, A technical momentum indicator invented by the technical analyst Tushar Chande. It is created by calculating the difference between the sum of all recent gains and the sum of all recent losses and then dividing the result by the sum of all price movement over the period. This oscillator is similar to other momentum indicators such as the Relative Strength Index and the Stochastic Oscillator because it is range bounded (+100 and -100).
Interpretation: The security is deemed to be overbought when the momentum oscillator is above +50 and oversold when it is below -50. Many technical traders add a nine-period moving average to this oscillator to act as a signal line. Bullish signals are generated when the oscillator crosses above the signal, and bearish signals are generated when the oscillator crosses down through the signal. 
Calculation: (Sum of Gain - Sum of Loss) * 100 /(Sum of Gain + Sum of Loss)
")]
    public class ChandeMomentumOscillator:Indicator
    {
        Sum sGain, sLoss;
        int mPeriod;
        public int Period { get { return mPeriod; } }
        public ChandeMomentumOscillator(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            mPeriod = period;
            sGain = Sum(period, 1);
            sGain.InputCacheSize = period + 1;
            sLoss = Sum(period, 1);
            sLoss.InputCacheSize = period + 1;
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            sGain.Push(values[0].Gain());
            sLoss.Push(values[0].Loss());
            double denominator = sGain.Value+sLoss.Value;
            Value = denominator < double.Epsilon ? 0d : 100d * (sGain.Value - sLoss.Value) / denominator;
        }
    }
    public partial class IndicatorBase
    {
        public ChandeMomentumOscillator ChandeMomentumOscillator(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new ChandeMomentumOscillator(period, size) { Source = this.Source };
        }
        public static ChandeMomentumOscillator Create_ChandeMomentumOscillator(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new ChandeMomentumOscillator(period, size) { Source = source };
        }
    }
}
