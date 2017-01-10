using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Text;
using System.Threading;


namespace JobSpoolerEngine
{
	public class ConcurrentGetting
	{
		public int Count ;
		public int TotalCount ;
	}
	public delegate void JobSpoolerRunSQLOnMessage(Object sender, System.Data.SqlClient.SqlException sqlException, System.Data.SqlClient.SqlInfoMessageEventArgs sqlMessage, System.Data.DataSet ds, string message);
	public class JobSpoolerGlobal
	{
		public static string SpoolerServerConnectionString = "";
		public static int SpoolerServerCommandTimeout = 30;
		public static int SpoolerServerHistoryCommandTimeout = 30;
		public static Object LockFlag;
		public static int MaxConcurrentServerReads = 5;
		public static ConcurrentGetting CurrentConcurrentGettings;
		public static JobSpooler appJobSpooler;
		public static string GenerateConnectionString(string pHostName, string pDatabase, string pUserName, string pPassword, bool pUseWindowsAuthentication)
		{
			if (pUseWindowsAuthentication)
				return ("integrated security=SSPI;data source=" + pHostName+";initial catalog="+pDatabase+";persist security info=True;Pooling=true;Max Pool Size=655350;");
			else 
				return("user id="+pUserName+";password="+pPassword+";data source=" + pHostName+";initial catalog="+pDatabase+";persist security info=false;Pooling=true;Max Pool Size=655350;");
					
		}
		public static void FreeMem()
		{
			GC.Collect();
			Thread.Sleep(10);
		}
	}
	public class JobSpoolerHistory
	{
		public long batch_id; 
		public long batch_detail_id;
		public void WriteHistory(string EventType, string Message)
		{
			SqlConnection c = new SqlConnection(JobSpoolerGlobal.SpoolerServerConnectionString);
			try
			{
				SqlCommand cmd = c.CreateCommand();
				c.Open();
				cmd.CommandTimeout = JobSpoolerGlobal.SpoolerServerHistoryCommandTimeout;
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.CommandText = "dbo.sp_history";
				cmd.Parameters.Add("@batch_id", SqlDbType.BigInt).Value = batch_id;
				cmd.Parameters.Add("@batch_detail_id",SqlDbType.BigInt).Value = batch_detail_id;
				cmd.Parameters.Add("@event", SqlDbType.VarChar, 50).Value = EventType;
				cmd.Parameters.Add("@message", SqlDbType.Text).Value = Message;
				cmd.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.WriteLine("Error: {0}, Batch: {1}, Detail {2}",ex.Message, batch_id, batch_detail_id);
			}
			finally
			{
				c.Close();
			}
		}
		public void WriteHistory(Object sender, SqlException sqlException)
		{
			int i;
			for( i=0; i<sqlException.Errors.Count; i++)
			{
				if (sqlException.Errors[i].Number == 0)
					WriteHistory("Message",sqlException.Errors[i].Message);
				else
				{
					WriteHistory("Error",
						"Error: " + sqlException.Errors[i].Number.ToString()+" " +
						"Line Number: " + sqlException.Errors[i].LineNumber.ToString()+" "+
						"Procedure: " + sqlException.Errors[i].Procedure.ToString()+" "+
						Environment.NewLine +
						sqlException.Errors[i].Message);
				}
			}
		}
		public void WriteHistory(Object sender, System.Data.SqlClient.SqlInfoMessageEventArgs sqlMessage)
		{
			WriteHistory("Message",sqlMessage.Message);
		}
		public void WriteHistory(Object sender, System.Data.DataSet ds)
		{
			string sql;
			SqlConnection c = new SqlConnection(JobSpoolerGlobal.SpoolerServerConnectionString);
			int[] columnSize;
			SqlParameter[] p;
			for(int i=0; i< ds.Tables.Count; i++)
			{
				WriteHistory("Result"+i.ToString(),ds.Tables[i].TableName);
				columnSize = null;
				columnSize = new int[ds.Tables[i].Columns.Count];
				p = null;
				p = new SqlParameter[ds.Tables[i].Columns.Count];
				for(int k = 0; k < ds.Tables[i].Rows.Count; k++)
				{
					for(int j = 0; j < ds.Tables[i].Columns.Count; j++)
					{
						if(ds.Tables[i].Columns[j].DataType == typeof(System.String))
						{
							if((ds.Tables[i].Rows[k][j].ToString()).Length > columnSize[j])
								columnSize[j] = ((string)ds.Tables[i].Rows[k][j]).Length;
							if (columnSize[j]==0) columnSize[j] = 10;
						}
						if(ds.Tables[i].Columns[j].DataType == typeof(System.Byte[]))
						{	
							if (ds.Tables[i].Rows[k][j].GetType() == typeof(System.DBNull))
							{
								if(columnSize[j] < 1) columnSize[j] = 10;
							}
							else if(((byte[])ds.Tables[i].Rows[k][j]).Length > columnSize[j])
								columnSize[j] = ((byte[])ds.Tables[i].Rows[k][j]).Length;
							if (columnSize[j]==0) columnSize[j] = 10;
						}
					}
				}
				sql = "create table " + ds.Tables[i].TableName + "(";
				for(int j = 0; j < ds.Tables[i].Columns.Count; j++)
				{
					p[j] = new SqlParameter();
					p[j].ParameterName = "@P"+j.ToString();
					p[j].IsNullable = true;
					if(ds.Tables[i].Columns[j].DataType == typeof(System.String))
					{
						if (columnSize[j] < 8000)
						{
							sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] varchar("+columnSize[j].ToString()+"),";
							p[j].SqlDbType = SqlDbType.VarChar;
							p[j].Size = columnSize[j];
						}
						else
						{
							sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] text,";
							p[j].SqlDbType = SqlDbType.Text;
						}
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.Byte[]))
					{
						if (columnSize[j] < 8000)
						{
							sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] varbinary("+columnSize[j].ToString()+"),";
							p[j].SqlDbType = SqlDbType.VarBinary;
							p[j].Size = columnSize[j];
						}
						else
						{
							sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] image,";
							p[j].SqlDbType = SqlDbType.Image;
						}
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.Boolean))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] bit,";
						p[j].SqlDbType = SqlDbType.Bit;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.Byte))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] tinyint,";
						p[j].SqlDbType = SqlDbType.TinyInt;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.DateTime))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] DateTime,";
						p[j].SqlDbType = SqlDbType.DateTime;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.Decimal))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] numeric(38,8),";
						p[j].SqlDbType = SqlDbType.Decimal;
						p[j].Precision = 38;
						p[j].Scale = 8;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.Double))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] float,";
						p[j].SqlDbType = SqlDbType.Float;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.Int16))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] smallint,";
						p[j].SqlDbType = SqlDbType.SmallInt;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.Int32))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] int,";
						p[j].SqlDbType = SqlDbType.Int;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.Int64))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] bigint,";
						p[j].SqlDbType = SqlDbType.BigInt;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.SByte))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] int,";
						p[j].SqlDbType = SqlDbType.Int;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.Single))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] real,";					
						p[j].SqlDbType = SqlDbType.Real;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.TimeSpan))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] varchar(50),";
						p[j].SqlDbType = SqlDbType.VarChar;
						p[j].Size = 50;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.UInt16))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] numeric(38,8),";
						p[j].SqlDbType = SqlDbType.Decimal;
						p[j].Precision = 38;
						p[j].Scale = 8;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.UInt32))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] numeric(38,8),";
						p[j].SqlDbType = SqlDbType.Decimal;
						p[j].Precision = 38;
						p[j].Scale = 8;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.UInt64))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] numeric(38,8),";
						p[j].SqlDbType = SqlDbType.Decimal;
						p[j].Precision = 38;
						p[j].Scale = 8;
					}
					else if(ds.Tables[i].Columns[j].DataType == typeof(System.Guid))
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] uniqueidentifier,";
						p[j].SqlDbType = SqlDbType.UniqueIdentifier;
					}
					else 
					{
						sql = sql + "["+ds.Tables[i].Columns[j].ColumnName+"] varchar(80),"; 
						p[j].SqlDbType = SqlDbType.VarChar;
						p[j].Size = 80;
					}
				}
				sql = sql + " [primary_id] uniqueidentifier default(newid()) )";
				try
				{
					SqlCommand cmd = c.CreateCommand();
					c.Open();
					cmd.CommandTimeout = JobSpoolerGlobal.SpoolerServerHistoryCommandTimeout;
					cmd.CommandType = System.Data.CommandType.Text;
					cmd.CommandText = sql;
					cmd.ExecuteNonQuery();
					for(int k = 0; k < ds.Tables[i].Rows.Count; k++)
					{
						sql = "insert into "+ds.Tables[i].TableName + " values(";
						for(int x = 0; x <ds.Tables[i].Columns.Count; x++)
						{
							sql = sql + "@P"+x.ToString()+",";
						}
						sql = sql + "newid())";
						cmd.Parameters.Clear();
						cmd.CommandText = sql;
						for(int x = 0; x <ds.Tables[i].Columns.Count; x++)
						{
							p[x].Value = ds.Tables[i].Rows[k][x];
							cmd.Parameters.Add(p[x]);
						}
						cmd.ExecuteNonQuery();
					}
				}
				catch(Exception ex)
				{
					Console.WriteLine("Error: {0}, Batch: {1}, Detail {2}",ex.Message, batch_id, batch_detail_id);
				}
				finally
				{
					c.Close();
					GC.Collect();
				}
			}
		}
	}
	public class JobSpoolerRunSQLException:System.Exception
	{
	}
	public class JobSpoolerRunSQL
	{
		public string HostName, Database, UserName, Password, CommandText;
		public int ConnectionTimeout, CommandTimeout;
		public bool UseWindowsAuthentication, RunInTransaction, CommitWhenError, SaveResultDataset;
		public string ConnectionString
		{
			get
			{
				return(JobSpoolerGlobal.GenerateConnectionString(HostName, Database, UserName, Password, UseWindowsAuthentication)+"Pooling=false;");
			}
		}
		public event JobSpoolerRunSQLOnMessage OnMessage;
		public JobSpoolerRunSQL()
		{
			ConnectionTimeout = 300;
			CommandTimeout = 3600;
			UseWindowsAuthentication = false;
			RunInTransaction = false;
			CommitWhenError = false;
			SaveResultDataset = false;
		}
		public void Start()
		{
			System.Data.SqlClient.SqlConnection Session = new System.Data.SqlClient.SqlConnection();
			Session.InfoMessage += new System.Data.SqlClient.SqlInfoMessageEventHandler(this.Session_InfoMessage);
			System.Data.SqlClient.SqlTransaction Trans = null;
			Session.ConnectionString = ConnectionString + "Timeout="+ConnectionTimeout.ToString();
			try
			{
				if (OnMessage != null)
					OnMessage(this, null, null, null, "Connect to "+HostName);
				Session.Open();
				if (OnMessage != null)
					OnMessage(this, null, null, null, HostName +" connected");
				if (RunInTransaction)
				{
					if (OnMessage != null)
						OnMessage(this, null, null, null, "Begin transaction");
					Trans = Session.BeginTransaction(IsolationLevel.ReadCommitted);					
				}
				System.Data.SqlClient.SqlCommand cmd = Session.CreateCommand();
				cmd.CommandText = "exec(@cmd)";
				cmd.Parameters.Add("@cmd",SqlDbType.NText);
				cmd.Parameters[0].Value = CommandText;
				cmd.Transaction = Trans;
				cmd.CommandTimeout = CommandTimeout;
				if(SaveResultDataset)
				{
					System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter();
					System.Data.DataSet ds = new DataSet();
					da.SelectCommand = cmd;
					da.Fill(ds);
					for(int i=0; i< ds.Tables.Count; i++)
					{
						ds.Tables[i].TableName = String.Format("[  {0}.{1:yyyyMMdd.HHmmss}.{2:00}...{3}]",HostName ,System.DateTime.Now,i,System.Guid.NewGuid());
					}
					if (OnMessage != null)
						OnMessage(this, null, null, ds, "");
				}
				else
				{
					cmd.ExecuteNonQuery();
				}
				if (RunInTransaction)
				{
					if(Trans != null) 
					{
						Trans.Commit();
						if (OnMessage != null)
							OnMessage(this, null, null, null, "Commit transaction");
					}
				}
			}
			catch(Exception er1)
			{
				if (OnMessage != null)
					OnMessage(this,(System.Data.SqlClient.SqlException) er1, null, null, "");
				if (RunInTransaction && Trans != null)
				{
					if(CommitWhenError)
					{
						Trans.Commit();
						if (OnMessage != null)
							OnMessage(this, null, null, null, @"Commit transaction with error");
					}
					else
					{
						Trans.Rollback(); 
						if (OnMessage != null)
							OnMessage(this, null, null, null, "Rollback transaction");
					}
				}
				throw new JobSpoolerRunSQLException();
			}
			finally
			{
				Session.Close();
				if (OnMessage != null)
					OnMessage(this, null, null, null, "Disconnected");
				JobSpoolerGlobal.FreeMem();
			}
		}
		private void Session_InfoMessage(object sender, System.Data.SqlClient.SqlInfoMessageEventArgs e)
		{   
			if (OnMessage != null)
				OnMessage(this, null, e, null, "");
		}	
	}

	public class JobSpoolerJob
	{
		JobSpoolerRunSQL runSQL;
		JobSpoolerHistory his;
		bool MarkAsCompleteWhenError;
		private void OnMessage(Object sender, System.Data.SqlClient.SqlException sqlException, System.Data.SqlClient.SqlInfoMessageEventArgs sqlMessage, System.Data.DataSet ds, string message)
		{
			if(sqlException != null)
				his.WriteHistory(sender,sqlException);
			if(sqlMessage != null)
				his.WriteHistory(sender,sqlMessage);
			if(ds != null)
				if (ds.Tables.Count > 0)
					his.WriteHistory(sender,ds);
			if(message != "")
				his.WriteHistory("Running", message);
		}
		public JobSpoolerJob()
		{
			runSQL = new JobSpoolerRunSQL();
			runSQL.OnMessage += new JobSpoolerRunSQLOnMessage(this.OnMessage);
			his = new JobSpoolerHistory();
		}
		public void Start()
		{
			lock(JobSpoolerGlobal.CurrentConcurrentGettings)
			{
				JobSpoolerGlobal.CurrentConcurrentGettings.TotalCount++;
			}
			Thread.CurrentThread.Name = "Thread " + JobSpoolerGlobal.CurrentConcurrentGettings.TotalCount.ToString();
			try
			{
				while(RunJob()){}
			}
			finally
			{
				lock(JobSpoolerGlobal.CurrentConcurrentGettings)
				{
					JobSpoolerGlobal.CurrentConcurrentGettings.TotalCount--;
				}
			}
		}
		public bool RunJob()
		{
			bool Result = false, RecordRead = false, isFinished = false;
			SqlConnection con = new SqlConnection(JobSpoolerGlobal.SpoolerServerConnectionString);
			con.Open();
			SqlCommand cmd = con.CreateCommand();
			SqlDataReader r = null; 
			cmd.CommandTimeout = JobSpoolerGlobal.SpoolerServerCommandTimeout;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "sp_get_job";
			SqlTransaction tran = con.BeginTransaction(IsolationLevel.ReadCommitted);
			cmd.Transaction = tran;
			try
			{
				if(JobSpoolerGlobal.CurrentConcurrentGettings.Count < JobSpoolerGlobal.MaxConcurrentServerReads)
				{
					lock(JobSpoolerGlobal.CurrentConcurrentGettings)
					{
						JobSpoolerGlobal.CurrentConcurrentGettings.Count++;
					}
					try
					{
						r = cmd.ExecuteReader();
						RecordRead = r.Read();
					}
					finally
					{
						lock(JobSpoolerGlobal.CurrentConcurrentGettings)
						{
							JobSpoolerGlobal.CurrentConcurrentGettings.Count--;
						}
					}
				}
				else
				{
					lock(JobSpoolerGlobal.LockFlag)
					{
						r = cmd.ExecuteReader();
						RecordRead = r.Read();
					}
				}
				if(RecordRead)
				{
					if(r["status"].ToString().ToLower() == "wait")
					{
						r.Close();
						tran.Rollback();
						return true;
					}
					his.batch_detail_id = r.GetSqlInt64(r.GetOrdinal("batch_detail_id")).Value;
					his.batch_id = r.GetSqlInt64(r.GetOrdinal("batch_id")).Value;
					runSQL.HostName = r["instance_name"].ToString();
					runSQL.Database = r["database_name"].ToString();
					runSQL.UseWindowsAuthentication = r.GetSqlBoolean(r.GetOrdinal("use_windows_authentication")).Value;
					runSQL.UserName = r["user_name"].ToString();
					runSQL.Password = r["password"].ToString();
					runSQL.CommandText = r["sql"].ToString();
					runSQL.RunInTransaction = r.GetSqlBoolean(r.GetOrdinal("run_in_transaction")).Value; 
					runSQL.CommitWhenError = !r.GetSqlBoolean(r.GetOrdinal("rollback_trans_when_error")).Value;
					MarkAsCompleteWhenError = r.GetSqlBoolean(r.GetOrdinal("mark_as_complete_when_error")).Value;
					runSQL.ConnectionTimeout = r.GetSqlInt32(r.GetOrdinal("connection_timeout")).Value;
					runSQL.CommandTimeout = r.GetSqlInt32(r.GetOrdinal("command_timeout")).Value; 
					runSQL.SaveResultDataset = r.GetSqlBoolean(r.GetOrdinal("save_result_dataset")).Value; 
					r.Close();
					runSQL.Start();
					tran.Commit();
					isFinished = true;
					his.WriteHistory("Running", "<<<Finished>>>");
					Result = true;
				}
				else
				{
					r.Close();
					tran.Rollback();
				}
			}
			catch(JobSpoolerRunSQLException )
			{
				if(MarkAsCompleteWhenError)
				{
					tran.Commit();
					his.WriteHistory("Running",@"<<<Finished>>> with error");
				}
				else
				{
					tran.Rollback();
					his.WriteHistory("Running","<<<Finished>>> with error");
				}
				Result = true;
			}
			catch(Exception er2)
			{
				if(!isFinished)
				{
					his.WriteHistory("Error",er2.Message);
					Console.WriteLine("Error: {0}, Batch: {1}, Detail {2}",er2.Message, his.batch_id, his.batch_detail_id);
				}
			}
			finally
			{
				con.Close();
				JobSpoolerGlobal.FreeMem();
			}
			return(Result);
		}
	}
	class JobSpoolerThreadState
	{
		public ManualResetEvent manualEvent;

		public JobSpoolerThreadState(ManualResetEvent manualEvent)
		{
			this.manualEvent = manualEvent;
		}
	}
	[Serializable]
	public class JobSpooler
	{
		public string HostName, Database, UserName, Password;
		public bool UseWindowsAuthentication;
		public int CommandTimeout, HistoryCommandTimeout, MaxThreads, MaxConcurrentServerReads;
		public static void RunSQLs(Object stateInfo)
		{
			
			(new JobSpoolerJob()).Start();
			if(stateInfo != null)
				((JobSpoolerThreadState)stateInfo).manualEvent.Set();
		}
		public static void RunSQLs()
		{			
			(new JobSpoolerJob()).Start();
		}
		static void Main() 
		{
			Console.WriteLine("Start spooling jobs to servers {0:yyyy-MM-dd HH:mm:ss}", System.DateTime.Now);
			string filename = Application.ExecutablePath;
			filename = filename.Substring(0,filename.LastIndexOf('.'))+".xml";
			XmlSerializer s = new XmlSerializer(typeof(JobSpooler));
			try
			{
				if (System.IO.File.Exists(filename))
				{
					Stream fs = new FileStream(filename, FileMode.Open);
					XmlReader r = new XmlTextReader(fs);					
					JobSpoolerGlobal.appJobSpooler = (JobSpooler) s.Deserialize(r);
					r.Close();
					fs.Close();
				}
				else
				{
					JobSpoolerGlobal.appJobSpooler = new JobSpooler();
					JobSpoolerGlobal.appJobSpooler.HostName = "HostName";
					JobSpoolerGlobal.appJobSpooler.Database = "DatabaseName";
					JobSpoolerGlobal.appJobSpooler.UserName = "UserName";
					JobSpoolerGlobal.appJobSpooler.Password = "Password";
					JobSpoolerGlobal.appJobSpooler.UseWindowsAuthentication = true;
					JobSpoolerGlobal.appJobSpooler.CommandTimeout = 10;
					JobSpoolerGlobal.appJobSpooler.HistoryCommandTimeout = 30;
					Stream fs = new FileStream(filename, FileMode.Create);
					XmlWriter w =	new XmlTextWriter(fs, new UTF8Encoding());
					s.Serialize(w, JobSpoolerGlobal.appJobSpooler);
					w.Close();
					System.Console.WriteLine("Configuration file created.");
					return;
				}
				JobSpoolerGlobal.SpoolerServerConnectionString = JobSpoolerGlobal.GenerateConnectionString(JobSpoolerGlobal.appJobSpooler.HostName, JobSpoolerGlobal.appJobSpooler.Database, JobSpoolerGlobal.appJobSpooler.UserName, JobSpoolerGlobal.appJobSpooler.Password, JobSpoolerGlobal.appJobSpooler.UseWindowsAuthentication);
				JobSpoolerGlobal.MaxConcurrentServerReads = JobSpoolerGlobal.appJobSpooler.MaxConcurrentServerReads;
				SqlConnection con = new SqlConnection(JobSpoolerGlobal.SpoolerServerConnectionString);
				con.Open();
				con.Close();
				JobSpoolerGlobal.LockFlag = new object();
				JobSpoolerGlobal.CurrentConcurrentGettings = new ConcurrentGetting();
                JobSpoolerGlobal.CurrentConcurrentGettings.Count = 0;
				JobSpoolerGlobal.CurrentConcurrentGettings.TotalCount = 0;
				//RunSQLs(null);  //Test
				//Use Thread Pool
				/*
				int workerThreads, portThreads;
				ThreadPool.GetMaxThreads(out workerThreads, out portThreads);
				Console.WriteLine("Maximum worker Threads: {0}; Maximum completion Port Threads {1}; Maximum server reads {2}",workerThreads, portThreads, JobSpoolerGlobal.MaxConcurrentServerReads);
				if(JobSpoolerGlobal.appJobSpooler.MaxThreads > workerThreads)
					JobSpoolerGlobal.appJobSpooler.MaxThreads = workerThreads;
				ManualResetEvent[] manualEvents = new ManualResetEvent[JobSpoolerGlobal.appJobSpooler.MaxThreads];
				for(int i = 0; i < JobSpoolerGlobal.appJobSpooler.MaxThreads; i++)
				{
					manualEvents[i] = new ManualResetEvent(false);
					ThreadPool.QueueUserWorkItem(new WaitCallback(RunSQLs), new JobSpoolerThreadState(manualEvents[i]) );
				}
				
				ManualResetEvent[] waitManualEvents = new ManualResetEvent[1];
				for(int i = 0; i < JobSpoolerGlobal.appJobSpooler.MaxThreads; i++)
				{
					waitManualEvents[0] = manualEvents[i];
					WaitHandle.WaitAll(waitManualEvents);
					Thread.Sleep(13);
				}*/
				//Use Thread
				Thread[] t = new Thread[JobSpoolerGlobal.appJobSpooler.MaxThreads];
				for(int i = 0; i < JobSpoolerGlobal.appJobSpooler.MaxThreads; i++)
				{
					t[i] = new Thread(new ThreadStart(RunSQLs));
					t[i].Start();
					Thread.Sleep(10);
				}
				for(int i = 0; i < JobSpoolerGlobal.appJobSpooler.MaxThreads; i++)
				{
					t[i].Join();
				}
			}
			catch(Exception e1)
			{
				System.Console.WriteLine("Error: "+e1.Message);
				return;
			}
			finally
			{
				Console.WriteLine("spooling finished @{0:yyyy-MM-dd HH:mm:ss}", System.DateTime.Now);
				System.Console.WriteLine("Done...");
				//System.Console.Read();
			}
		}
	}
}
