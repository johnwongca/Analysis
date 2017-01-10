using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;


namespace Trade.Base
{
    public class BindingSourceAdapter : System.Windows.Forms.BindingSource
    {
        public BindingSourceAdapter()
            : base()
        {
            InitializeComonent();
        }
        public BindingSourceAdapter(IContainer container)
            : base(container)
        {
            InitializeComonent();
        }
        public BindingSourceAdapter(Object dataSource, string dataMember)
            : base(dataSource, dataMember)
        {
            InitializeDelegates();
        }
        private void InitializeComonent()
        {
            m_InternalPositionChangedEventAttached = false;
            InitializeDelegates();
            SetDataSource();
        }
        DataTable m_DataTable = null;
        Component m_Adapter;
        protected void SetDataSource()
        {
            base.DataMember = "";
            base.DataSource = DataTable;
            return;
        }
        
        protected bool IsAdapter(Component adapter)
        {
            return adapter.IsAdapter();
        }

        protected void DoDataTableChanged(DataTable oldDataTable, DataTable newDataTable)
        {
            BindingSourceAdapterDataTableChanged e = new BindingSourceAdapterDataTableChanged(oldDataTable, newDataTable);
            if (DataTableBeforeChange != null)
            {
                DataTableBeforeChange(this, e);
                if (e.CancelChange)
                    return;
            }
            m_DataTable = newDataTable;
            TableEventRemove(oldDataTable);
            TableEventAdd(newDataTable);
            SetDataSource();
            if (DataTableAfterChange != null)
            {
                e.CancelChange = false;
                DataTableAfterChange(this, e);
                if (e.CancelChange)
                {
                    m_DataTable = oldDataTable;
                    TableEventRemove(newDataTable);
                    TableEventAdd(oldDataTable);
                    SetDataSource();
                }
            }

        }
        protected void TableEventAdd(DataTable dataTable)
        {
            if (dataTable == null) return;
            dataTable.ColumnChanged += DelegateColumnChanged;
            dataTable.ColumnChanging += DelegateColumnChanging;
            dataTable.Initialized += DelegateInitialized;
            dataTable.RowChanged += DelegateRowChanged;
            dataTable.RowChanging += DelegateRowChanging;
            dataTable.RowDeleted += DelegateRowDeleted;
            dataTable.RowDeleting += DelegateRowDeleting;
            dataTable.TableCleared += DelegateTableCleared;
            dataTable.TableClearing += DelegateTableClearing;
            dataTable.TableNewRow += DelegateTableNewRow;
        }
        protected void TableEventRemove(DataTable dataTable)
        {
            if (dataTable == null) return;
            dataTable.ColumnChanged -= DelegateColumnChanged;
            dataTable.ColumnChanging -= DelegateColumnChanging;
            dataTable.Initialized -= DelegateInitialized;
            dataTable.RowChanged -= DelegateRowChanged;
            dataTable.RowChanging -= DelegateRowChanging;
            dataTable.RowDeleted -= DelegateRowDeleted;
            dataTable.RowDeleting -= DelegateRowDeleting;
            dataTable.TableCleared -= DelegateTableCleared;
            dataTable.TableClearing -= DelegateTableClearing;
            dataTable.TableNewRow -= DelegateTableNewRow;
        }
        protected void DoColumnChange(Object sender, DataColumnChangeEventArgs e)
        {
            if (ColumnChanged != null)
                ColumnChanged(sender, e);
        }
        protected void DoColumnChanging(Object sender, DataColumnChangeEventArgs e)
        {
            if (ColumnChanging != null)
                ColumnChanging(sender, e);
        }
        protected void DoInitialized(Object sender, EventArgs e)
        {
            if (Initialized != null)
                Initialized(sender, e);
        }
        protected void DoRowChanged(Object sender, DataRowChangeEventArgs e)
        {
            if (RowChanged != null)
                RowChanged(sender, e);
        }
        protected void DoRowChanging(Object sender, DataRowChangeEventArgs e)
        {
            if (RowChanging != null)
                RowChanging(sender, e);
        }
        protected void DoRowDeleted(Object sender, DataRowChangeEventArgs e)
        {
            if (RowDeleted != null)
                RowDeleted(sender, e);
        }
        protected void DoRowRowDeleting(Object sender, DataRowChangeEventArgs e)
        {
            if (RowDeleting != null)
                RowDeleting(sender, e);
        }
        protected void DoTableCleared(Object sender, DataTableClearEventArgs e)
        {
            if (TableCleared != null)
                TableCleared(sender, e);
        }
        protected void DoTableClearing(Object sender, DataTableClearEventArgs e)
        {
            if (TableClearing != null)
                TableClearing(sender, e);
        }
        protected void DoTableNewRow(Object sender, DataTableNewRowEventArgs e)
        {
            if (TableNewRow != null)
                TableNewRow(sender, e);
        }

        private DataColumnChangeEventHandler DelegateColumnChanged;
        private DataColumnChangeEventHandler DelegateColumnChanging;
        private EventHandler DelegateInitialized;
        private DataRowChangeEventHandler DelegateRowChanged;
        private DataRowChangeEventHandler DelegateRowChanging;
        private DataRowChangeEventHandler DelegateRowDeleted;
        private DataRowChangeEventHandler DelegateRowDeleting;
        private DataTableClearEventHandler DelegateTableCleared;
        private DataTableClearEventHandler DelegateTableClearing;
        private DataTableNewRowEventHandler DelegateTableNewRow;
        private void InitializeDelegates()
        {
            DelegateColumnChanged = new DataColumnChangeEventHandler(DoColumnChange);
            DelegateColumnChanging = new DataColumnChangeEventHandler(DoColumnChanging);
            DelegateInitialized = new EventHandler(DoInitialized);
            DelegateRowChanged = new DataRowChangeEventHandler(DoRowChanged);
            DelegateRowChanging = new DataRowChangeEventHandler(DoRowChanging);
            DelegateRowDeleted = new DataRowChangeEventHandler(DoRowDeleted);
            DelegateRowDeleting = new DataRowChangeEventHandler(DoRowRowDeleting);
            DelegateTableCleared = new DataTableClearEventHandler(DoTableCleared);
            DelegateTableClearing = new DataTableClearEventHandler(DoTableClearing);
            DelegateTableNewRow = new DataTableNewRowEventHandler(DoTableNewRow);
        }

        protected int CallAdapterFill(Object[] parameters)
        {
            if (Adapter == null)
                return -1;
            return (int)Adapter.GetType().InvokeMember("Fill", BindingFlags.InvokeMethod, null, Adapter, parameters);
        }
        protected DataTable CallAdapterGetData(Object[] parameters)
        {
            if (Adapter == null)
                return null;
            return (DataTable)Adapter.GetType().InvokeMember("GetData", BindingFlags.InvokeMethod, null, Adapter, parameters);
        }
        public int Fill(object[] parameters)
        {
            int r = -1;
            if (this.DataTable == null)
                return -1;
            try
            {
                Program.mainForm.Cursor = Cursors.WaitCursor;
                Program.MainFormLabelShow("Opening " + this.DataTable.TableName + "...");
                this.DataTable.BeginLoadData();
                r = CallAdapterFill(parameters);
            }
            finally
            {
                this.DataTable.EndLoadData();
                Program.mainForm.Cursor = Cursors.Default;
                Program.MainFormLabelHide();
            }
            return r;
        }
        public DataTable GetData(object[] parameters)
        {
            return CallAdapterGetData(parameters);
        }
        public int Update()
        {
            if (ReadOnly)
                return -1;
            if (this.DataTable == null)
                return -1;
            if (Adapter == null)
                return -1;
            if (DataTable == null)
                return -1;
            this.EndEdit();
            int r = -1;
            try
            {
                try { System.Windows.Forms.Application.DoEvents(); } catch { }
                var pp = new object[] { this.DataTable };
                Program.MainFormLabelShow("Updating table " + this.DataTable.TableName + "...");
                r = (int)Adapter.GetType().InvokeMember("Update", BindingFlags.InvokeMethod, null, Adapter, pp);
            }
            catch (MissingMethodException)
            {
            }
            finally
            {
                try { System.Windows.Forms.Application.DoEvents(); } catch { }
                Program.MainFormLabelHide();
            }
            return r;
        }
        public int DeleteCurrent()
        {
            /*if (Current == null)
                return 0;
            ((DataRowView)Current).Delete();*/
            RemoveCurrent();
            return Update();
        }

        [Browsable(false), Category("Data")]
        public new string DataMember
        {
            get
            {
                SetDataSource();
                return base.DataMember;
            }
            set
            {
                //base.DataMember = value;
                SetDataSource();
            }
        }

        [Browsable(false), Category("Data")]
        public new object DataSource
        {
            get
            {
                SetDataSource();
                return base.DataSource;
            }
            set
            {
                //base.DataSource = value;
                SetDataSource();
            }
        }

        [Browsable(true), Category("Data")]
        public DataTable DataTable
        {
            get
            {
                return m_DataTable;
            }
            set
            {
                if (m_DataTable == null && value != null)
                {
                    DoDataTableChanged(m_DataTable, value);
                    return;
                }
                if (m_DataTable != null && value == null)
                {
                    DoDataTableChanged(m_DataTable, value);
                    return;
                }
                if (m_DataTable == value)
                    return;
                DoDataTableChanged(m_DataTable, value);
            }
        }
        
        [Browsable(true), Category("Data")]
        public Component Adapter
        {
            get { return m_Adapter; }
            set
            {
                if (value == null)
                {
                    m_Adapter = null;
                    return;
                }
                if (IsAdapter(value))
                {
                    m_Adapter = value;
                    return;
                }
                throw new Exception("The component does not contain GetData and Fill method.");
            }
        }

        bool m_ReadOnly;
        [Browsable(true), Category("Data")]
        public bool ReadOnly
        {
            get
            {
                return m_ReadOnly;
            }
            set
            {
                if (value != m_ReadOnly)
                {
                    m_ReadOnly = value;
                    if (ReadOnlyChanged != null)
                        ReadOnlyChanged(this, (new EventArgs()));
                }
            }
        }
        [Browsable(true), Category("DataSource")]
        public event DataTableChangedEventHandler DataTableBeforeChange;
        [Browsable(true), Category("DataSource")]
        public event DataTableChangedEventHandler DataTableAfterChange;

        [Browsable(true), Category("DataAdapter")]
        public event EventHandler ReadOnlyChanged;
        bool m_InternalPositionChangedEventAttached;
        [Browsable(false)]
        public bool InternalPositionChangedEventAttached
        {
            get { return m_InternalPositionChangedEventAttached; }
            set { m_InternalPositionChangedEventAttached = value; }
        }
        [Browsable(false)]
        public event EventHandler InternalPositionChanged;
        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            if (InternalPositionChanged != null)
                InternalPositionChanged(this, e);
            
        }

        [Browsable(true), Category("DataTable")]
        public event DataColumnChangeEventHandler ColumnChanged;
        [Browsable(true), Category("DataTable")]
        public event DataColumnChangeEventHandler ColumnChanging;
        [Browsable(true), Category("DataTable")]
        public event EventHandler Initialized;
        [Browsable(true), Category("DataTable")]
        public event DataRowChangeEventHandler RowChanged;
        [Browsable(true), Category("DataTable")]
        public event DataRowChangeEventHandler RowChanging;
        [Browsable(true), Category("DataTable")]
        public event DataRowChangeEventHandler RowDeleted;
        [Browsable(true), Category("DataTable")]
        public event DataRowChangeEventHandler RowDeleting;
        [Browsable(true), Category("DataTable")]
        public event DataTableClearEventHandler TableCleared;
        [Browsable(true), Category("DataTable")]
        public event DataTableClearEventHandler TableClearing;
        [Browsable(true), Category("DataTable")]
        public event DataTableNewRowEventHandler TableNewRow;
    }
    public class BindingSourceAdapterDataTableChanged : EventArgs
    {
        DataTable m_old, m_new;
        bool m_CancelChange;
        public DataTable OldDataTable
        {
            get { return m_old; }
            set { m_old = value; }
        }
        public DataTable NewDataTable
        {
            get { return m_new; }
            set { m_new = value; }
        }
        public bool CancelChange
        {
            get { return m_CancelChange; }
            set { m_CancelChange = value; }
        }
        public BindingSourceAdapterDataTableChanged(DataTable oldDataTable, DataTable newDataTable)
        {
            m_old = oldDataTable;
            m_new = newDataTable;
            m_CancelChange = false;
        }
    }
    public delegate void DataTableChangedEventHandler(object sender, BindingSourceAdapterDataTableChanged e);
}
