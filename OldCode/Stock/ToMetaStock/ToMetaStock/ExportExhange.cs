using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using METALIBLib;

namespace ToMetaStock
{
	public class ExportExhange
	{
		SqlConnection conn = null;
		string m_rootFolder, m_exchangeName, m_Folder, m_Symbol, m_date;
		bool m_performSort;
		MLWriter m_writer;
		int m_daysBack, m_percent, m_ExchangeID;
		object m_param = null;
		object m_error = null;
		public bool PerformSort
		{
			get { return m_performSort; }
		}
		public string ExchangeName
		{
			get { return m_exchangeName; }
		}
		public int ExchangeID
		{
			get { return m_ExchangeID;  }
		}
		public string Folder
		{
			get { return m_Folder; }
		}
		public string RootFolder
		{
			get { return m_rootFolder; }
		}
		public string Symbol
		{
			get { return m_Symbol; }
		}
		public string Date
		{
			get { return m_date; }
		}
		public int DaysBack
		{
			get { return m_daysBack; }
		}
		public int Percent
		{
			get { return m_percent; }
		}
		public object Param
		{
			get { return m_param; }
		}
		public object Error
		{
			get { return m_error; }
		}
		public event EventHandler OnProgress;
		public ExportExhange(string rootFolder, string exchangeName, int daysBack, bool performSort)
		{
			m_rootFolder = rootFolder;
			m_exchangeName = exchangeName;
			m_daysBack = daysBack;
			m_Folder = rootFolder + @"\" + exchangeName;
			m_Symbol = "";
			m_date = "";
			m_percent = 0;
			m_performSort = performSort;
			conn = new SqlConnection("Data Source=localhost;Initial Catalog=Stock;Integrated Security=True;MultipleActiveResultSets=True");
			try
			{
				SqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = "select ID from STK.Exchange where name = '" + ExchangeName.Replace("'", "''") + "'";
				cmd.CommandTimeout = 0;
				conn.Open();
				SqlDataReader r = cmd.ExecuteReader();
				if (!r.Read())
				{
					r.Close();
					throw new Exception("Could not find Exchange " + ExchangeName);
				}
				m_ExchangeID = (int)r[0];
				r.Close();
			}
			finally
			{
				conn.Close();
			}
		}
		public ExportExhange(string rootFolder, string exchangeName, int daysBack, bool performSort, object param)
			: this(rootFolder, exchangeName, daysBack, performSort)
		{
			m_param = param;
		}
		DataTable GetRecordSet(SqlConnection connection, string SQL)
		{
			DataTable ret = new DataTable();
			SqlDataAdapter da = new SqlDataAdapter(SQL, connection);
			da.SelectCommand.CommandTimeout = 0;
			da.Fill(ret);
			return ret;
		}
		public bool Export()
		{
			string folderIndex, lastDate;
			DateTime dt;
			int i;
			m_percent = 0;
			if(!Directory.Exists(RootFolder))
				Directory.CreateDirectory(RootFolder);
			if (!Directory.Exists(Folder))
				Directory.CreateDirectory(Folder);
			try
			{
				m_writer = new MLWriter();
				conn.Open();
				DataTable symbolList = GetRecordSet(conn, "exec STK.GetSymbol " + ExchangeID.ToString());
				i = 0;
				foreach (DataRow symbol in symbolList.Rows)
				{
					m_percent = i*100 / symbolList.Rows.Count;
					m_Symbol = symbol["Name"].ToString().Trim();
					folderIndex = Folder + @"\" + symbol["Name"].ToString().Substring(0, 1);
					if (!Directory.Exists(folderIndex))
					{
						m_writer.CreateDirectory(folderIndex);
						m_writer.CloseDirectory();
					}
					m_writer.OpenDirectory(folderIndex);
					if (!m_writer.get_bSymbolExists(symbol["Name"].ToString()))
						m_writer.AppendSecurity(symbol["Name"].ToString(), symbol["Description"].ToString().Trim(), PERIODICITY.Daily);
					else
						m_writer.OpenSecurityBySymbol(symbol["Name"].ToString());
					lastDate = m_writer.iLastDate.ToString();
					if (m_writer.iLastDate > 0)
					{
						dt = new DateTime(Int32.Parse(lastDate.Substring(0, 4)), Int32.Parse(lastDate.Substring(4, 2)), Int32.Parse(lastDate.Substring(6, 2)));
						dt = dt.AddDays(Math.Abs(DaysBack) * (-1) );
					}
					else
						dt = new DateTime(1932, 01, 01);
					DataTable security = GetRecordSet(conn, "exec STK.GetSecurity " + ExchangeID.ToString() + ", '" + symbol["Name"].ToString() + "', '" + dt.ToString("yyyy-MM-dd") + "'");
					foreach (DataRow sec in security.Rows)
					{
						//try { m_writer.DeleteSecRecord((int)sec["DateInt"]); } catch{}
						try 
						{ 
							//if(!m_writer.get_bDateExists((int)sec["DateInt"]))
								m_writer.AppendDataRec((int)sec["DateInt"], 0, Single.Parse(sec["Opening"].ToString()), Single.Parse(sec["High"].ToString()), Single.Parse(sec["Low"].ToString()), Single.Parse(sec["Closing"].ToString()), Single.Parse(sec["Volume"].ToString()), Single.Parse(sec["Interest"].ToString())); 
						}
						catch { }
					}
					try { if(PerformSort) m_writer.SortEx(); }	catch { }
					m_writer.CloseSecurity();
					m_writer.CloseDirectory();
					security = null;
					GC.Collect();
					if (OnProgress != null)
						OnProgress(this, new EventArgs());
					i ++;
				}
			}
			/*catch(Exception er)
			{
				m_error = er;
				if (OnProgress != null)
					OnProgress(this, new EventArgs());
				return false;
			}*/
			finally
			{
				m_writer = null;
				conn.Close();
				GC.Collect();
			}
			return true;
		}
	}
}
