using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trade.Base
{
    public partial class DbTree : System.Windows.Forms.TreeView
    {
        List<DbTreeData> m_Data;
        bool positionChanging;
        public DbTree()
        {
            DelegateBindingSourceListChanged = new ListChangedEventHandler(BindingSourceListChanged);
            DelegateBindingSourcePositionChanged = new EventHandler(BindingSourcePositionChanged);
            m_Data = new List<DbTreeData>();
            positionChanging = false;
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
        BindingSourceAdapter m_DataSource;
        [Browsable(true), Category("Data")]
        public BindingSourceAdapter DataSource
        {
            get { return this.m_DataSource; }
            set 
            {
                if (m_DataSource != value)
                {
                    if (m_DataSource != null)
                    {
                        m_DataSource.ListChanged -= this.BindingSourceListChanged;
                        m_DataSource.PositionChanged -= DelegateBindingSourcePositionChanged;
                    }
                    m_DataSource = value;
                    m_DataSource.ListChanged += this.BindingSourceListChanged;
                    m_DataSource.PositionChanged += DelegateBindingSourcePositionChanged;
                }
                
            }
        }
        
        private EventHandler DelegateBindingSourcePositionChanged;
        private void BindingSourcePositionChanged(Object sender, EventArgs e)
        {
            if (positionChanging) return;
            try
            {
                positionChanging = true;
                BindingSourceAdapter bs = (BindingSourceAdapter)sender;
                if (bs.Current == null)
                    this.SelectedNode = null;
                else
                {
                    var n = m_Data.FirstOrDefault(x => { return x.ID == (int)((DataRowView)bs.Current)["ID"]; });
                    if (n == null)
                        this.SelectedNode = null;
                    else
                        this.SelectedNode = n.Node;
                }
            }
            finally
            {
                positionChanging = false;
            }
        }
        private ListChangedEventHandler DelegateBindingSourceListChanged;
        protected void BindingSourceListChanged(Object sender, ListChangedEventArgs e)
        {
            try
            {
                positionChanging = true;
                BindingSourceAdapter bs = (BindingSourceAdapter)sender;
                DbTreeData d = null;
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        d = new DbTreeData((DataRowView)(bs.List[e.NewIndex])) { Position = e.NewIndex };
                        m_Data.Add(d);
                        if (d.ID < 0)
                            Edit();
                        break;
                    case ListChangedType.ItemChanged:
                        d = m_Data.FirstOrDefault(x => { return x.Position == e.NewIndex; });
                        if (d == null)
                            m_Data.Add(new DbTreeData((DataRowView)(bs.List[e.NewIndex])) { Position = e.NewIndex });
                        else
                            d.Row = (DataRowView)bs.List[e.NewIndex];
                        break;
                    case ListChangedType.ItemDeleted:
                        d = m_Data.FirstOrDefault(x => { return x.Position == e.NewIndex; });
                        if (d != null)
                        {
                            m_Data.Remove(d);
                            d.Node.Remove();
                            foreach (DbTreeData d1 in m_Data.Where(x => { return x.Node.TreeView == null; }).ToList())
                                m_Data.Remove(d1);
                        }
                        break;
                    case ListChangedType.ItemMoved:
                        d = m_Data.FirstOrDefault(x => { return x.Position == e.OldIndex; });
                        if (d != null)
                            d.Position = e.NewIndex;
                        break;
                    case ListChangedType.Reset:
                        m_Data.Clear();
                        this.Nodes.Clear();
                        GC.Collect();
                        int i = -1;
                        foreach (DataRowView r in bs.List)
                        {
                            i++;
                            m_Data.Add(new DbTreeData(r) { Position = i });
                        }
                        break;
                    default:
                        break;
                }
                LoadTree();
            }
            finally
            {
                positionChanging = false;
            }
            if (this.SelectedNode == null)
                DataSource.MoveFirst();
        }
        protected void LoadTree()
        {
            LoadTree(null);
        }
        protected void LoadTree(DbTreeData dt)
        {
            TreeNodeCollection Nodes;
            List<DbTreeData> children;
            if (dt == null)
            {
                children = m_Data.Where(x => { return x.ParentID == -1; }).ToList<DbTreeData>();
                Nodes = this.Nodes;
            }
            else
            {
                children = m_Data.Where(x => { return x.ParentID == dt.ID; }).ToList<DbTreeData>();
                Nodes = dt.Node.Nodes;
            }
            foreach (DbTreeData c in children)
            {
                if (c.Node.TreeView == null)
                    Nodes.Add(c.Node);
                LoadTree(c);
            }
        }
        protected void Edit()
        {
            if (this.DataSource == null)
                return;
            BindingSourceAdapter bs = DataSource;
            if(bs.Current==null)
                return;
            DbTreeData d = m_Data.FirstOrDefault(x => { return x.ID < 0; });
            if( d == null)
                d = m_Data.FirstOrDefault(x => { return x.ID == (int)((DataRowView)(bs.Current))["ID"]; });
            if (d == null)
                return;
            if (d.ID < 0)
            {
                d.Name = "New Item";
                d.ParentID = -2;
            }
            DbTreeDialogForm f = new DbTreeDialogForm();
            f.Current = d;
            f.Data = m_Data;
            f.LoadData();
            if (f.ShowDialog() == DialogResult.OK)
            {
                d.Row.BeginEdit();
                d.Row["ParentID"] = f.Current.ParentID;
                d.Row["Name"] = f.Current.Name;
                d.Row.EndEdit();
                d.Row = d.Row;
                if (d.Node.TreeView != null)
                    d.Node.Remove();
            }
            else
            {
                d.Row.CancelEdit();
                if (d.ID < 0)
                    m_Data.Remove(d);
            }
            f.Close();
            f = null;
            GC.Collect();
            return;
        }
        private void DbTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (positionChanging) return;
            try
            {
                positionChanging = true;
                if (DataSource == null)
                    return;
                if (this.SelectedNode == null)
                    return;
                if (DataSource.Position != ((DbTreeData)this.SelectedNode.Tag).Position)
                    DataSource.Position = ((DbTreeData)this.SelectedNode.Tag).Position;
            }
            finally
            {
                positionChanging = false;
            }
        }

        private void DbTree_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Alt||e.Control||e.Handled||e.KeyCode != Keys.Delete) return;
            if (this.SelectedNode == null)
                return;
            if (this.DataSource == null)
                return;
            if (SelectedNode.Tag is DbTreeData)
            {
                if (!OnBeforeDelete().Cancel)
                {
                    DataSource.DeleteCurrent();
                    OnAfterDelete();
                }
            }
        }
        protected DbTreeBeforeDeleteEventArgs OnBeforeDelete()
        {
            DbTreeBeforeDeleteEventArgs ret = new DbTreeBeforeDeleteEventArgs();
            if (BeforeDelete != null)
                BeforeDelete(this, ret);
            return ret;
        }
        protected void OnAfterDelete()
        {
            if (AfterDelete != null)
                AfterDelete(this, new EventArgs());
        }
        [Browsable(true), Category("Behavior")]
        public event DbTreeDelete BeforeDelete;
        [Browsable(true), Category("Behavior")]
        public event EventHandler AfterDelete;

        private void DbTree_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            Edit();
            e.CancelEdit = true;
            LoadTree();
            this.SelectedNode = e.Node;
        }
    }
    public class DbTreeData
    {
        public int ID, ParentID, Position;
        string m_Name;
        public string Name
        {
            get { return m_Name;}
            set 
            {
                if (m_Name != value)
                {
                    m_Name = value;
                    if (Node != null)
                    {
                        Node.Text = m_Name;
                    }
                }
            }
        }
        public TreeNode Node;
        DataRowView m_Row;
        public DataRowView Row
        {
            get { return m_Row; }
            set
            {
                m_Row = value;
                ID = (int)(m_Row)["ID"];
                ParentID = m_Row["ParentID"] == DBNull.Value ? -1 : (int)m_Row["ParentID"];
                Name = m_Row["Name"].ToString();
            }
        }
        public DbTreeData()
        {
            ID = -1;
            ParentID = -1;
            Name = "";
            Node = new TreeNode(Name);
            Node.Tag = this;
        }
        public DbTreeData(DataRowView row):this()
        {
            Row = row;
        }
        public void Remove()
        {
            if (Node.TreeView != null)
            {
                Remove(Node);
            }
        }
        protected void Remove(TreeNode node)
        {
            foreach (TreeNode n in node.Nodes)
                Remove(node);
            node.Remove();
            ((DbTreeData)node.Tag).ID = -1;
        }
    }
    public class DbTreeBeforeDeleteEventArgs : EventArgs
    {
        public bool Cancel;
        public DbTreeBeforeDeleteEventArgs()
            : base()
        {
            Cancel = false;
        }
    }
    public delegate void DbTreeDelete(Object sender, DbTreeBeforeDeleteEventArgs e);
}
