using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Base.Data;

namespace Console
{
	public partial class TestAPICursorExecutionEngine : BaseForm
	{
		APICursorExecutionEngine ee;
		public TestAPICursorExecutionEngine()
		{
			InitializeComponent();
		}
		
		private void btnOpenCursor_Click(object sender, EventArgs e)
		{
			conn.Open();
			ee.CommandText = txtSql.Text;
            /*ee.Parameters.Add("@batch_id",SqlDbType.Int);
            ee.Parameters[0].Value = 464;*/
			ee.Open();
    	}
		private void TestAPICursorExecutionEngine_Load(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Maximized;
			ee = new APICursorExecutionEngine(conn);
		}

		private void btnReConnectData_Click(object sender, EventArgs e)
		{
            dbgData.Columns.Clear();
            dbgData.DataSource = ee.Data;
            dbgData.AutoGenerateColumns = false;
            dbgData.AutoGenerateColumns = true;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			ee.Close();
			conn.Close();
		}

		private void conn_StateChange(object sender, StateChangeEventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
		}

        private void btnFirst_Click(object sender, EventArgs e)
        {
            ee.First();
            btnReConnectData_Click(sender, e);
        }

        private void btnPrivious_Click(object sender, EventArgs e)
        {
            ee.Previous();
            btnReConnectData_Click(sender, e);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ee.Next();
            btnReConnectData_Click(sender, e);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            ee.Last();
            btnReConnectData_Click(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ee.Refresh();
            btnReConnectData_Click(sender, e);
        }

        private void btnReconnectMetaData_Click(object sender, EventArgs e)
        {
            dbgSchema.Columns.Clear();
            dbgSchema.DataSource = ee.Sechema;
            dbgSchema.AutoGenerateColumns = false;
            dbgSchema.AutoGenerateColumns = true;
            dbgCursor.Columns.Clear();
            dbgCursor.DataSource = ee.Cursor;
            dbgCursor.AutoGenerateColumns = false;
            dbgCursor.AutoGenerateColumns = true;

            dbgTable.Columns.Clear();
            dbgTable.DataSource = ee.CursorTables;
            dbgTable.AutoGenerateColumns = false;
            dbgTable.AutoGenerateColumns = true;

            dbgColumn.Columns.Clear();
            dbgColumn.DataSource = ee.CursorColumns;
            dbgColumn.AutoGenerateColumns = false;
            dbgColumn.AutoGenerateColumns = true;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ee.UpdateTable = "batch_detail";
            ee.Delete(null);
        }

        private void btnReloadMetaData_Click(object sender, EventArgs e)
        {
            ee.DescribeCursor();
            ee.DescribeCursorColumns();
            ee.DescribeCursorTables();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ee.UpdateTable = "batch";
            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("name",(new Random()).Next().ToString()));
            ee.Update(p,null);
            ee.Refresh();
            btnReConnectData_Click(sender, e);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
             ee.UpdateTable = "batch";
            List<SqlParameter> p = new List<SqlParameter>();
            p.Add(new SqlParameter("name",(new Random()).Next().ToString()));
            p.Add(new SqlParameter("type",(new Random()).Next().ToString()));
            ee.Insert(p, null);
            ee.Refresh();
            btnReConnectData_Click(sender, e);
        }
	}
}

