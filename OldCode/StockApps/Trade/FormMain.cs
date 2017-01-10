using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Trade.Base;

namespace Trade
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
            tmFreeResource.Enabled = true;
		}
        public Form CreateForm(Type formType)
        {
            Form f = this.GetForm(formType);
            if (f != null)
            {
                f.Activate();
                return f;
            }
            f = (Form)Activator.CreateInstance(formType);
            f.Show();
            return f;
        }
        public Form GetForm(Type formType)
        {
            foreach (Form f in MdiChildren)
            {
                if (f.GetType() == formType)
                    return f;
            }
            return null;
        }
		private void miWindowsWindowsItems_Click(object sender, EventArgs e)
		{
			((BaseForm)((ToolStripMenuItem)sender).Tag).Activate();
		}
		private void miExit_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void miWindows_DropDownOpening(object sender, EventArgs e)
		{
			miWindowsCascade.Enabled = MdiChildren.Length != 0;
			miWindowsHorizontal.Enabled = miWindowsCascade.Enabled;
			miWindowsVertical.Enabled = miWindowsCascade.Enabled;
			miWindowsWidnows.Enabled = miWindowsCascade.Enabled;
			miWindowsWidnows.DropDownItems.Clear();
			miWindowsWidnows.DropDownItems.Add(miWindowsCloseAll);
			miWindowsWidnows.DropDownItems.Add(miWindowSeparatorCloseAll);
		}
		private void miWindowShowTabs_Click(object sender, EventArgs e)
		{
			penalWindowTabs.Visible = miWindowsShowTabs.Checked && this.MdiChildren.Length > 0;
			miWindowsShowMultiLineTabs.Visible = miWindowsShowTabs.Checked;
			miWindowsShowMultiLineTabs_Click(null, null);
		}
		private void miWindowsWidnows_DropDownOpening(object sender, EventArgs e)
		{
            List<Form> forms = new List<Form>(this.MdiChildren);
            forms.Sort((Form x, Form y) => { return x.Text.CompareTo(y.Text);});
            foreach (Form i in forms)
			{
				if (i is BaseForm)
				{
					ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem(i.Text);
					item.Tag = i;
					item.Checked = this.ActiveMdiChild == i;
					item.Click += new System.EventHandler(miWindowsWindowsItems_Click);
					miWindowsWidnows.DropDownItems.Add(item);
				}
			}
		}
		private void miWindowCascade_Click(object sender, EventArgs e)
		{
			this.LayoutMdi(MdiLayout.Cascade);
		}
		private void miWindowHorizontal_Click(object sender, EventArgs e)
		{
			this.LayoutMdi(MdiLayout.TileHorizontal);
		}
		private void miWindowVertical_Click(object sender, EventArgs e)
		{
			this.LayoutMdi(MdiLayout.TileVertical);
		}
		private void miWindowCloseAll_Click(object sender, EventArgs e)
		{
			while (this.MdiChildren.Length > 0)
				this.MdiChildren[0].Close();
			miWindowShowTabs_Click(null, null);
		}
		public void RegisterChildWindow(BaseForm obj)
		{
			obj.MdiParent = this;
			miWindowShowTabs_Click(null, null);
			TabPage p = new TabPage(obj.Text);
			p.Tag = obj;
			tabChildFormControl.TabPages.Insert(0, p);
			tabChildFormControl.SelectedTab = p;
			miWindowShowTabs_Click(null, null);
		}
		public void UnregisterChildWindow(BaseForm obj)
		{
			foreach (TabPage p in tabChildFormControl.TabPages)
			{
				if (p.Tag == obj)
				{
					tabChildFormControl.TabPages.Remove(p);
					break;
				}
			}
			miWindowShowTabs_Click(null, null);
		}
		public void ActivateChild(BaseForm obj)
		{
			if (tabChildFormControl.TabCount == 0)
				return;
			if (tabChildFormControl.SelectedTab == null)
				return;
			if (tabChildFormControl.SelectedTab.Tag == obj)
				return;
			foreach (TabPage p in tabChildFormControl.TabPages)
			{
				if (p.Tag == obj)
				{
					tabChildFormControl.SelectedTab = p;
					break;
				}
			}
		}
		private void tabChildFormControl_Selected(object sender, TabControlEventArgs e)
		{
			if (tabChildFormControl.TabCount == 0)
				return;
			if (tabChildFormControl.SelectedTab == null)
				return;
			((BaseForm)(tabChildFormControl.SelectedTab.Tag)).Activate();
		}
		private void miTabClose_Click(object sender, EventArgs e)
		{
			if (tabChildFormControl.TabCount == 0)
				return;
			if (tabChildFormControl.SelectedTab == null)
				return;
			((BaseForm)tabChildFormControl.SelectedTab.Tag).Close();
		}
		private void FormMain_Load(object sender, EventArgs e)
		{
			tabChildFormControl.TabPages.Clear();
		}
		private void cmTabChildFormControl_Opening(object sender, CancelEventArgs e)
		{
			if (tabChildFormControl.TabCount == 0)
			{
				e.Cancel = true;
				return;
			}
			if (tabChildFormControl.SelectedTab == null)
			{
				e.Cancel = true;
				return;
			}
		}
		private void miWindowsShowMultiLineTabs_Click(object sender, EventArgs e)
		{
			tabChildFormControl.Multiline = miWindowsShowMultiLineTabs.Checked;
			Rectangle r = tabChildFormControl.DisplayRectangle;
			if (penalWindowTabs.Height != r.Y)
				penalWindowTabs.Height = r.Y;
		}
		private void FormMain_Resize(object sender, EventArgs e)
		{
			if (tabChildFormControl.Multiline)
				miWindowsShowMultiLineTabs_Click(null, null);
		}
		private TabPage TabOrder_GetTabPageByTab(Point pt)
		{
			TabPage p = null;
			for (int i = 0; i < tabChildFormControl.TabCount; i++)
			{
				if (tabChildFormControl.GetTabRect(i).Contains(pt))
				{
					p = tabChildFormControl.TabPages[i];
					break;
				}
			}
			return p;
		}
		private int TabOrder_FindIndex(TabPage page)
		{
			for (int i = 0; i < tabChildFormControl.TabCount; i++)
			{
				if (tabChildFormControl.TabPages[i] == page)
					return i;
			}
			return -1;
		}
		private void miTest1_Click(object sender, EventArgs e)
		{
			BaseForm bf = new BaseForm();
			bf.Text = (new Random()).Next().ToString();
			bf.Show();
		}
		private void tabChildFormControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				return;
			Point pt = new Point(e.X, e.Y);
			TabPage tp = TabOrder_GetTabPageByTab(pt);
			if (tp != null)
			{
				DoDragDrop(tp, DragDropEffects.All);
			}
		}
		private void tabChildFormControl_DragOver(object sender, DragEventArgs e)
		{
			Point pt = new Point(e.X, e.Y);
			pt = tabChildFormControl.PointToClient(pt);
			TabPage hover_tab = TabOrder_GetTabPageByTab(pt);
			if (hover_tab != null)
			{
				if (e.Data.GetDataPresent(typeof(TabPage)))
				{
					e.Effect = DragDropEffects.Move;
					TabPage drag_tab = (TabPage)e.Data.GetData(typeof(TabPage));
					int item_drag_index = TabOrder_FindIndex(drag_tab);
					int drop_location_index = TabOrder_FindIndex(hover_tab);
					if (item_drag_index != drop_location_index)
					{
						ArrayList pages = new ArrayList();
						for (int i = 0; i < tabChildFormControl.TabCount; i++)
						{
							if (i != item_drag_index)
								pages.Add(tabChildFormControl.TabPages[i]);
						}
						pages.Insert(drop_location_index, drag_tab);
						tabChildFormControl.TabPages.Clear();
						tabChildFormControl.TabPages.AddRange((TabPage[])pages.ToArray(typeof(TabPage)));
						tabChildFormControl.SelectedTab = drag_tab;
						((BaseForm)drag_tab.Tag).Activate();
					}
				}
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}
        private void miTestForm_Click(object sender, EventArgs e)
        {
            CreateForm(typeof(FormTest));
        }
        private void miTabSort_Click(object sender, EventArgs e)
        {
            List<TabPage> pages = new List<TabPage>();
            foreach (TabPage p in tabChildFormControl.TabPages)
                pages.Add(p);
            pages.Sort((TabPage x, TabPage y) => { return x.Text.CompareTo(y.Text); });
            tabChildFormControl.TabPages.Clear();
            tabChildFormControl.TabPages.AddRange(pages.ToArray());
            tabChildFormControl_Selected(null, null);
        }

        private void miImportProvider_Click(object sender, EventArgs e)
        {
            CreateForm(typeof(Interface.Import.FormProvider));
        }

        private void miUnprocessedFiles_Click(object sender, EventArgs e)
        {
            CreateForm(typeof(Interface.Import.FormUnprocessedFiles));
        }

        private void miArchivedFiles_Click(object sender, EventArgs e)
        {
            CreateForm(typeof(Interface.Import.FormArchivedFiles));
        }

        private void miAccount_Click(object sender, EventArgs e)
        {
            CreateForm(typeof(Interface.Transaction.FormAccount));
        }

        private void miSymbol_Click(object sender, EventArgs e)
        {
            CreateForm(typeof(Interface.Transaction.FormSymbol));
        }

        private void miTransaction_Click(object sender, EventArgs e)
        {
            CreateForm(typeof(Interface.Transaction.FormTransaction));
        }

        private void miExprorer_Click(object sender, EventArgs e)
        {
            CreateForm(typeof(Interface.Analyze.FormExplorer));
        }

        private void testAPICursorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new Trade.Interface.Analyze.FormCharts()).Show();
        }

        private void miAnalyzeTest_Click(object sender, EventArgs e)
        {
            CreateForm(typeof(Interface.Analyze.FormTest));
        }

        private void bwFreeResource_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = GC.MaxGeneration; i >= 0; i--)
                GC.Collect(i, GCCollectionMode.Forced);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            for (int i = GC.MaxGeneration; i >= 0; i--)
                GC.Collect(i, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }

        }
        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);
        private void tmFreeResource_Tick(object sender, EventArgs e)
        {
            tmFreeResource.Enabled = false;
            bwFreeResource.RunWorkerAsync();
        }

        private void bwFreeResource_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tmFreeResource.Enabled = true;
        }	
	}

}