using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Algorithm.Core
{
    [Description(@"Aroon indicator helps you to anticipate a change in security prices from trending to a trading rage and vise versa by measuring the number of periods that have passed since prices reached recent high and low values. When you plot this indicator, you should have 2, Aroon Up and Aroon Down. It's developed by Tushar Chande.
Calculating Aroon Up need a window on High where calculating Aroon Down need a widow on Low. Since Aroon need n+1 period. The window should be n + 1. For instance, calculating 20 day Aroon, you need a 21 day window.
IsCalculateAroonUp=""true"" or ""false""

Inputs: 

Interpretation

Extremes: When Aroon UP line reaches 100, strength is indicated. If the Aroon Up remains persistantly betwen 70 and 100, an uptrend is indicated. Likewise, if the Aroon Down line reaches 0, potential weakness is indicated. If the Aroon Down remains persistently between 0 and 30, a downtrend is indicated. If both Up and Down remain at extreme levels, rather than just one of the plots being at an extreme level, a stronger trend is indicated. Thus a strong uptrend is indicated when the Aroon Up line persistently remain between 70 and 100 while the Aroon Down line persistently remains between 0 and 30. Conversely, a strong downtrend is indicated when the Aroon Down line persistently remains between 70 and 100 while the Aroon Up line persistently remains between 0 and 30.

Parallel Movement: Consideration is indicated when the Aroon Up and Aroon Down lines move parallel with each other and are roughly at the same level. Expect further consideration until a directional move is indicated by an extreme level or a crossover.

Crossovers. When the Aroon Down line crosses above the Aroon Up line, potential weakness is indicated. Expect price to begin trending lower. Thwn the Aroon Up line crosses above the Aroon Down line, potential strength is indicated. Expect prices to begin trending higher.

Calculation:

Aroon Up = (Period - (period since highest high in ""n + 1"" periods) )/period)* 100
Aroon Down = (Period - (period since lowest low in ""n + 1"" periods) )/period)* 100
")]
    public class Aroon : Indicator
    {
        protected int mPeriod;
        Window<double> mDown;
        public int Period { get { return mPeriod; } }
        public Window<double> Up { get { return this; } }
        public Window<double> Down { get { return mDown; } }
        public Aroon(int period, int size = Window<double>.DefaultDataWindowSize)
            : base(size)
        {
            mPeriod = period;
            mDown = new Window<double>(this.Size);
        }

        protected override void AfterSetValue(params Window<double>[] values)
        {
            if(CurrentLocation< Period)
            {
                Value = 0d;
                Down.Push(0);
                return;
            }
            int locationUp = 0, locationDown = 0;
            for (int i = 0; i < Period; i++)
            {

                if (High[locationUp] < High[i]) //up
                    locationUp = i;
                if (Low[locationDown] > Low[i]) // down
                    locationDown = i;
            }
            Value = (Period - locationUp).ToDouble() * 100d / Period.ToDouble();
            Down.Push((Period - locationDown).ToDouble() * 100d / Period.ToDouble());
        }
    }
    public partial class IndicatorBase
    {
        public Aroon Aroon(int period, int size = Window<double>.DefaultDataWindowSize)
        {
            return new Aroon(period, size) { Source = this.Source };
        }
        public static Aroon Create_Aroon(int period, int size = Window<double>.DefaultDataWindowSize, ISource source = null)
        {
            return new Aroon(period, size) { Source = source };
        }
    }
}
