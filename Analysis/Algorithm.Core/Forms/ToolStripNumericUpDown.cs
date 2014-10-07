using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Algorithm.Core.Forms
{
    // code copied from http://www.codeproject.com/Messages/2528435/NumericUpDown-control-in-ToolStrip.aspx
   [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripNumericUpDown : ToolStripControlHost
    {
        // Call the base constructor passing in a NumericUpDown instance.
       public ToolStripNumericUpDown()
           : base(new NumericUpDown())
        {
        }

        ///

        /// Gets the numeric up down control.
        ///

        /// The numeric up down control.
        public NumericUpDown NumericUpDownControl
        {
            get
            {
                return Control as NumericUpDown;
            }
        }



        ///

        /// Gets or sets the value.
        ///

        /// The value.
        public decimal Value
        {
            get
            {
                return NumericUpDownControl.Value;
            }
            set
            {
                value = NumericUpDownControl.Value;
            }
        }

        ///

        /// Subscribe and unsubscribe the control events you wish to expose.
        ///

        /// The c.
        protected override void OnSubscribeControlEvents(Control c)
        {
            // Call the base so the base events are connected.
            base.OnSubscribeControlEvents(c);

            // Cast the control to a NumericUpDown control.
            NumericUpDown mumControl = (NumericUpDown)c;

            // Add the event.
            mumControl.ValueChanged += new EventHandler(OnValueChanged);
        }

        ///

        /// Subscribe and unsubscribe the control events you wish to expose.
        ///

        /// The c.
        protected override void OnUnsubscribeControlEvents(Control c)
        {
            // Call the base method so the basic events are unsubscribed.
            base.OnUnsubscribeControlEvents(c);

            // Cast the control to a NumericUpDown control.
            NumericUpDown mumControl = (NumericUpDown)c;

            // Remove the event.
            mumControl.ValueChanged -= new EventHandler(OnValueChanged);
        }

        // Declare the ValueChanged event.
        public event EventHandler ValueChanged;

        // Raise the ValueChanged event.
        private void OnValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }
        }
    }
}
