using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Trade.Base
{
	public partial class BaseForm : Form
	{
        protected bool isFilling;
		public BaseForm()
		{
            isFilling = false;
            m_AutoOpen = true;
            m_BindingNavigatorVisible = true;
			InitializeComponent();
            
		}
		private void BaseForm_Load(object sender, EventArgs e)
        {
			if (this.DesignMode)
				return;
            (this.GetMainForm()).RegisterChildWindow(this);
            this.WindowState = FormWindowState.Maximized;
            InitializeControls();
            SetAllBindingSourceAdapterAdapter();
            ReOpen();
            FormMain m = this.GetMainForm();
            m.btnRefresh.Click -= this.DelegateRefreshClick;
            m.btnSave.Click -= this.DelegateSaveClick;
            m.bnMain.ItemClicked -= this.MainNavigatorItemClick;
            m.btnRefresh.Click += this.DelegateRefreshClick;
            m.btnSave.Click += this.DelegateSaveClick;
            m.bnMain.ItemClicked += this.MainNavigatorItemClick;
            BindingNavigatorVisible = BindingNavigatorVisible;
		}
		private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DesignMode)
				return;
            FormMain m = this.GetMainForm();
            m.UnregisterChildWindow(this);
            m.btnRefresh.Click -= this.DelegateRefreshClick;
            m.btnSave.Click -= this.DelegateSaveClick;
            m.bnMain.ItemClicked -= this.MainNavigatorItemClick;
			GC.ReRegisterForFinalize(this);
            GC.Collect();
		}

		private void BaseForm_Activated(object sender, EventArgs e)
		{
			if (this.DesignMode)
				return;
            FormMain m = this.GetMainForm();
            m.ActivateChild(this);
            BindingNavigatorVisible = BindingNavigatorVisible;
		}
        private void BaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormMain m = this.GetMainForm();
            m.btnRefresh.Click -= this.DelegateRefreshClick;
            m.btnSave.Click -= this.DelegateSaveClick;
            m.bnMain.ItemClicked -= this.MainNavigatorItemClick;
            m.bnMain.BindingSource = null;
            m.btnSave.Enabled = false;
            m.btnRefresh.Enabled = false;
        }
        protected void SetAllBindingSourceAdapterAdapter()
        {
            var bs = this.BindingSourceAdapters.Where(x => { return x.Adapter == null; });
            var adapters = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                .Where(x => { return x.MemberType == MemberTypes.Field; })
                                .Select(x => { return this.GetType().InvokeMember(x.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, this, null);})
                                .Where(x => {
                                                if (x is Component)
                                                    return ((Component)x).IsAdapter();
                                                return false;
                                            }
                                        )
                                .Select(x => { return new { adapter = (Component)x, ReturnType = ((Component)x).GetAdapterReturnType() }; }).Distinct().ToList();
            foreach (var b in bs)
            {
                if (b.Adapter != null)
                    continue;
                var a = adapters.FirstOrDefault(x => { return x.ReturnType == b.DataTable.GetType(); });
                if(a != null)
                    b.Adapter = a.adapter;
            }
            
        }
        protected BindingSourceAdapter GetParentBindingSource(BindingSourceAdapter bs, List<BindingSourceAdapter> all)
        {
            if (bs == null)
                return null;
            DataRelation dr = bs.DataTable.DataSet.Relations.Cast<DataRelation>().FirstOrDefault(x => { return x.ChildTable == bs.DataTable && x.ParentTable != null; });
            if (dr == default(DataRelation))
                return null;
            if (dr.ParentTable == null)
                return null;
            if (all == null)
                all = BindingSourceAdapters;
            return all.FirstOrDefault(x => { return x.DataTable == dr.ParentTable; });
        }
        public void ReOpen()
        {
            if (!AutoOpen) return;
            try
            {
                isFilling = true;
                List<BindingSourceAdapter> all = BindingSourceAdapters;
                List<BindingSourceAdapter> pending = new List<BindingSourceAdapter>(all);
                List<BindingSourceAdapter> opened = new List<BindingSourceAdapter>();
                foreach (var b in all)
                    ReOpen(b, all, pending, opened);
            }
            finally
            {
                isFilling = false;
            }
        }
        public void ReOpen(BindingSourceAdapter bs)
        {
            if (!AutoOpen) return;
            if (bs == null) return;
            try
            {
                isFilling = true;
                BindingSourceAdapter parent = GetParentBindingSource(bs, BindingSourceAdapters);
                if (parent != null)
                {
                    DataRelation dr = null; List<object> parameters = new List<object>();
                    dr = bs.DataTable.DataSet.Relations.Cast<DataRelation>().FirstOrDefault(x => { return x.ChildTable == bs.DataTable && x.ParentTable == parent.DataTable; });
                    if (parent.Current == null)
                        parent.MoveFirst();
                    parameters.Add(bs.DataTable);
                    foreach (var c in dr.ParentColumns)
                    {
                        object o = null;
                        try { o= Activator.CreateInstance(c.DataType); }
                        catch { };
                        if (parent.Current != null)
                            o = ((DataRowView)parent.Current)[c.ColumnName];
                        if (c.DataType == typeof(System.String) && o is DBNull)
                            o = "";
                        parameters.Add(o);
                    }
                    var cc = dr.ChildColumns.ToList();
                    for (int i = 0; i < cc.Count; i++)
                        dr.ChildTable.Columns[cc[i].ColumnName].DefaultValue = parameters[i + 1];
                    bs.Fill(parameters.ToArray());
                    return;
                }
                bs.Fill(new object[] { bs.DataTable });
            }
            finally
            {
                isFilling = false;
            }
        }
        protected void ReOpen(BindingSourceAdapter bs, List<BindingSourceAdapter> all, List<BindingSourceAdapter> pending, List<BindingSourceAdapter> opened)
        {
            if (!AutoOpen) return;
            if (pending.Count == 0)
                return;
            if (bs == null)
            {
                var bss = all.Where(x => { return GetParentBindingSource(x, all) == null; });
                foreach (var b in bss)
                    ReOpen(b, all, pending, opened);
                return;
            }
            BindingSourceAdapter parent = GetParentBindingSource(bs, all);
            if (parent != null)
            {
                if(opened.IndexOf(parent) < 0)
                    ReOpen(parent, all, pending, opened);
                DataRelation dr = null; List<object> parameters = new List<object>();
                dr = bs.DataTable.DataSet.Relations.Cast<DataRelation>().FirstOrDefault(x => { return x.ChildTable == bs.DataTable && x.ParentTable == parent.DataTable; });
                if (parent.Current == null)
                    parent.MoveFirst();
                parameters.Add(bs.DataTable);
                foreach (var c in dr.ParentColumns)
                {
                    object o = null;
                    try { o = Activator.CreateInstance(c.DataType); }
                    catch { };
                    if (parent.Current != null)
                        o = ((DataRowView)parent.Current)[c.ColumnName];
                    if (c.DataType == typeof(System.String) && o is DBNull)
                        o = "";
                    parameters.Add(o);
                }
                var cc = dr.ChildColumns.ToList();
                for (int i = 0; i < cc.Count; i++)
                    dr.ChildTable.Columns[cc[i].ColumnName].DefaultValue = parameters[i+1];
                try
                {
                    bs.Fill(parameters.ToArray());
                }
                finally
                {
                    pending.Remove(bs);
                    opened.Add(bs);
                }
                return;
            }
            try
            {
                bs.Fill(new object[]{bs.DataTable});
            }
            finally
            {
                pending.Remove(bs);
                opened.Add(bs);
            }
            return;
        }
        
        [Browsable(false)]
        public BindingNavigator Navigator
        {
            get { return this.GetMainForm().bnMain; }
        }
        private bool m_BindingNavigatorVisible;
        [Browsable(true), Category("Navigator"), DefaultValue(true)]
        public bool BindingNavigatorVisible
        {
            get { return m_BindingNavigatorVisible; }
            set 
            { 
                m_BindingNavigatorVisible = value;
                if(this.GetMainForm() != null)
                    if (this.GetMainForm().ActiveMdiChild == this)
                        this.GetMainForm().bnMain.Visible = m_BindingNavigatorVisible;
            }
        }

        private bool m_AutoOpen;
        [Browsable(true), Category("Navigator"), DefaultValue(true)]
        public bool AutoOpen
        {
            get { return m_AutoOpen; }
            set
            {
                m_AutoOpen = value;
            }
        }

        [Browsable(true), Category("Navigator")]
        public event ToolStripItemClickedEventHandler NavigatorItemClicked;
        private void bnBase_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            OnNavigatorItemClicked(sender, e);
        }
        protected void OnNavigatorItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (NavigatorItemClicked != null)
                NavigatorItemClicked(sender, e);
        }
        private ToolStripItemClickedEventHandler MainNavigatorItemClick;

        private Control m_CurrentControl;
        private List<Control> m_SupportedControls;
        [Browsable(false)]
        public Control CurrentControl
        {
            get { return m_CurrentControl; }
            set { m_CurrentControl = value; }
        }
        [Browsable(false)]
        public List<Control> SupportedControls
        {
            get { return m_SupportedControls; }
        }
        [Browsable(false)]
        public List<BindingSourceAdapter> BindingSourceAdapters
        {
            get {return this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(x => { return x.MemberType == MemberTypes.Field; })
                .Select(x => { return this.GetType().InvokeMember(x.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, this, null); })
                .Where(x => 
                            {
                                if (x is BindingSourceAdapter)
                                    if (((BindingSourceAdapter)x).DataTable != null)
                                        if (((BindingSourceAdapter)x).DataTable.DataSet != null)
                                        {
                                            if (!((BindingSourceAdapter)x).InternalPositionChangedEventAttached)
                                            {
                                                ((BindingSourceAdapter)x).InternalPositionChanged += DelegateBindingSourceAdapterPositionChanged;
                                                ((BindingSourceAdapter)x).InternalPositionChangedEventAttached = true;
                                            }
                                            return true;
                                        }
                                return false;
                            }
                      ).Select(x => { return (BindingSourceAdapter)x; }).Distinct().ToList();}
        }
        protected void InitializeControls()
        {
            m_SupportedControls = new List<Control>();
            //this.ControlAdded += new ControlEventHandler(BaseForm_OnControlAdded);
            //this.ControlRemoved += new ControlEventHandler(BaseForm_OnControlRemoved);
            DelegateControlEnter = new EventHandler(ControlEnter);
            DelegateControlLeave = new EventHandler(ControlLeave);
            DelegateBindingSourceAdapterReadOnlyChanged = new EventHandler(BindingSourceAdapterReadOnlyChanged);
            DelegateBindingSourceAdapterPositionChanged = new EventHandler(BindingSourceAdapterInternalPositionChanged);
            MainNavigatorItemClick = new ToolStripItemClickedEventHandler(bnBase_ItemClicked);
            DelegateSaveClick = new EventHandler(btnSave_Click);
            DelegateRefreshClick = new EventHandler(btnRefresh_Click);
            var controls = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                .Where(x => { return x.MemberType == MemberTypes.Field; })
                                .Select(x => { return this.GetType().InvokeMember(x.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, this, null); })
                                .Where(x =>
                                            {
                                                return x is TextBox ||
                                                        x is CheckBox ||
                                                        x is DataGridView ||
                                                        x is DateTimePicker ||
                                                        x is ListBox ||
                                                        x is ListView ||
                                                        x is MaskedTextBox ||
                                                        x is NumericUpDown ||
                                                        x is RadioButton ||
                                                        x is ListBox||
                                                        x is DbTree;
                                            }
                                      ).Select(x => { return (Control)x; }).Distinct().ToList();
            foreach (var c in controls)
            {
                c.Enter += DelegateControlEnter;
                c.Leave += DelegateControlLeave;
            }

        }
        protected void SetNavigator(BindingSourceAdapter bs)
        {
            var bnBase = this.GetMainForm().bnMain;
            if (bnBase.BindingSource == bs)
                return;
            if (bnBase.BindingSource != null)
            {
                ((BindingSourceAdapter)bnBase.BindingSource).Update();
                ((BindingSourceAdapter)bnBase.BindingSource).ReadOnlyChanged -= DelegateBindingSourceAdapterReadOnlyChanged;
            }
            if (bs == null)
            {
                bnBase.BindingSource = null;
                BindingSourceAdapterReadOnlyChanged(bs, null);
                return;
            }
            bnBase.BindingSource = bs;
            bs.ReadOnlyChanged += DelegateBindingSourceAdapterReadOnlyChanged;
            BindingSourceAdapterReadOnlyChanged(bs, null);
            return;
        }
        private EventHandler DelegateControlEnter;
        private void ControlEnter(object sender, EventArgs e)
        {
            SetNavigator((BindingSourceAdapter)((Control)sender).GetBindingSource());
            return;
        }
        private EventHandler DelegateControlLeave;
        private void ControlLeave(object sender, EventArgs e)
        {
            return;
        }
        private EventHandler DelegateBindingSourceAdapterReadOnlyChanged;
        private void BindingSourceAdapterReadOnlyChanged(object sender, EventArgs e)
        {
            var btnRefresh = this.GetMainForm().btnRefresh;
            var btnSave = this.GetMainForm().btnSave;
            if (sender == null)
            {
                btnRefresh.Enabled = false;
                btnSave.Enabled = false;
                return;
            }
            btnRefresh.Enabled = ((BindingSourceAdapter)sender).Adapter != null;
            btnSave.Enabled = btnRefresh.Enabled && !((BindingSourceAdapter)sender).ReadOnly;
        }
        private EventHandler DelegateBindingSourceAdapterPositionChanged;
        private void BindingSourceAdapterInternalPositionChanged(object sender, EventArgs e)
        {
            if (isFilling)
                return;            
            var b = (BindingSourceAdapter)sender;
            if (b.DataTable.GetChanges() != null)
            {
                b.Update();
            }
            var d = (DataRowView)b.Current;
            var adapters = BindingSourceAdapters;
            var relations = b.DataTable.DataSet.Relations.Cast<DataRelation>().Where(x => { return x.ParentTable == b.DataTable; });
            List<Object> parameters = new List<object>();
            foreach(var r in relations)
            {
                parameters.Clear();
                foreach (var p in r.ParentColumns)
                {
                    object o = null;
                    try 
                    { 
                        o = Activator.CreateInstance(p.DataType); 
                    }
                    catch { };
                    if (d != null)
                    {
                        try { o = d[p.ColumnName]; }
                        catch { }
                    }
                    if(o == null)
                    {
                        if (p.DataType == typeof(System.String))
                            o = "";
                    }
                    else if (o is DBNull)
                    {
                        if (p.DataType == typeof(System.String))
                            o = "";
                    }
                    parameters.Add(o);
                }
                var cc = r.ChildColumns.ToList();
                for(int i = 0; i< cc.Count; i++)
                    r.ChildTable.Columns[cc[i].ColumnName].DefaultValue = parameters[i];
                ReOpen(adapters.FirstOrDefault(x => { return x.DataTable == r.ChildTable; }));
            }
            

        }

        private EventHandler DelegateSaveClick;
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.GetMainForm().ActiveMdiChild != this) return;
            var bnBase = this.GetMainForm().bnMain;
            if (bnBase.BindingSource != null)
            {
                if (bnBase.BindingSource is BindingSourceAdapter)
                {
                    ((BindingSourceAdapter)bnBase.BindingSource).Update();
                }
            }

        }
        private EventHandler DelegateRefreshClick;
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (this.GetMainForm().ActiveMdiChild != this) return;
            var bnBase = this.GetMainForm().bnMain;
            if (bnBase.BindingSource != null)
            {
                if (bnBase.BindingSource is BindingSourceAdapter)
                {
                    ((BindingSourceAdapter)bnBase.BindingSource).Update();
                    this.ReOpen((BindingSourceAdapter)bnBase.BindingSource);
                }
            }
        }

        

        
	}
}