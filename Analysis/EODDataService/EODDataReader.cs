using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EODDataService
{
    public class EODDataReader:IDataReader
    {
        static DateTime EarliestDateTime = new DateTime(1900,1,1);
        int mCurrentLocation = -1;
        object[][] mData;
        DataTable mSchemaTable = new DataTable();

       public object[][] Data
        {
            get { return mData; }
            set { mData = value; }
        }
        public EODDataReader(DataTable schemaTable, object[][] data)
        {
            mSchemaTable = schemaTable;
            mData = data;
        }

        public EODDataReader(DataTable schemaTable)
        {
            mSchemaTable = schemaTable;
        }
        public bool IsClosed
        {
            get { return false; }
        }
        public DataTable GetSchemaTable()
        {
            return mSchemaTable;
        }

        public void Close() // this method will terminate all threads
        {
            
        }

        public bool Read()
        {
            if (mCurrentLocation < mData.Length-1)
            {
                mCurrentLocation++;
                return true;
            }
            return false;
        }

        public virtual void Dispose()
        {
            mData = null;
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
        public bool GetBoolean(int i)
        {
            return (bool)mData[mCurrentLocation][i];
        }
        public object this[string name]
        {
            get { return mData[mCurrentLocation][GetOrdinal(name)]; }
        }
        public object this[int i]
        {
            get 
            {
                object ret = mData[mCurrentLocation][i];
                if(ret is DateTime)
                    if ((DateTime)ret < EarliestDateTime)
                         ret = DBNull.Value;
                return ret;
            }
        }
        public byte GetByte(int i) { return (byte)mData[mCurrentLocation][i]; }
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new Exception("GetBytes is not supported");
        }
        public char GetChar(int i)
        {
            return (char)mData[mCurrentLocation][i];
        }
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new Exception("GetChars is not supported");
        }
        //this is not supported in SqlDatareader anyways
        public IDataReader GetData(int i)
        {
            throw new Exception("Method GetData is not implemented");
        }
        public string GetDataTypeName(int i)
        {
            return mData[mCurrentLocation][i].GetType().ToString();
        }
        public DateTime GetDateTime(int i)
        {
            return (DateTime)mData[mCurrentLocation][i];
        }
        public decimal GetDecimal(int i)
        {
            return (decimal)mData[mCurrentLocation][i];
        }
        public double GetDouble(int i)
        {
            return (double)mData[mCurrentLocation][i];
        }
        public Type GetFieldType(int i)
        {
            return mData[mCurrentLocation][i].GetType();
        }
        public float GetFloat(int i)
        {
            return (float)mData[mCurrentLocation][i];
        }
        public Guid GetGuid(int i)
        {
            return (Guid)mData[mCurrentLocation][i];
        }
        public short GetInt16(int i)
        {
            return (short)mData[mCurrentLocation][i];
        }
        public int GetInt32(int i)
        {
            return (int)mData[mCurrentLocation][i];
        }
        public long GetInt64(int i)
        {
            return (long)mData[mCurrentLocation][i];
        }
        public string GetName(int i)
        {
            return mSchemaTable.Rows[i][SchemaTableColumn.ColumnName].ToString();
        }
        public int GetOrdinal(string name)
        {
            for (int i = 0; i < mSchemaTable.Rows.Count; i++)
            {
                if (name == mSchemaTable.Rows[i][SchemaTableColumn.ColumnName].ToString())
                    return i;
            }
            return -1;
        }
        public string GetString(int i)
        {
            return (string)mData[mCurrentLocation][i];
        }
        public object GetValue(int i)
        {
            return mData[mCurrentLocation][i];
        }
        public int GetValues(object[] values)
        {
            int maxAllowableLength = Math.Min(values.Length, mData[mCurrentLocation].Length);
            for (int i = 0; i < maxAllowableLength; i++)
                values[i] = mData[mCurrentLocation][i];
            return maxAllowableLength;
        }
        public bool IsDBNull(int i)
        {
            return this[i] is DBNull;
        }
        public int FieldCount { get { return mSchemaTable.Rows.Count; } }
        #endregion
    }
}
