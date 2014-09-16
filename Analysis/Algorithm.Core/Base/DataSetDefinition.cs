using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Fasterflect;

namespace Algorithm.Core
{
    public class DataColumnDefinition
    {
        string mDataTypeName;
        int mSize;
        public string Name;
        public int Ordinal;
        public int Size
        {
            get { return mSize; }
            set { mSize = value > 8000 ? -1 : value; }
        }
        public short Precision = 255;
        public short Scale = 255;
        public bool Nullable = true;
        public Type DataType
        {
            get
            {
                Type ret;
                switch (SQLDBType)
                {
                    case SqlDbType.BigInt: ret = typeof(long); break;
                    case SqlDbType.Binary: ret = typeof(byte[]); break;
                    case SqlDbType.Bit: ret = typeof(bool); break;
                    case SqlDbType.Char: ret = typeof(string); break;
                    case SqlDbType.Date: ret = typeof(DateTime); break;
                    case SqlDbType.DateTime: ret = typeof(DateTime); break;
                    case SqlDbType.DateTime2: ret = typeof(DateTime); break;
                    case SqlDbType.DateTimeOffset: ret = typeof(DateTimeOffset); break;
                    case SqlDbType.Decimal: ret = typeof(decimal); break;
                    case SqlDbType.Float: ret = typeof(double); break;
                    case SqlDbType.Image: ret = typeof(byte[]); break;
                    case SqlDbType.Int: ret = typeof(int); break;
                    case SqlDbType.Money: ret = typeof(decimal); break;
                    case SqlDbType.NChar: ret = typeof(string); break;
                    case SqlDbType.NText: ret = typeof(string); break;
                    case SqlDbType.NVarChar: ret = typeof(string); break;
                    case SqlDbType.Real: ret = typeof(Single); break;
                    case SqlDbType.SmallDateTime: ret = typeof(DateTime); break;
                    case SqlDbType.SmallInt: ret = typeof(Int16); break;
                    case SqlDbType.SmallMoney: ret = typeof(decimal); break;
                    case SqlDbType.Text: ret = typeof(string); break;
                    case SqlDbType.Time: ret = typeof(DateTime); break;
                    case SqlDbType.Timestamp: ret = typeof(byte[]); break;
                    case SqlDbType.TinyInt: ret = typeof(byte); break;
                    case SqlDbType.UniqueIdentifier: ret = typeof(Guid); break;
                    case SqlDbType.VarBinary: ret = typeof(byte[]); break;
                    case SqlDbType.VarChar: ret = typeof(string); break;
                    case SqlDbType.Xml: ret = typeof(string); break;
                    default:
                        ret = typeof(byte[]);
                        break;
                }
                return ret;
            }
        }
        public string DataTypeName
        {
            get { return mDataTypeName; }
            set { mDataTypeName = value.ToLower(); }
        }
        public Type SQLType
        {
            get
            {
                Type ret;
                switch (SQLDBType)
                {
                    case SqlDbType.BigInt: ret = typeof(System.Data.SqlTypes.SqlInt64); break;
                    case SqlDbType.Binary: ret = typeof(System.Data.SqlTypes.SqlBinary); break;
                    case SqlDbType.Bit: ret = typeof(System.Data.SqlTypes.SqlBoolean); break;
                    case SqlDbType.Char: ret = typeof(System.Data.SqlTypes.SqlString); break;
                    case SqlDbType.Date: ret = typeof(System.Data.SqlTypes.SqlDateTime); break;
                    case SqlDbType.DateTime: ret = typeof(System.Data.SqlTypes.SqlDateTime); break;
                    case SqlDbType.DateTime2: ret = typeof(System.Data.SqlTypes.SqlDateTime); break;
                    case SqlDbType.DateTimeOffset: ret = typeof(System.Data.SqlTypes.SqlDateTime); break;
                    case SqlDbType.Decimal: ret = typeof(System.Data.SqlTypes.SqlDecimal); break;
                    case SqlDbType.Float: ret = typeof(System.Data.SqlTypes.SqlDouble); break;
                    case SqlDbType.Image: ret = typeof(System.Data.SqlTypes.SqlBytes); break;
                    case SqlDbType.Int: ret = typeof(System.Data.SqlTypes.SqlInt32); break;
                    case SqlDbType.Money: ret = typeof(System.Data.SqlTypes.SqlMoney); break;
                    case SqlDbType.NChar: ret = typeof(System.Data.SqlTypes.SqlString); break;
                    case SqlDbType.NText: ret = typeof(System.Data.SqlTypes.SqlChars); break;
                    case SqlDbType.NVarChar: ret = Size == -1 ? typeof(System.Data.SqlTypes.SqlChars) : typeof(System.Data.SqlTypes.SqlString); break;
                    case SqlDbType.Real: ret = typeof(System.Data.SqlTypes.SqlSingle); break;
                    case SqlDbType.SmallDateTime: ret = typeof(System.Data.SqlTypes.SqlDateTime); break;
                    case SqlDbType.SmallInt: ret = typeof(System.Data.SqlTypes.SqlInt16); break;
                    case SqlDbType.SmallMoney: ret = typeof(System.Data.SqlTypes.SqlMoney); break;
                    case SqlDbType.Text: ret = typeof(System.Data.SqlTypes.SqlChars); break;
                    case SqlDbType.Time: ret = typeof(System.Data.SqlTypes.SqlDateTime); break;
                    case SqlDbType.Timestamp: ret = typeof(System.Data.SqlTypes.SqlBinary); break;
                    case SqlDbType.TinyInt: ret = typeof(System.Data.SqlTypes.SqlByte); break;
                    case SqlDbType.UniqueIdentifier: ret = typeof(System.Data.SqlTypes.SqlGuid); break;
                    case SqlDbType.VarBinary: ret = Size == -1 ? typeof(System.Data.SqlTypes.SqlBytes) : typeof(System.Data.SqlTypes.SqlBinary); break;
                    case SqlDbType.VarChar: ret = Size == -1 ? typeof(System.Data.SqlTypes.SqlChars) : typeof(System.Data.SqlTypes.SqlString); break;
                    case SqlDbType.Xml: ret = typeof(System.Data.SqlTypes.SqlXml); break;
                    default:
                        ret = typeof(System.Data.SqlTypes.SqlBytes);
                        break;
                }
                return ret;
            }
        }
        public SqlDbType SQLDBType
        {
            get
            {
                SqlDbType mSQLDBType;
                if (!Enum.TryParse<SqlDbType>(mDataTypeName, true, out mSQLDBType))
                {
                    mSQLDBType = SqlDbType.VarBinary;
                    Size = -1;
                }
                return mSQLDBType;
            }
        }
        public string SQLDefinition
        {
            get
            {
                string ret = "";
                switch (SQLDBType)
                {

                    case SqlDbType.Char:
                    case SqlDbType.NChar:
                    case SqlDbType.Binary:
                        ret = SQLDBType.ToString().ToLower() + "(" + Size.ToString() + ")"; break;
                    case SqlDbType.Decimal:
                        ret = SQLDBType.ToString().ToLower() + "(" + Precision.ToString() + "," + Scale.ToString() + ")"; break;
                    case SqlDbType.DateTime2:
                    case SqlDbType.DateTimeOffset:
                    case SqlDbType.Time:
                        ret = SQLDBType.ToString().ToLower() + "(" + Scale.ToString() + ")"; break;
                    case SqlDbType.VarChar:
                    case SqlDbType.NVarChar:
                    case SqlDbType.VarBinary:
                        ret = SQLDBType.ToString().ToLower() + "(" + (Size == -1 ? "max" : Size.ToString()) + ")"; break;
                    case SqlDbType.Timestamp:
                        ret = "binary(8)"; break;
                    default:
                        ret = SQLDBType.ToString().ToLower();
                        break;
                }
                ret = "[" + Name + "] " + ret;
                if (!Nullable)
                    ret = ret + " not null";
                return ret;
            }
        }
        public SqlParameter ParameterDefinition
        {
            get
            {
                SqlParameter ret = new SqlParameter("@" + Name.GetSQLName(), SQLDBType);
                switch (SQLDBType)
                {
                    case SqlDbType.Char:
                    case SqlDbType.NChar:
                    case SqlDbType.Binary:
                    case SqlDbType.VarChar:
                    case SqlDbType.NVarChar:
                    case SqlDbType.VarBinary:
                        ret.Size = Size; break;
                    case SqlDbType.Decimal:
                        ret.Precision = (byte)Precision; ret.Scale = (byte)Scale; break;
                    case SqlDbType.DateTime2:
                    case SqlDbType.DateTimeOffset:
                    case SqlDbType.Time:
                        ret.Scale = (byte)Scale; break;
                    case SqlDbType.Timestamp:
                        ret.SqlDbType = SqlDbType.Binary; ret.Size = 8; break;
                    default:
                        break;
                }
                return ret;
            }
        }
        public SqlMetaData SqlMetaData
        {
            get
            {
                SqlMetaData m = null;
                switch (SQLDBType)
                {
                    case SqlDbType.Char:
                    case SqlDbType.NChar:
                    case SqlDbType.Binary:
                    case SqlDbType.VarChar:
                    case SqlDbType.NVarChar:
                    case SqlDbType.VarBinary:
                        m = new SqlMetaData(Name, SQLDBType, Size); break;
                    case SqlDbType.Decimal:
                        m = new SqlMetaData(Name, SqlDbType.Decimal, (byte)Precision, (byte)Scale); break;
                    case SqlDbType.DateTime2:
                    case SqlDbType.DateTimeOffset:
                    case SqlDbType.Time:
                        m = new SqlMetaData(Name, SQLDBType, (byte)Scale); break;
                    case SqlDbType.Timestamp:
                        m = new SqlMetaData(Name, SqlDbType.Binary, 8); break;
                    case SqlDbType.Text:
                        m = new SqlMetaData(Name, SqlDbType.VarChar, -1); break;
                    case SqlDbType.NText:
                        m = new SqlMetaData(Name, SqlDbType.NVarChar, -1); break;
                    case SqlDbType.Image:
                        m = new SqlMetaData(Name, SqlDbType.VarBinary, -1); break;
                    default:
                        m = new SqlMetaData(Name, SQLDBType);
                        break;
                }
                return m;
            }
        }
        public object Tag = null;
        public bool IsPrimaryKey = false;
        public bool IsLong
        {
            get
            {
                SqlDbType t = SQLDBType;
                return mSize == -1 || t == SqlDbType.Xml || t == SqlDbType.Text || t == SqlDbType.Image;
            }
        }
        public DataSetDefinition Table;
        public MemberGetter ValueGetter = null;
        public object GetterObject = null;
        public object GetValue()
        {
            return ValueGetter(GetterObject);
        }
    }
    public class DataSetDefinition
    {
        DataTable mSchemaTable;
        public int ColumnCount
        {
            get { return Columns.Count; }
        }
        public List<DataColumnDefinition> Columns = new List<DataColumnDefinition>();

        public void ResetColumnsWithSchemaTable(DataTable schemaTable)
        {
            mSchemaTable.Rows.Clear();
            foreach (DataRow x in schemaTable.Rows)
            {
                DataColumnDefinition item = new DataColumnDefinition()
                {
                    Name = (string)x[SchemaTableColumn.ColumnName],
                    Ordinal = (int)x[SchemaTableColumn.ColumnOrdinal],
                    Size = (int)x[SchemaTableColumn.ColumnSize],
                    Precision = (short)x[SchemaTableColumn.NumericPrecision],
                    Scale = (short)x[SchemaTableColumn.NumericScale],
                    Nullable = (bool)x[SchemaTableColumn.AllowDBNull],
                    DataTypeName = (string)x["DataTypeName"].ToString(),
                    Table = this
                };
                Columns.Add(item);
            }
            ResetColumnOrdinal();
        }
        public DataSetDefinition()
        {
            CreateSchemaTable();
        }
        void CreateSchemaTable()
        {
            mSchemaTable = new DataTable("SchemaTable");
            mSchemaTable.Clear();
            mSchemaTable.Columns.Clear();
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.ColumnName, typeof(string)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.ColumnOrdinal, typeof(int)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.ColumnSize, typeof(int)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.NumericPrecision, typeof(short)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.NumericScale, typeof(short)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.DataType, typeof(Type)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.ProviderSpecificDataType, typeof(Type)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.NonVersionedProviderType, typeof(int)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.ProviderType, typeof(int)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.IsLong, typeof(bool)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.AllowDBNull, typeof(bool)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsReadOnly, typeof(bool)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsRowVersion, typeof(bool)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.IsUnique, typeof(bool)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.IsKey, typeof(bool)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsAutoIncrement, typeof(bool)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsHidden, typeof(bool)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.BaseCatalogName, typeof(string)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.BaseSchemaName, typeof(string)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.BaseTableName, typeof(string)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.BaseColumnName, typeof(string)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.BaseServerName, typeof(string)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.IsAliased, typeof(bool)));
            mSchemaTable.Columns.Add(new DataColumn(SchemaTableColumn.IsExpression, typeof(bool)));
            mSchemaTable.Columns.Add(new DataColumn("IsIdentity", typeof(bool)));
            mSchemaTable.Columns.Add(new DataColumn("DataTypeName", typeof(string)));
            mSchemaTable.Columns.Add(new DataColumn("UdtAssemblyQualifiedName", typeof(string)));
            mSchemaTable.Columns.Add(new DataColumn("XmlSchemaCollectionDatabase", typeof(string)));
            mSchemaTable.Columns.Add(new DataColumn("XmlSchemaCollectionOwningSchema", typeof(string)));
            mSchemaTable.Columns.Add(new DataColumn("XmlSchemaCollectionName", typeof(string)));
            mSchemaTable.Columns.Add(new DataColumn("IsColumnSet", typeof(bool)));
        }
        public void ResetColumnOrdinal()
        {
            for (int i = 0; i < Columns.Count; i++)
            {
                Columns[i].Ordinal = i;
            }
        }
        public DataTable GetSchemaTable()
        {
            DataColumnDefinition c;
            ResetColumnOrdinal();
            mSchemaTable.Rows.Clear();
            mSchemaTable.AcceptChanges();
            for (int i = 0; i < Columns.Count; i++)
            {
                c = Columns[i];
                DataRow r = mSchemaTable.NewRow();
                r[SchemaTableColumn.ColumnName] = c.Name;
                r[SchemaTableColumn.ColumnOrdinal] = c.Ordinal;
                r[SchemaTableColumn.ColumnSize] = c.Size;
                r[SchemaTableColumn.NumericPrecision] = c.Precision;
                r[SchemaTableColumn.NumericScale] = c.Scale;
                r[SchemaTableColumn.DataType] = c.DataType;
                r[SchemaTableOptionalColumn.ProviderSpecificDataType] = c.SQLType;
                r[SchemaTableColumn.NonVersionedProviderType] = 12;
                r[SchemaTableColumn.ProviderType] = 12;
                r[SchemaTableColumn.IsLong] = c.IsLong;
                r[SchemaTableColumn.AllowDBNull] = c.Nullable;
                r[SchemaTableOptionalColumn.IsReadOnly] = true;
                r[SchemaTableOptionalColumn.IsRowVersion] = false;
                r[SchemaTableColumn.IsUnique] = false;
                r[SchemaTableColumn.IsKey] = false;
                r[SchemaTableOptionalColumn.IsAutoIncrement] = false;
                r[SchemaTableOptionalColumn.IsHidden] = false;
                r[SchemaTableOptionalColumn.BaseCatalogName] = DBNull.Value;
                r[SchemaTableColumn.BaseSchemaName] = DBNull.Value;
                r[SchemaTableColumn.BaseTableName] = DBNull.Value;
                r[SchemaTableColumn.BaseColumnName] = c.Name;
                r[SchemaTableOptionalColumn.BaseServerName] = DBNull.Value;
                r[SchemaTableColumn.IsAliased] = false;
                r[SchemaTableColumn.IsExpression] = false;
                r["IsIdentity"] = false;
                r["DataTypeName"] = c.DataTypeName;
                r["UdtAssemblyQualifiedName"] = DBNull.Value;
                r["XmlSchemaCollectionDatabase"] = DBNull.Value;
                r["XmlSchemaCollectionOwningSchema"] = DBNull.Value;
                r["XmlSchemaCollectionName"] = DBNull.Value;
                r["IsColumnSet"] = false;
                mSchemaTable.Rows.Add(r);
                r.AcceptChanges();
            }
            return mSchemaTable;
        }
        public SqlParameter[] GetParameter()
        {
            lock (this)
            {
                return Columns.Select(x => x.ParameterDefinition).ToArray();
            }
        }
        public SqlMetaData[] GetMetaData()
        {
            lock (this)
            {
                return Columns.Select(x => x.SqlMetaData).ToArray();
            }
        }
        public SqlDataRecord GetDataRecord()
        {
            lock (this)
            {
                return new SqlDataRecord(GetMetaData());
            }
        }
    }
}
