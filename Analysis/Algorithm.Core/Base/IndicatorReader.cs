using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;
namespace Algorithm.Core
{
    public class IndicatorReader: IDataReader
    {
        long rowNumber = 0;
        DataSetDefinition mDatasetDefinition = null;
        bool mIsClosed = true;
        public Indicator mIndicator;
        public Indicator Indicator
        {
            get
            {
                return mIndicator;
            }
            set 
            {
                if (mIndicator != value)
                {
                    mIndicator = value;
                    GetDataSetDefinition();
                    mIsClosed = false;
                }
            }
        }
        public DataSetDefinition DataSetDefinition { get { return mDatasetDefinition; } }
        void GetDataSetDefinition()
        {
            mDatasetDefinition = new DataSetDefinition();
            mDatasetDefinition.Columns.Add(new DataColumnDefinition() { Name = "___RowNumber___", DataTypeName = "bigint", Table = mDatasetDefinition, IsPrimaryKey = true });
            DataColumnDefinition c = new DataColumnDefinition()
            {
                Name = "___Return___",
                DataTypeName = "float",
                Table = mDatasetDefinition,
                GetterObject = mIndicator,
                ValueGetter = mIndicator.GetType().DelegateForGetPropertyValue("Value")
            };
            mDatasetDefinition.Columns.Add(c);
            Type type = mIndicator.GetType();
            OutputAttribute output = null;
            foreach (var p in type.GetProperties())
            {
                if (p.PropertyType.IsWindow())
                {
                    output = (OutputAttribute)p.GetCustomAttributes(true).FirstOrDefault(x => x is OutputAttribute);
                    if (output != null)
                    {
                        c = new DataColumnDefinition()
                                    {
                                        Name = string.IsNullOrEmpty(output.Name)? p.Name : output.Name,
                                        PropertyName = p.Name,
                                        DataTypeName = (p.PropertyType == typeof(Window<double>)) || (p.PropertyType.IsSubclassOf(typeof(Window<double>))) ? "real" : ((p.PropertyType == typeof(Window<DateTime>))|| (p.PropertyType.IsSubclassOf(typeof(Window<DateTime>))) ? "DateTime" : "Int"),
                                        Table = mDatasetDefinition
                                    };
                        c.GetterObject = mIndicator.GetPropertyValue(p.Name);
                        if (c.GetterObject != null)
                            c.ValueGetter = c.GetterObject.GetType().DelegateForGetPropertyValue("Value");
                        mDatasetDefinition.Columns.Add(c);
                    }
                }
            }
        }
        public bool Read()
        {
            bool ret = mIndicator.Read();
            if (rowNumber == 0)
            {
                foreach(var c in mDatasetDefinition.Columns)
                {
                    if (c.PropertyName == "")
                        continue;
                    if (c.PropertyName == null)
                        continue;
                    if (c.GetterObject != null)
                        continue;
                    c.GetterObject = mIndicator.TryGetPropertyValue(c.PropertyName);
                    c.ValueGetter = c.GetterObject.GetType().DelegateForGetPropertyValue("Value");
                }
            }
            rowNumber++;
            return ret;
        }

        public void Close()
        {
            mIsClosed = true;
        }

        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public DataTable GetSchemaTable()
        {
            return mDatasetDefinition.GetSchemaTable();
        }

        public bool IsClosed
        {
            get { return mIsClosed; }
        }

        public bool NextResult()
        {
            return false;
        }

        public int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            return;
        }

        public int FieldCount
        {
            get { return mDatasetDefinition.ColumnCount; }
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            return mDatasetDefinition.Columns[i].DataTypeName;
        }

        public DateTime GetDateTime(int i)
        {
            return Convert.ToDateTime(this[i]);
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            return Convert.ToDouble(this[i]);
        }

        public Type GetFieldType(int i)
        {
            return mDatasetDefinition.Columns[i].DataType;
        }

        public float GetFloat(int i)
        {
            return Convert.ToSingle(this[i]);
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            return Convert.ToInt16(this[i]);
        }

        public int GetInt32(int i)
        {
            return Convert.ToInt32(this[i]);
        }

        public long GetInt64(int i)
        {
            return Convert.ToInt64(this[i]);
        }

        public string GetName(int i)
        {
            return mDatasetDefinition.Columns[i].Name;
        }

        public int GetOrdinal(string name)
        {
            for (int i = 0; i < mDatasetDefinition.Columns.Count; i++ )
            {
                if (mDatasetDefinition.Columns[i].Name == name)
                    return i;
            }
            throw new Exception(name + " could not be found.");
        }

        public string GetString(int i)
        {
            return Convert.ToString(this[i]);
        }

        public object GetValue(int i)
        {
            return this[i];
        }

        public int GetValues(object[] values)
        {
            int ret = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (i < FieldCount)
                {
                    values[i] = this[i];
                    ret++;
                }
            }
            return ret;
        }

        public bool IsDBNull(int i)
        {
            return false;
        }

        public object this[string name]
        {
            get { return this[GetOrdinal(name)]; }
        }

        public object this[int i]
        {
            get { return i == 0 ? rowNumber : mDatasetDefinition.Columns[i].GetValue(); }
        }
    }
}
