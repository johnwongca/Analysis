//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Windows.Forms.Design;

//namespace Algorithm.Core.Forms
//{

//    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
//    public class ToolStripDateTimePicker : ToolStripControlHost
//    {
//        FlowLayoutPanel controlPanel;
//        DateTimePicker picker = new DateTimePicker();
//        public ToolStripDateTimePicker()
//            : base(new FlowLayoutPanel())
//        {
//            controlPanel = (FlowLayoutPanel)base.Control;
//            controlPanel.BackColor = Color.Transparent;
//            controlPanel.Controls.Add(picker);
//        }

//        public DateTimePicker DateTimePickerControl
//        {
//            get
//            {
//                return picker;
//            }
//        }

//        public DateTime Value
//        {
//            get
//            {
//                return picker.Value;
//            }
//            set
//            {
//                picker.Value = value;
//            }
//        }

//        protected override void OnSubscribeControlEvents(Control c)
//        {
//            base.OnSubscribeControlEvents(c);
//            picker.ValueChanged += new EventHandler(OnValueChanged);
//        }
//        protected override void OnUnsubscribeControlEvents(Control c)
//        {
//            base.OnUnsubscribeControlEvents(c);
//            DateTimePicker mumControl = (DateTimePicker)c;
//            picker.ValueChanged -= new EventHandler(OnValueChanged);
//        }

//        public event EventHandler ValueChanged;

//        private void OnValueChanged(object sender, EventArgs e)
//        {
//            if (ValueChanged != null)
//            {
//                ValueChanged(this, e);
//            }
//        }
//    }
//}
