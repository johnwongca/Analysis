using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Stock
{
	static class Global
	{
		public static FormMain mainForm = null;
		public static Form CreateForm(Type formType)
		{
			Form f = Global.GetForm(formType);
			if (f != null)
			{
				f.Activate();
				return f;
			}
			f = (Form)Activator.CreateInstance(formType);
			f.Show();
			return f;
		}
		public static Form GetForm(Type formType)
		{
			if (Global.mainForm == null)
				return null;
			foreach (Form f in Global.mainForm.MdiChildren)
			{
				if (f.GetType() == formType)
					return f;
			}
			return null;
		}
	}
	public class BaseFormComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return Compare((Form)x, (Form)y);
		}
		public int Compare(Form x, Form y)
		{
			return x.Text.CompareTo(y.Text);
		}
	}	
}
