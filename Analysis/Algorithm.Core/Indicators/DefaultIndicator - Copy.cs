using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Algorithm.Core
{
    [Indicator(), Description(@"This is the Test1 indicator")]
    public class Test1Indicator : Indicator
    {
        public Test1Indicator()
            : base(1)
        {
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
