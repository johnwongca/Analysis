using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace EODDataService
{
    public enum DownloadTaskStatus{Pending, Running, Complete}
    public class DownloadTask :IDisposable
    {
        static object SyncObject = new object();
        static List<DownloadTask> RunningTasks = new List<DownloadTask>();
        static void AddToList(DownloadTask downloadTask)
        {
            lock (SyncObject)
            {
                RunningTasks.Add(downloadTask);
            }
        }
        static void RemoveFromList(DownloadTask downloadTask)
        {
            lock (SyncObject)
            {
                try
                {
                    StatusUpdate(downloadTask, "Done");
                }
                finally
                {
                    RunningTasks.Remove(downloadTask);
                }
            }
        }
        static void StatusUpdate(DownloadTask downloadTask, string status)
        {
            lock (SyncObject)
            {
                using (SqlConnection connection = new SqlConnection(G.ConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd= connection.CreateCommand();
                    cmd.CommandText = "[EODData].[SetSessionStatus]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ManagementThreadID", SqlDbType.Int).Value = Thread.CurrentThread.ManagedThreadId;
                    cmd.Parameters.Add("@TaskSessionID", SqlDbType.SmallInt).Value = downloadTask.SessionID;
                    cmd.Parameters.Add("@TaskID", SqlDbType.Int).Value = downloadTask.TaskID;
                    cmd.Parameters.Add("@PoolID", SqlDbType.SmallInt).Value = downloadTask.PoolID;
                    cmd.Parameters.Add("@MethodName", SqlDbType.VarChar, 50).Value = downloadTask.MethodName;
                    cmd.Parameters.Add("@IntervalID", SqlDbType.TinyInt).Value = (byte)downloadTask.IntervalID;
                    cmd.Parameters.Add("@Exchange", SqlDbType.VarChar, 10).Value = downloadTask.Exchange;
                    cmd.Parameters.Add("@DateFrom", SqlDbType.Date).Value = downloadTask.DateFrom;
                    cmd.Parameters.Add("@Status", SqlDbType.VarChar, 50).Value = status;
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, -1).Value = downloadTask.ExceptionInSqlFormat;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public static int TaskCount
        {
            get 
            {
                lock (SyncObject)
                    return RunningTasks.Count;
            }
        }
        public static void WaitAll()
        {
            while (TaskCount != 0)
                Thread.Sleep(10);
        }

        
        SqlConnection connection = new SqlConnection(G.ConnectionString);
        public Task task;
        public List<Exception> Exceptions = new List<Exception>();
        public SqlString ExceptionInSqlFormat
        {
            get 
            {
                return Exceptions.Count == 0? SqlString.Null:new SqlString(string.Join("\n", Exceptions.Select(x => x.Message).ToArray()));
            }
        }
        public DownloadTaskStatus Status = DownloadTaskStatus.Pending;

        public short SessionID = -1;
        public short PoolID = -1;
        public int TaskID = -1;
        public string MethodName = ""; 
        public string Exchange = "";
        public EODDataInterval IntervalID = EODDataInterval.None;
        public DateTime DateFrom = DateTime.MinValue;
        public DownloadTask()
        {
            AddToList(this);
            task = Task.Factory.StartNew(TaskBody, TaskCreationOptions.LongRunning);
            StatusUpdate(this, "Object Created");
        }

        bool OpenConnection()
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "@set xact_abort off; begin transaction;select @@spid";
                SessionID = (short)cmd.ExecuteScalar();
                return true;
            }
            catch(Exception e)
            {
                Exceptions.Add(e);
                return false;
            }
        }
        void CloseConnection()
        {
            try
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "while @@trancount > 0 commit;";
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Exceptions.Add(e);
            }
        }
        void ExecuteTask()
        {
            try
            {
                if (MethodName.ToUpper() == "GetCountries".ToUpper())
                    G.EODDataConnection.GetCountries().WriteToServer();
            }
            catch(Exception e)
            {
                Exceptions.Add(e);
            }
            finally
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.CommandText = "[EODData].[EndTask]";
                cmd.Parameters.Add("@PoolID", SqlDbType.SmallInt).Value = PoolID;
                cmd.Parameters.Add("@TaskID", SqlDbType.Int).Value = TaskID;
                cmd.Parameters.Add("@Error", SqlDbType.VarChar, -1).Value = ExceptionInSqlFormat;
                cmd.ExecuteNonQuery();
            }
        }
        bool ReadTask()
        {
            try
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "EODData.GetTask";
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r == null)
                        return false;
                    if(r.Read())
                    {
                        PoolID = r.IsDBNull(0) ? (short)-1 : r.GetInt16(0);
                        TaskID = r.IsDBNull(1) ? -1 : r.GetInt32(1);
                        MethodName = r.IsDBNull(2) ? "" : r.GetString(2);
                        Exchange = r.IsDBNull(3) ? "" : r.GetString(3);
                        IntervalID = r.IsDBNull(4) ? EODDataInterval.None : (EODDataInterval)r.GetByte(4);
                        DateFrom = r.IsDBNull(5) ? DateTime.MinValue : r.GetDateTime(5);
                        return true;
                    }
                    r.Close();
                }
                return false;
            }
            catch(Exception e)
            {
                Exceptions.Add(e);
                return false;
            }
        }
        void TaskBody()
        {
            try
            {
                StatusUpdate(this, "Opening Connection");
                if (!OpenConnection())
                    return;
                StatusUpdate(this, "Reading Task");
                if (!ReadTask())
                    return;
                StatusUpdate(this, "Executing Task");
                Status = DownloadTaskStatus.Running;
                ExecuteTask();
            }
            finally
            {
                Status = DownloadTaskStatus.Complete;
                CloseConnection();
                RemoveFromList(this);
            }
        }

        public void Dispose()
        {
            try 
            { 
                connection.Close(); 
                
            }
            catch { }
            Exceptions.Clear();

        }
    }
}
