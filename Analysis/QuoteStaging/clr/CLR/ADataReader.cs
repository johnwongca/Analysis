using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

using Microsoft.SqlServer.Server;

namespace etl.sqlnotes.info
{
    public abstract class ADataReader : IDataReader
    {
        bool mIsReady = false, mIsClosed = false;
        public bool IsClosed { get { return mIsClosed; } }
        public bool IsReady { get { return mIsReady; } }
        DataSetDefinition mSetDefinition = null;
        public DataSetDefinition SetDefinition
        {
            get
            {
                return mSetDefinition;
            }
        }
        public DataTable mSchemaTable = null;

        object[] mCurrentRow = null;
        public object[] CurrentRow { get { return mCurrentRow; } }



        int mMaxBufferSize = 100, mCurrentBufferSize = 0;
        long mRecordsRead = 0;
        ConcurrentQueue<object[]> mBuffer = new ConcurrentQueue<object[]>();
        public int MaxTargeteBufferSize { get { return mMaxBufferSize; } set { mMaxBufferSize = value; } }


        public void PopulateMetadataTable(DataSetDefinition metadata)
        {
            if (IsReady)
                return;
            mSetDefinition = metadata;
            mSetDefinition.ResetColumnOrdinal();
            mSchemaTable = mSetDefinition.GetSchemaTable();
            mIsReady = true;
        }
        public void PopulateMetadataTable(DataTable metadata)
        {
            if (IsReady)
                return;
            this.mSchemaTable = metadata;
            mSetDefinition = new DataSetDefinition();
            mSetDefinition.ResetColumnsWithSchemaTable(metadata);
            mSetDefinition.ResetColumnOrdinal();
            mSchemaTable = mSetDefinition.GetSchemaTable();
            mIsReady = true;
        }
        public DataTable GetSchemaTable()
        {
            if (!IsReady)
                InitializeDataSource();
            return mSchemaTable;
        }

        public virtual bool InitializeDataSource()
        {
            return true;
        }
        public virtual bool ReadOneFromDataSource(bool stop = false)
        {
            return !stop;
        }

        public virtual void CompleteDataSource()
        {

        }

        protected bool PushRecord(object[] data)
        {
            mBuffer.Enqueue(data);
            Interlocked.Increment(ref mCurrentBufferSize);
            Interlocked.Increment(ref mRecordsRead);
            return true;
        }
        public void Close() // this method will terminate all threads
        {
            CompleteDataSource();
            mIsClosed = true;
        }

        bool ReadQueue()
        {
            if (mIsClosed)
                return false;
            bool ret = mBuffer.TryDequeue(out mCurrentRow);
            if (ret)
            {
                Interlocked.Decrement(ref mCurrentBufferSize);
                ClearDataRecordsInternalList();
                OnReadInternal();
            }
            return ret;
        }
        public Action<object> OnRead;
        public Action<ADataReader> OnDataRequest;
        protected virtual void OnReadInternal()
        {
            if (OnRead != null)
                OnRead(this);
        }
        public bool Read()
        {
            if (!IsReady)
                InitializeDataSource();
            if (!ReadOneFromDataSource(false))
                return false;
            if (OnDataRequest != null)
            {
                OnDataRequest(this);
            }
            return ReadQueue();
        }

        public virtual void Dispose()
        {
            Close();
        }

        #region implementation of IDataReader
        public bool NextResult()
        {
            return false;
        }
        public int Depth { get { return 0; } }
        public int RecordsAffected { get { return -1; } }
        #endregion

        #region implementation of IDataRecords
        List<byte[]> byteList = null;
        List<char[]> charList = null;
        void ClearDataRecordsInternalList()
        {
            CreateDataRecordsInternalList();
            for (int i = 0; i < byteList.Count; i++)
            {
                byteList[i] = null;
                charList[i] = null;
            }
        }
        void CreateDataRecordsInternalList()
        {
            if (byteList == null)
            {
                DataTable dt = GetSchemaTable();
                byteList = new List<byte[]>();
                charList = new List<char[]>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    byteList.Add(null);
                    charList.Add(null);
                }
            }
        }
        public bool GetBoolean(int i)
        {
            return (bool)this[i];
        }
        public object this[string name]
        {
            get { return mCurrentRow[GetOrdinal(name)]; }
        }
        public object this[int i]
        {
            get { return mCurrentRow[i]; }
        }
        public byte GetByte(int i) { return (byte)this[i]; }
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            CreateDataRecordsInternalList();
            object obj = this[i];
            if (obj == null || obj == DBNull.Value)
                return 0;
            if (byteList[i] == null)
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    formatter.Serialize(ms, obj);
                    byteList[i] = ms.ToArray();
                }
            }
            if (fieldOffset >= byteList[i].Length)
                return 0;
            long bytesCopied = 0;
            for (long j = fieldOffset; i < byteList[i].Length && j < length; j++)
            {
                buffer[j - fieldOffset] = byteList[i][j];
                bytesCopied++;
            }
            return bytesCopied;
        }
        public char GetChar(int i)
        {
            return (char)this[i];
        }
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            CreateDataRecordsInternalList();
            object obj = this[i];
            if (obj == null || obj == DBNull.Value)
                return 0;
            if (charList[i] == null)
                charList[i] = obj.ToString().ToCharArray();
            if (fieldoffset >= charList[i].Length)
                return 0;
            long charsCopied = 0;
            for (long j = fieldoffset; i < byteList[i].Length && j < length; j++)
            {
                buffer[j - fieldoffset] = charList[i][j];
                charsCopied++;
            }
            return charsCopied;
        }
        //this is not supported in SqlDatareader anyways
        public IDataReader GetData(int i)
        {
            throw new Exception("Method GetData is not implemented");
        }
        public string GetDataTypeName(int i)
        {
            return mSetDefinition.Columns[i].DataTypeName;
        }
        public DateTime GetDateTime(int i)
        {
            return (DateTime)this[i];
        }
        public decimal GetDecimal(int i)
        {
            return (decimal)this[i];
        }
        public double GetDouble(int i)
        {
            return (double)this[i];
        }
        public Type GetFieldType(int i)
        {
            return (Type)this[i];
        }
        public float GetFloat(int i)
        {
            return (float)this[i];
        }
        public Guid GetGuid(int i)
        {
            return (Guid)this[i];
        }
        public short GetInt16(int i)
        {
            return (short)this[i];
        }
        public int GetInt32(int i)
        {
            return (int)this[i];
        }
        public long GetInt64(int i)
        {
            return (long)this[i];
        }
        public string GetName(int i)
        {
            return this.GetSchemaTable().Rows[i][SchemaTableColumn.ColumnName].ToString();
        }
        public int GetOrdinal(string name)
        {
            for (int i = 0; i < this.SetDefinition.Columns.Count; i++)
            {
                if (name == SetDefinition.Columns[i].Name)
                    return i;
                //return this.GetSchemaTable().Columns.IndexOf(name);
            }
            return -1;
        }
        public string GetString(int i)
        {
            return (string)this[i];
        }
        public object GetValue(int i)
        {
            return this[i];
        }
        public int GetValues(object[] values)
        {
            int maxAllowableLength = Math.Min(values.Length, mCurrentRow.Length);
            for (int i = 0; i < maxAllowableLength; i++)
                values[i] = this[i];
            return maxAllowableLength;
        }
        public bool IsDBNull(int i)
        {
            return this[i] is DBNull;
        }
        public int FieldCount { get { return this.GetSchemaTable().Rows.Count; } }
        #endregion


    }
}

