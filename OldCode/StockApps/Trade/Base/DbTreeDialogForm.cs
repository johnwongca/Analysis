using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trade.Base
{
    public partial class DbTreeDialogForm : Form
    {
        public DbTreeDialogForm()
        {
            m_Data = null;
            m_Current = null;
            InitializeComponent();
        }
        List<DbTreeData> m_Data;
        public List<DbTreeData> Data
        {
            get { return m_Data; }
            set 
            {
                this.m_Data = new List<DbTreeData>();
                foreach(var m in value)
                    m_Data.Add(new DbTreeData() { ID = m.ID, ParentID = m.ParentID, Name = m.Name});
                GC.Collect();
            }
        }
        DbTreeData m_Current;
        public DbTreeData Current
        {
            get 
            {
                m_Current.Name = tbName.Text;
                if (ckUseRoot.Checked)
                {
                    m_Current.ParentID = -2;
                }
                else
                {
                    if (tv.SelectedNode == null)
                    {
                        m_Current.ParentID = -2;
                    }
                    else
                    {
                        m_Current.ParentID = ((DbTreeData)tv.SelectedNode.Tag).ID;
                    }
                }
                return m_Current; 
            }
            set 
            { 
                m_Current = value;
                tbName.Text = m_Current.Name;
            }
        }

        private void ckUseRoot_CheckedChanged(object sender, EventArgs e)
        {
            tv.Enabled = !ckUseRoot.Checked;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            m_Current.Name = tbName.Text;
        }
        public void LoadData()
        {
            LoadTree(null);
            DbTreeData d = m_Data.FirstOrDefault(x => { return x.ID == Current.ID; });
            if (d != null)
            {
                d.Node.Remove();
                var nodes = m_Data.Where(x => { return x.Node.TreeView == null; }).ToList();
                foreach (var n in nodes)
                    m_Data.Remove(n);
            }
            if (d.ParentID >= 0)
            {
                ckUseRoot.Checked = false;
                tv.Enabled = true;
                tv.Select();
                tv.SelectedNode = m_Data.FirstOrDefault(x => { return x.ID == d.ParentID; }).Node;
                tv.SelectedNode.Expand();
            }
        }
        private void LoadTree(DbTreeData dt)
        {
            TreeNodeCollection Nodes;
            List<DbTreeData> children;
            if (dt == null)
            {
                children = m_Data.Where(x => { return x.ParentID == -1; }).ToList<DbTreeData>();
                Nodes = tv.Nodes;
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
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
