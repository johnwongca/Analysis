using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Base.Data;

namespace Base
{
    
    namespace Controls
    {
        public class DataSourceException : Exception
        {
            CDataSource m_dataSource;
            CDataSource DataSource
            {
                get { return m_dataSource; }
            }
            public DataSourceException(CDataSource dataSource) : base("Could not set the property on an opened data source.") 
            {
                m_dataSource = dataSource;
            }
            public DataSourceException(CDataSource dataSource, string message) : base(message) 
            {
                m_dataSource = dataSource;
            }
        }
        [DesignTimeVisible(false)]
        public class MasterDetailLink : MarshalByValueComponent
        {
            string m_masterField;
            string m_detailField;
            public string MasterField
            {
                get { return m_masterField; }
				set 
				{ 
					m_masterField = value;
					OnMemberChanged(); 
				}
            }
            public string DetailField
            {
                get { return m_detailField; }
				set 
				{ 
					m_detailField = value;
					OnMemberChanged(); 
				}
            }
			public event EventHandler MemberChanged;
			protected void OnMemberChanged()
			{
				if (MemberChanged != null)
					MemberChanged(this, new EventArgs());
			}
        }
		public class MasterDetailLinkCollection : List<MasterDetailLink>
		{
			CDataSource m_owner;
            public CDataSource Owner { get { return m_owner; } }
			public MasterDetailLinkCollection(CDataSource owner)
            {
                m_owner = owner;
            }
            public new void Clear()
            {
                base.Clear();
				OnMemberChanged();
            }
			public new void Insert(int index, MasterDetailLink item)
            {
                base.Insert(index, item);
				OnMemberChanged();
            }
			public new void InsertRange(int index, IEnumerable<MasterDetailLink> collection)
            {
                base.InsertRange(index, collection);
				OnMemberChanged();
            }
			public new bool Remove(MasterDetailLink item)
            {
				bool ret = base.Remove(item);
				OnMemberChanged();
                return ret;
				
            }
            public new void RemoveAt(int index)
            {
                base.RemoveAt(index);
				OnMemberChanged();
            }
            public new void RemoveRange(int index, int count)
            {
                base.RemoveRange(index, count);
				OnMemberChanged();
            }
            public new void Reverse()
            {
                base.Reverse();
            }
            public new void Reverse(int index, int count)
            {
                base.Reverse(index, count);
				OnMemberChanged();
            }
			public new void Add(MasterDetailLink item)
            {
                base.Add(item);
				OnMemberChanged();
            }
			public new void AddRange(IEnumerable<MasterDetailLink> collection)
            {
                base.AddRange(collection);
				OnMemberChanged();
            }
			public event EventHandler MemberChanged;
			protected void OnMemberChanged()
			{
				if(MemberChanged!=null)
					MemberChanged(this, new EventArgs());
			}
		}
        public enum FieldType
        {
            Normal, Calculated, Lookup
        }
        [DesignTimeVisible(false)]
        public class Field : MarshalByValueComponent
        {
             
            DataColumn m_dataColumn = null;
            bool m_autoIncrement, m_nullable, m_readOnly;
            long m_autoIncrementSeed;
            int m_autoIncrementStep, m_length, m_precision, m_scale;
            string m_caption, m_fieldName, m_lookupFieldName, m_LookupKeys;
            object m_defaultValue = null;
            internal int m_ordinal;
            CDataSource m_lookupDataSource;
            SqlDbType m_dataType;
            FieldType m_fieldType;
            Type m_type, m_systemType;
            internal CDataSource m_owner;
            private void CheckActive()
            {
                if (m_owner == null)
                    return;
                if (m_owner.Active)
                    throw new DataSourceException(Owner, "Could not change field definition while buffer is active.");
            }
            private void CheckType()
            {
                switch (this.DataType)
                {
                    case SqlDbType.BigInt:
                        m_type = typeof(System.Data.SqlTypes.SqlInt64);
                        m_systemType = typeof(System.Int64);
                        m_length = -1;
                        break;
                    case SqlDbType.Binary:
                        m_type = typeof(System.Data.SqlTypes.SqlBinary);
                        m_systemType = typeof(System.Byte[]);
                        break;
                    case SqlDbType.Bit:
                        m_type = typeof(System.Data.SqlTypes.SqlBoolean);
                        m_systemType = typeof(System.Boolean);
                        m_length = -1;
                        break;
                    case SqlDbType.Char:
                        m_type = typeof(System.Data.SqlTypes.SqlChars);
                        m_systemType = typeof(System.String);
                        break;
                    case SqlDbType.DateTime:
                        m_type = typeof(System.Data.SqlTypes.SqlDateTime);
                        m_systemType = typeof(System.DateTime);
                        m_length = -1;
                        break;
                    case SqlDbType.Decimal:
                        m_type = typeof(System.Data.SqlTypes.SqlDecimal);
                        m_systemType = typeof(System.Decimal);
                        m_length = -1;
                        break;
                    case SqlDbType.Float:
                        m_type = typeof(System.Data.SqlTypes.SqlDouble);
                        m_systemType = typeof(System.Double);
                        m_length = -1;
                        break;
                    case SqlDbType.Image:
                        m_type = typeof(System.Data.SqlTypes.SqlBinary);
                        m_systemType = typeof(System.Byte[]);
                        m_length = System.Int32.MaxValue;
                        break;
                    case SqlDbType.Int:
                        m_type = typeof(System.Data.SqlTypes.SqlInt32);
                        m_systemType = typeof(System.Int32);
                        m_length = -1;
                        break;
                    case SqlDbType.Money:
                        m_type = typeof(System.Data.SqlTypes.SqlMoney);
                        m_systemType = typeof(System.Decimal);
                        m_length = -1;
                        break;
                    case SqlDbType.NChar:
                        m_type = typeof(System.Data.SqlTypes.SqlString);
                        m_systemType = typeof(System.String);
                        break;
                    case SqlDbType.NText:
                        m_type = typeof(System.Data.SqlTypes.SqlString);
                        m_systemType = typeof(System.String);
                        break;
                    case SqlDbType.NVarChar:
                        m_type = typeof(System.Data.SqlTypes.SqlString);
                        m_systemType = typeof(System.String);
                        break;
                    case SqlDbType.Real:
                        m_type = typeof(System.Data.SqlTypes.SqlSingle);
                        m_systemType = typeof(System.Single);
                        m_length = -1;
                        break;
                    case SqlDbType.SmallDateTime:
                        m_type = typeof(System.Data.SqlTypes.SqlDateTime);
                        m_systemType = typeof(System.DateTime);
                        m_length = -1;
                        break;
                    case SqlDbType.SmallInt:
                        m_type = typeof(System.Data.SqlTypes.SqlInt16);
                        m_systemType = typeof(System.Int16);
                        m_length = -1;
                        break;
                    case SqlDbType.SmallMoney:
                        m_type = typeof(System.Data.SqlTypes.SqlMoney);
                        m_systemType = typeof(System.Decimal);
                        m_length = -1;
                        break;
                    case SqlDbType.Text:
                        m_type = typeof(System.Data.SqlTypes.SqlString);
                        m_systemType = typeof(System.String);
                        m_length = System.Int32.MaxValue;
                        break;
                    case SqlDbType.Timestamp:
                        m_type = typeof(System.Data.SqlTypes.SqlBinary);
                        m_systemType = typeof(System.Byte[]);
                        break;
                    case SqlDbType.TinyInt:
                        m_type = typeof(System.Data.SqlTypes.SqlByte);
                        m_systemType = typeof(System.Byte);
                        m_length = -1;
                        break;
                    case SqlDbType.Udt:
                        m_type = typeof(System.Data.SqlTypes.SqlString);
                        m_systemType = typeof(System.String);
                        m_length = System.Int32.MaxValue;
                        break;
                    case SqlDbType.UniqueIdentifier:
                        m_type = typeof(System.Data.SqlTypes.SqlGuid);
                        m_systemType = typeof(System.Guid);
                        m_length = -1;
                        break;
                    case SqlDbType.VarBinary:
                        m_type = typeof(System.Data.SqlTypes.SqlBinary);
                        m_systemType = typeof(System.Byte[]);
                        break;
                    case SqlDbType.VarChar:
                        m_type = typeof(System.Data.SqlTypes.SqlString);
                        m_systemType = typeof(System.String);
                        break;
                    case SqlDbType.Variant:
                        m_type = typeof(object);
                        m_length = -1;
                        break;
                    case SqlDbType.Xml:
                        m_type = typeof(System.Data.SqlTypes.SqlXml);
                        m_systemType = typeof(System.String);
                        m_length = -1;
                        break;
                }

            }
            public bool AutoIncrement
            {
                get { return m_autoIncrement; }
                set
                {
                    CheckActive();
                    m_autoIncrement = value;
                }
            }
            public long AutoIncrementSeed
            {
                get { return m_autoIncrementSeed; }
                set
                {
                    CheckActive();
                    m_autoIncrementSeed = value;
                }
            }
            public int AutoIncrementStep
            {
                get { return m_autoIncrementStep; }
                set
                {
                    CheckActive();
                    m_autoIncrementStep = value == 0 ? 1 : value;
                }
            }
            public bool Nullable
            {
                get { return m_nullable; }
                set
                {
                    CheckActive();
                    m_nullable = value;
                }
            }
            public SqlDbType DataType
            {
                get { return m_dataType; }
                set
                {
                    CheckActive();
                    m_dataType = value;
                    CheckType();
                }
            }
            public FieldType FieldType
            {
                get { return m_fieldType; }
                set
                {
                    CheckActive();
                    m_fieldType = value;
                    DataType = this.m_dataType;
                }
            }
            public int Length
            {
                get { return m_length; }
                set
                {
                    CheckActive();
                    m_length = value;
                    CheckType();
                }
            }
            public int Precision
            {
                get { return m_precision; }
                set
                {
                    CheckActive();
                    m_precision = value;
                }
            }
            public int Scale
            {
                get { return m_scale; }
                set
                {
                    CheckActive();
                    m_scale = value;
                }
            }
            public bool ReadOnly
            {
                get { return m_readOnly || m_autoIncrement || FieldType == FieldType.Calculated || FieldType == FieldType.Lookup; }
                set
                {
                    m_readOnly = value;
                }
            }
            public string Caption
            {
                get { return m_caption; }
                set
                {
                    m_caption = value;
                }
            }
            public string FieldName
            {
                get { return m_fieldName; }
                set
                {
                    CheckActive();
                    m_fieldName = value;
                    if (Caption == "")
                        Caption = m_fieldName;
                }
            }
            public string LookupKeys
            {
                get { return m_LookupKeys; }
                set { m_LookupKeys = value; }
            }
            public string LookupFieldName
            {
                get { return m_lookupFieldName; }
                set { m_lookupFieldName = value; }
            }
            public int Ordinal
            {
                get { return m_ordinal; }
            }
            public object DefaultValue
            {
                get 
                {
                    return m_defaultValue; 
                }
                set
                {
                    if(Owner!=null)
                        if (Owner.Active)
                            m_dataColumn.DefaultValue = value;
                    m_defaultValue = value;
                }
            }
            public Type SystemType
            {
                get { return m_systemType; }
            }
            [Browsable(false)]
            public Type Type
            {
                get 
                { 
                    if(m_owner != null)
                        if (!m_owner.Active) 
                            CheckType(); 
                    return m_type; 
                }
            }
            [Browsable(false)]
            public CDataSource Owner
            {
                get { return m_owner; }
            }
            public CDataSource LookupDataSource
            {
                get { return m_lookupDataSource; }
                set { m_lookupDataSource = value; }
            }
            internal DataColumn DataColumn
            {
                get { return m_dataColumn; }
                set { m_dataColumn = value; }
            }
            public Field()
            {
                m_autoIncrementStep = 1;
                m_dataType = SqlDbType.VarChar;
                m_length = 50;
                m_nullable = true;
                m_fieldType = FieldType.Normal;
                m_ordinal = 0;
                m_owner = null;
            }
            public Field(CDataSource owner)
            {
                m_autoIncrementStep = 1;
                DataType = SqlDbType.VarChar;
                Length = 50;
                Nullable = true;
                FieldType = FieldType.Normal;
                m_ordinal = 0;
                m_owner = owner;
            }
            public Field(CDataSource owner, string fieldName)
                : this(owner)
            {
                FieldName = fieldName;
            }
            public Field(CDataSource owner, string fieldName, SqlDbType type)
                : this(owner, fieldName)
            {
                DataType = type;
            }
            public Field(CDataSource owner, string fieldName, SqlDbType type, int length)
                : this(owner, fieldName, type)
            {
                Length = length;
            }
            public Field(CDataSource owner, string fieldName, SqlDbType type, int precision, int scale)
                : this(owner, fieldName, type)
            {
                Precision = precision;
                Scale = scale;
            }
            public override string ToString()
            {
                return FieldName;
            }
            internal DataColumn GetDataColumn()
            {
                DataColumn ret = new DataColumn();
                m_dataColumn = ret;
                ret.AllowDBNull = this.Nullable;
                ret.AutoIncrement = this.AutoIncrement;
                ret.AutoIncrementSeed = this.AutoIncrementSeed;
                ret.AutoIncrementStep = this.AutoIncrementStep;
                ret.Caption = this.Caption;
                ret.ColumnName = this.FieldName;
                ret.DataType = this.SystemType;
                ret.MaxLength = this.Length;
                ret.DefaultValue = this.DefaultValue;
                return ret;
            }
        }
        public class FieldCollection : List<Field>
        {
            CDataSource m_owner;
            public CDataSource Owner { get { return m_owner; } }
            public FieldCollection(CDataSource owner)
            {
                m_owner = owner;
            }
            private void SetFieldOridinal()
            {
                for (int i = 0; i < Count; i++)
                    ((Field)(this[i])).m_ordinal = i;
            }
            private void CheckActive()
            {
                if (m_owner.Active)
                    throw new DataSourceException(Owner, "Could not change FieldCollection while buffer is active.");
            }
            public new void Clear()
            {
                CheckActive();
                base.Clear();
            }
            public new void Insert(int index, Field item)
            {
                CheckActive();
                base.Insert(index, item);
            }
            public new void InsertRange(int index, IEnumerable<Field> collection)
            {
                CheckActive();
                base.InsertRange(index, collection);
            }
            public new bool Remove(Field item)
            {
                CheckActive();
                return base.Remove(item);
            }
            public new void RemoveAt(int index)
            {
                CheckActive();
                base.RemoveAt(index);
            }
            public new void RemoveRange(int index, int count)
            {
                CheckActive();
                base.RemoveRange(index, count);
            }
            public new void Reverse()
            {
                CheckActive();
                base.Reverse();
            }
            public new void Reverse(int index, int count)
            {
                CheckActive();
                base.Reverse(index, count);
            }
            public new void Add(Field item)
            {
                CheckActive();
                item.m_owner = m_owner;
                base.Add(item);
                SetFieldOridinal();
            }
            public Field Add()
            {
                CheckActive();
                Field f = new Field(m_owner);
                base.Add(f);
                SetFieldOridinal();
                return f;
            }
            public Field Add(string fieldName)
            {
                CheckActive();
                Field f = new Field(m_owner, fieldName);
                base.Add(f);
                SetFieldOridinal();
                return f;
            }
            public Field Add(string fieldName, SqlDbType type)
            {
                CheckActive();
                Field f = new Field(m_owner, fieldName, type);
                base.Add(f);
                SetFieldOridinal();
                return f;
            }
            public Field Add(string fieldName, SqlDbType type, int length)
            {
                CheckActive();
                Field f = new Field(m_owner, fieldName, type, length);
                base.Add(f);
                SetFieldOridinal();
                return f;
            }
            public Field Add(string fieldName, SqlDbType type, int precision, int scale)
            {
                CheckActive();
                Field f = new Field(m_owner, fieldName, type, precision, scale);
                base.Add(f);
                SetFieldOridinal();
                return f;
            }
            public new void AddRange(IEnumerable<Field> collection)
            {
                CheckActive();
                base.AddRange(collection);
            }
        }
        public class CDataSourceEventArgs : EventArgs
        {
            bool m_cancel;
            DataRow m_newValue, m_oldValue;
            public CDataSourceEventArgs():base()
            {
                m_cancel = false;
            }
            public CDataSourceEventArgs(DataRow newValue, DataRow oldValue): this()
            {
                NewValue = newValue;
                OldValue = oldValue;
            }
            public bool Cancel
            {
                get { return m_cancel; }
                set { m_cancel = value; }
            }
            public DataRow NewValue
            {
                get { return m_newValue; }
                set { m_newValue = value; }
            }
            public DataRow OldValue
            {
                get { return m_oldValue; }
                set { m_oldValue = value; }
            }
        }
        public delegate void CDataSourceEvent(object sender, CDataSourceEventArgs e);
        public class CDataSource : System.Windows.Forms.BindingSource
        {
            #region Private Members & methods
            CDataTable m_data;
            bool m_caseSensitive, m_allowNew, m_allowRemove, m_allowEdit;
            CDataSource m_masterSource;
            FieldCollection m_fields;
			MasterDetailLinkCollection m_masterFields;
			void m_data_ColumnChanged(object sender, DataColumnChangeEventArgs e)
			{
				if (ColumnChanged != null)
					ColumnChanged(this, e);
			}
			void m_data_ColumnChanging(object sender, DataColumnChangeEventArgs e)
			{
				if (ColumnChanging != null)
					ColumnChanging(this, e);
			}
			void m_data_RowChanged(object sender, DataRowChangeEventArgs e)
			{
				if (RowChanged != null)
					RowChanged(this, e);
			}
			void m_data_RowChanging(object sender, DataRowChangeEventArgs e)
			{
				if (RowChanging != null)
					RowChanging(this, e);
			}
			void m_data_RowDeleted(object sender, DataRowChangeEventArgs e)
			{
				if (RowDeleted != null)
					RowDeleted(this, e);
			}
			void m_data_RowDeleting(object sender, DataRowChangeEventArgs e)
			{
				if (RowDeleting != null)
					RowDeleting(this, e);
			}
			void m_data_TableCleared(object sender, DataTableClearEventArgs e)
			{
				if (TableCleared != null)
					TableCleared(this, e);
			}
			void m_data_TableClearing(object sender, DataTableClearEventArgs e)
			{
				if (TableClearing != null)
					TableClearing(this, e);
			}
			void m_data_TableNewRow(object sender, DataTableNewRowEventArgs e)
			{
				if (TableNewRow != null)
					TableNewRow(this, e);
			}
            #endregion

            #region Constructors
            private void InitializeComponent()
            {
				m_masterFields = new MasterDetailLinkCollection(this);
                m_fields = new FieldCollection(this);
            }
            public CDataSource(IContainer container)
                : base(container)
            {
				m_data = null;
                InitializeComponent();
            }
            public CDataSource()
                : base()
            {

                InitializeComponent();
            }
            protected override void Dispose(bool disposing)
            {
                Active = false;
                if (disposing)
                {
					foreach (Field f in m_fields)
						f.Dispose();
					m_fields.Clear();
					foreach (MasterDetailLink f in m_masterFields)
						f.Dispose();
					m_masterFields.Clear();
                }
                base.Dispose(disposing);
            }
            #endregion

            #region Protected Methods
            protected void CheckIsActive()
            {
				if (this.DesignMode)
					return;
                if (!Active)
                    throw new DataSourceException(this, "Could not apply this operation on an inactive data source.");
            }
            #endregion
            #region Public Methods
            
            public void Open()
            {
				if (Active)
					return;
                OnBeforeOpen();
                if (m_fields.Count == 0)
                    throw new DataSourceException(this, "Can not open a data source without field definition.");
                m_data = new CDataTable();
                m_data.CaseSensitive = CaseSensitive;
                foreach (Field f in m_fields)
                    m_data.Columns.Add(f.GetDataColumn());
                base.DataSource = m_data;
				m_data.ColumnChanged += new DataColumnChangeEventHandler(m_data_ColumnChanged);
				m_data.ColumnChanging +=new DataColumnChangeEventHandler(m_data_ColumnChanging);
				m_data.RowChanged += new DataRowChangeEventHandler(m_data_RowChanged);
				m_data.RowChanging += new DataRowChangeEventHandler(m_data_RowChanging);
				m_data.RowDeleted += new DataRowChangeEventHandler(m_data_RowDeleted);
				m_data.RowDeleting += new DataRowChangeEventHandler(m_data_RowDeleting);
				m_data.TableCleared += new DataTableClearEventHandler(m_data_TableCleared);
				m_data.TableClearing += new DataTableClearEventHandler(m_data_TableClearing);
				m_data.TableNewRow += new DataTableNewRowEventHandler(m_data_TableNewRow);
                OnAfterOpen();
            }
            public void Close()
            {
				if (!Active)
					return;
                OnBeforeClose();
                m_data.Dispose();
                m_data = null;
                GC.Collect();
                OnAfterClose();
            }
            public DataRow NewRow()
            {
                CheckIsActive();
                return Data.NewRow();
            }
            public int Add(DataRow value)
            {
                CheckIsActive();
                return base.Add(value);
            }
            public new DataRow AddNew()
            {
                CheckIsActive();
                return (DataRow)base.AddNew();
            }
            public new void CancelEdit()
            {
                CheckIsActive();
                base.CancelEdit();
            }
            public override void Clear()
            {
                CheckIsActive();
                base.Clear();
            }
            public override bool Contains(object value)
            {
                CheckIsActive();
                return base.Contains(value);
            }
            public bool Contains(DataRow value)
            {
                CheckIsActive();
                return base.Contains(value);
            }
            public override void CopyTo(Array arr, int index)
            {
                CheckIsActive();
                base.CopyTo(arr, index);
            }
            public new void EndEdit()
            {
                CheckIsActive();
                base.EndEdit();
            }
            public override void Insert(int index, object value)
            {
                CheckIsActive();
                base.Insert(index, value);
            }
            public void Insert(int index, DataRow value)
            {
                this.Insert(index, (object)value);
            }
            public new void MoveFirst()
            {
                CheckIsActive();
                base.MoveFirst();
            }
            public new void MoveLast()
            {
                CheckIsActive();
                base.MoveLast();
            }
            public new void MoveNext()
            {
                CheckIsActive();
                base.MoveNext();
            }
            public new void MovePrevious()
            {
                CheckIsActive();
                base.MovePrevious();
            }
            public override void RemoveAt(int index)
            {
                CheckIsActive();
                base.RemoveAt(index);
            }
            public new void RemoveCurrent()
            {
                CheckIsActive();
                base.RemoveCurrent();
            }
            #endregion
            #region Invisible Properties
            [Browsable(false)]
            public CDataTable Data
            {
                get { return m_data; }
            }
			[Browsable(false)]
            public override int Count 
            {
                get 
                {
                    /*
                     * if (!Active)
                        throw new DataSourceException(this, "Could not get record count when source is inactive.");
                     */
                    return base.Count;
                } 
            }
			[Browsable(false)]
            public new DataRow Current 
            { 
                get
                {
					CheckIsActive();
                    return (DataRow)base.Current;
                }
            }
			[Browsable(false)]
            public override bool IsFixedSize 
            {
                get 
                {
                    if (!Active)
                        throw new DataSourceException(this, "Could not determine whether it is fix size when source is inactive.");
                    return base.IsFixedSize;
                }
            }
            public new int Position 
            {
                get 
                {
					CheckIsActive();
                    return base.Position;
                }
                set 
                {
                    if (!Active)
                        throw new DataSourceException(this, "Could not set position when source is inactive.");
                    base.Position = value;
                } 
            }
            public new DataRow this[int index] 
            {
                get 
                {
                    if (!Active)
                        throw new DataSourceException(this, "Could not read values when source is inactive.");
                    return (DataRow)base[index];
                }
            }
            
            #endregion

            #region Visible Properties
            [Category("Behavior"), DescriptionAttribute("Open or Close data source.")]
            public bool Active
            {
                get { return (m_data != null); }
                set 
                {
                    if (Active && (!value))
                    {
                        Close();
                        return;
                    }
                    if ((!Active) && value)
                    {
                        Open();
                    }
                }
            }
            [Browsable(true)]
            [Category("Behavior"), DescriptionAttribute("Determine whether the source is editable.")]
            public new bool AllowEdit 
            {
                get { return m_allowEdit && base.AllowEdit; }
                set
                {
                    m_allowEdit = value;
                }
            }
            [Browsable(true)]
            [Category("Behavior"), DescriptionAttribute("Determine whether a new item can be added to the data source.")]
            public override bool AllowNew 
            {
                get { return m_allowNew && base.AllowNew;}
                set
                {
                    m_allowNew = value;
                    base.AllowNew = m_allowNew;
                }
            }
            [Browsable(true)]
            [Category("Behavior"), DescriptionAttribute("Determine whether a item can be removed from the data source.")]
            public new bool AllowRemove 
            {
                get { return m_allowRemove && base.AllowRemove; }
                set
                {
                    m_allowRemove = value;
                }
            }
            [Browsable(true)]
            [Category("Behavior"), DescriptionAttribute("Determine whether the data can be modified.")]
            public new bool IsReadOnly
            {
                get { return !(AllowNew || AllowEdit || AllowRemove); }
                set
                {
                    if (value)
                    {
                        if (IsReadOnly) return;
                        AllowNew = false;
                        AllowEdit = false;
                        AllowRemove = false;
                    }
                    else
                    {
                        if (!IsReadOnly) return;
                        AllowNew = true;
                        AllowEdit = true;
                        AllowRemove = true;
                    }
                }
            }
            [Category("Data"), DescriptionAttribute("Set case sensitive or not.")]
            public bool CaseSensitive
            {
                get
                {
                    return m_caseSensitive;
                }
                set
                {
                    m_caseSensitive = value;
                    if(m_data != null)
                        m_data.CaseSensitive = m_caseSensitive;
                }
            }
            [Category("Data"), DescriptionAttribute("Set Fields.")]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            [Editor(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
            public FieldCollection Fields
            {
                get { return m_fields; }
            }
            [Category("Data"), DescriptionAttribute("Set master source.")]
            public CDataSource MasterSource
            {
                get { return m_masterSource; }
                set { m_masterSource = value; }
            }
            [Category("Data"), DescriptionAttribute("Set the field name.")]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            [Editor(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
            public List<MasterDetailLink> MasterFields
            {
                get { return m_masterFields; }
            }
            #endregion
            #region Event 
			[Category("DataSource"), DescriptionAttribute("Event raise before open the data soure.")]
            public event EventHandler BeforeOpen;
			[Category("DataSource"), DescriptionAttribute("Event raise after open the data soure.")]
            public event EventHandler AfterOpen;
			[Category("DataSource"), DescriptionAttribute("Event raise before close the data soure.")]
            public event EventHandler BeforeClose;
			[Category("DataSource"), DescriptionAttribute("Event raise after close the data soure.")]
            public event EventHandler AfterClose;
			[Category("UnderlyingTable"), DescriptionAttribute("Occurs after a value has been changed for the specified DataColumn in a DataRow.")]
			public event DataColumnChangeEventHandler ColumnChanged;
			[Category("UnderlyingTable"), DescriptionAttribute("Occurs when a value is being changed for the specified DataColumn in a DataRow.")]
			public event DataColumnChangeEventHandler ColumnChanging;
			[Category("UnderlyingTable"), DescriptionAttribute("Occurs after a DataRow has been changed successfully.")]
			public event DataRowChangeEventHandler RowChanged;
			[Category("UnderlyingTable"), DescriptionAttribute("Occurs when a DataRow is changing.")]
			public event DataRowChangeEventHandler RowChanging;
			[Category("UnderlyingTable"), DescriptionAttribute("Occurs after a row in the table has been deleted.")]
			public event DataRowChangeEventHandler RowDeleted;
			[Category("UnderlyingTable"), DescriptionAttribute("Occurs before a row in the table is about to be deleted.")]
			public event DataRowChangeEventHandler RowDeleting;
			[Category("UnderlyingTable"), DescriptionAttribute("Occurs after a DataTable is cleared.")]
			public event DataTableClearEventHandler TableCleared;
			[Category("UnderlyingTable"), DescriptionAttribute("Occurs when a DataTable is about to be cleared.")]
			public event DataTableClearEventHandler TableClearing;
			[Category("UnderlyingTable"), DescriptionAttribute("Occurs when a new DataRow is inserted.")]
			public event DataTableNewRowEventHandler TableNewRow;


            protected void OnBeforeOpen()
            {
                if (BeforeOpen != null)
                    BeforeOpen(this, new EventArgs());
            }
            protected void OnAfterOpen()
            {
                if (AfterOpen != null)
                    AfterOpen(this, new EventArgs());
            }
            protected void OnBeforeClose()
            {
                if (BeforeClose != null)
                    BeforeClose(this, new EventArgs());
            }
            protected void OnAfterClose()
            {
                if (AfterClose != null)
                    AfterClose(this, new EventArgs());
            }
            public class FieldChangeEventArgs : EventArgs
            {
                bool m_cancel;
                object m_newValue, m_oldValue;
                public FieldChangeEventArgs(bool cancel, object newValue, object oldValue)
                    : base()
                {
                    m_cancel = cancel;
                    m_newValue = newValue;
                    m_oldValue = oldValue;
                }
                public bool Cancel
                {
                    get { return m_cancel; }
                    set { m_cancel = value; }
                }
                public object NewValue
                {
                    get { return m_newValue; }
                    set { m_newValue = value; }
                }
                public object OldValue
                {
                    get { return m_oldValue; }
                }
            }
            public delegate void FieldChangeEventHandler(Object sender, FieldChangeEventArgs e);
            #endregion
            #region useless properties but have to have.
            [System.ComponentModel.Browsable(false)]
            public new Object DataSource { get { return base.DataSource; }}
            [System.ComponentModel.Browsable(false)]
            public new string DataMember { get { return base.DataMember; } }
            #endregion

        }
    }
}
