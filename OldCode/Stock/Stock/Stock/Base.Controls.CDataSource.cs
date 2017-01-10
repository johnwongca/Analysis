using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using Base.Data;

namespace Base
{
	namespace Controls
	{
		public enum CursorLocation { Client, Server }
		public enum CursorType { Dynamic, KeySet, Static, ForwardOnly }
		public enum DataSourceState : int
		{
			Opening		= 0x1, 
			Opened		= 0x2,
			Closing		= 0x4,
			Closed		= 0x8,
			Inserting	= 0x10,	
			Editing		= 0x20,
			Deleting	= 0x40,
			Browsing	= 0x80
		}

		public class DataSourceException : Exception
		{
			public DataSourceException() : base("Could not set the property on an opened data source.") { }
			public DataSourceException(string message) : base(message) { }
		}
		public class MasterDetailLink
		{
			string masterField;
			string parameterName;
			public string MasterField
			{
				get { return masterField; }
				set { masterField = value; }
			}
			public string ParameterName
			{
				get { return parameterName; }
				set { parameterName = value;}
			}
		}
		public class CDataSource : System.Windows.Forms.BindingSource
		{
			private System.ComponentModel.IContainer components = null;

			protected override void Dispose(bool disposing)
			{
				if (disposing && (components != null))
				{
					components.Dispose();
				}
				base.Dispose(disposing);
			}
			private void InitializeComponent()
			{
				components = new System.ComponentModel.Container();
			}

			public CDataSource()
			{
				m_Data = null;
				base.DataSource = m_Data;
				
				m_allowEdit = true;
				m_allowRemove = true;
				m_allowNew = true;
				m_masterFields = new List<MasterDetailLink>();
				m_state = DataSourceState.Closed;
				m_columns = new List<DataColumn>();
				InitializeComponent();
			}
			
			#region Private Members

			DataTable m_Data;
			
			bool m_allowEdit, m_allowRemove, m_allowNew;
			DataSourceState m_state;
			CDataSource m_masterSource;
			List<MasterDetailLink> m_masterFields;
			List<DataColumn> m_columns;
			#endregion

			#region Protected Member
			protected DataTable Data
			{
				get { return m_Data; }
			}
			#endregion

			#region Protected Method
			#endregion


			#region Public Members
			public void Open()
			{
				return;
			}
			#endregion
			#region Public Events
			#endregion
			#region Private Events
			#endregion
			#region Invisible properties
			public DataSourceState State
			{
				get { return m_state; }
			}
			#endregion

			#region Visible Properties

			[Category("Data"), Description("Set master source.")]
			public CDataSource MasterSource
			{
				get { return m_masterSource; }
				set { m_masterSource = value; }
			}
			[Category("Data"), Description("Set linking fields between master and detail")]
			public List<MasterDetailLink> MasterFields
			{
				get { return m_masterFields; }
			}
			[Category("Data"), Description("Define columns")]
			public List<DataColumn> Columns
			{
				get { return m_columns; }
			}

			[Category("Behavior"), Description("Determine if the data in data source is changable."), Browsable(true)]
			public new bool AllowEdit
			{
				get { return m_allowEdit; }
				set { m_allowEdit = value; }
			}
			[Category("Behavior"), Description("Determine if a new record can be added to data source."), Browsable(true)]
			public new bool AllowNew
			{
				get { return m_allowNew; }
				set { m_allowNew = value; }
			}
			[Category("Behavior"), Description("Determine if a record can be removed from data source."), Browsable(true)]
			public new bool AllowRemove
			{
				get { return m_allowRemove; }
				set { m_allowRemove = value; }
			}
			[Category("Behavior"), Description("Determine if underlying data is read only.")]
			public bool ReadOnly
			{
				get { return !(m_allowEdit || m_allowNew || m_allowRemove); }
				set 
				{
					AllowEdit = !value;
					AllowNew = !value;
					AllowRemove = !value;
				}
			}
			[System.ComponentModel.Browsable(false)]
			public new Object DataSource { get { return base.DataSource; } }
			[System.ComponentModel.Browsable(false)]
			public new string DataMember { get { return base.DataMember; } }

			#endregion
		}
	}
}
