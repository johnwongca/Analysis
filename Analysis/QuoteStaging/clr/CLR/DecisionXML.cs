using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
namespace SDASync
{
    public class DecisionXML
    {
        XElement mData = null;
        DecisionXML mParent = null;
        public List<DecisionXML> Children = new List<DecisionXML>();
        public int Sequence = 0;
        public string Name = "";
        public string Value = "";
        public string Path = "/";
        public bool IsAttribute = false;
        public XElement Data { get { return mData; } }
        public DecisionXML Parent { get { return mParent; } }
        public DecisionXML(DecisionXML parent, XElement data, int sequence = 1)
        {
            if (data == null)
                throw new Exception("Xml element should not be NULL.");
            Sequence = sequence;
            mData = data;
            mParent = parent;
            if (parent != null)
                Path = parent.Path + Sequence.ToString() + "/";
            Analyze();
        }
        public DecisionXML(DecisionXML parent, int sequence = 1)
        {
            Sequence = sequence;
            mParent = parent;
            if (parent != null)
                Path = parent.Path + Sequence.ToString() + "/";
        }
        public void Analyze()
        {
            XElement n = mData.Element("Name");
            XElement v = null;
            if (n != null)
            {
                v = mData.Element("Value");
            }
            if (v != null) 
            {
                Name = n.Value;
                Value = v.Value;
                return;
            }
            Name = mData.Name.LocalName;
            if (!mData.HasElements)
            {
                Value = mData.Value;
            }
            DecisionXML pr = null;
            int sequence = 1;
            foreach (XElement item in mData.Elements())
            {
                pr = new DecisionXML(this, item, sequence);
                Children.Add(pr);
                sequence++;
            }
            foreach (XAttribute item in mData.Attributes())
            {
                pr = new DecisionXML(this, sequence);
                pr.Name = item.Name.LocalName;
                pr.Value = item.Value;
                pr.IsAttribute = true;
                sequence++;
                Children.Add(pr);
                
            }
        }
        public List<DecisionXML> ToList()
        {
            List<DecisionXML> ret = new List<DecisionXML>();
            ret.Add(this);
            foreach (DecisionXML item in Children)
                ret.AddRange(item.ToList());
            return ret;
        }
        //[SqlProcedure]
        //public static void ParseDecisionXML(SqlXml xml)
        //{
        //    if (xml.IsNull)
        //        return;
        //    SqlDataRecord record = new SqlDataRecord(
        //            new SqlMetaData("Path", SqlDbType.VarChar, -1), 
        //            new SqlMetaData("IsLeaf", SqlDbType.Bit),
        //            new SqlMetaData("IsAttribute", SqlDbType.Bit),
        //            new SqlMetaData("Name", SqlDbType.VarChar, -1),
        //            new SqlMetaData("Value", SqlDbType.VarChar, -1),
        //            new SqlMetaData("Length", SqlDbType.BigInt)
        //            );
        //    SqlContext.Pipe.SendResultsStart(record);
        //    foreach(DecisionXML item in (new DecisionXML(null, XElement.Parse(xml.Value))).ToList())
        //    {
        //        record.SetString(0, item.Path);
        //        record.SetBoolean(1, item.Children.Count == 0);
        //        record.SetBoolean(2, item.IsAttribute);
        //        record.SetString(3, item.Name);
        //        if (string.IsNullOrEmpty(item.Value))
        //            record.SetValue(4, DBNull.Value);
        //        else
        //            record.SetString(4, item.Value);
        //        record.SetInt64(5, item.Value.Length);
        //        SqlContext.Pipe.SendResultsRow(record);
        //    }
        //    SqlContext.Pipe.SendResultsEnd();
        //}
        [SqlFunction(DataAccess = DataAccessKind.Read,
                FillRowMethodName = "ParseDecisionXML_FillRow",
                TableDefinition = "Path nvarchar(max), IsLeaf bit, IsAttribute bit ,Name nvarchar(max), Value nvarchar(max), Length int"
                )]
        public static IEnumerable ParseDecisionXML(SqlXml xml)
        {

            if (xml.IsNull)
                return new List<DecisionXML>();
            return (new DecisionXML(null, XElement.Parse( xml.Value))).ToList();

        }
        public static void ParseDecisionXML_FillRow(object o, out SqlString Path, out SqlBoolean IsLeaf, out SqlBoolean IsAttribute, out SqlString Name, out SqlString Value, out SqlInt32 Length)
        {
            DecisionXML d = (DecisionXML)o;
            Path = new SqlString(d.Path);
            Name = new SqlString(d.Name);
            Value = string.IsNullOrEmpty(d.Value) ? SqlString.Null : new SqlString(d.Value);
            Length = new SqlInt32(d.Value.Length);
            IsLeaf = new SqlBoolean(d.Children.Count == 0);
            IsAttribute = new SqlBoolean(d.IsAttribute);
        }
    }
}
