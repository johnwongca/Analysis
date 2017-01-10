using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Design;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using Base;
using Base.Data;
using Base.Controls;


namespace Stock
{
	

	public partial class FormTest : BaseForm
	{
		DataTable dt;
		public FormTest()
		{System.ComponentModel.Design.
			InitializeComponent();
			dt = new DataTable();
			bindingSource1.DataSource = dt;
			dataGridView1.AutoGenerateColumns = false;
			dataGridView1.AutoGenerateColumns = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			SqlDataAdapter da = new SqlDataAdapter("select top 20 * from test1", conn);
			da.Fill(dt);
			for (int i = 0; i < 10; i++)
				dt.Rows.RemoveAt(0);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			
		}
	}
	public class test1
	{
		DataRow R;
		
	}
}

