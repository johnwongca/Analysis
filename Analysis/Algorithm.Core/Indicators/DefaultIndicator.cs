using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Algorithm.Core
{
    [Indicator(), Description(@"This is the default indicator")]
    public class DefaultIndicator : Indicator
    {
        public DefaultIndicator()
            : base(1000)
        {
            
        }
    }
    public partial class IndicatorBase
    {
        public DefaultIndicator DefaultIndicator()
        {
            return new DefaultIndicator() { Source = this.Source };
        }
        public static DefaultIndicator Create_DefaultIndicator(ISource source = null)
        {
            return new DefaultIndicator() { Source = source };
        }
    }
}
