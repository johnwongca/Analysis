using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Console
{
	public partial class BaseForm : Form
	{
		public BaseForm()
		{
			InitializeComponent();
		}

		private void BaseForm_Load(object sender, EventArgs e)
		{
			if (this.DesignMode)
				return;
			Global.mainForm.RegisterChildWindow(this);
		}
		private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DesignMode)
				return;
			Global.mainForm.UnregisterChildWindow(this);
			GC.ReRegisterForFinalize(this);
            GC.Collect();
		}

		private void BaseForm_Activated(object sender, EventArgs e)
		{
			if (this.DesignMode)
				return;
			Global.mainForm.ActivateChild(this);
		}

	}
}