using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Fasterflect;

namespace Algorithm.Core
{
    public partial class Indicator: IndicatorBase
    {
        public string CursorName 
        { 
            get
            {
                return Methods.GetCursorName(SymbolID, IntervalType, Interval, StartDate);
            }
        }
        bool mIsWritingToServer = false;
        public bool IsWritingToServer { get { return mIsWritingToServer; } }
        public Dictionary<string, InputAttribute> InputAttributes
        {
            get { return this.GetType().GetInputAttributes(); }
        }
        public void SetDefaultValues()
        {
            foreach(var p in InputAttributes)
            {
                this.SetPropertyValue(p.Key, p.Value.GetPropertyValue("DefaultValue"));
            }
        }
        
        public Indicator(int size = DefaultDataWindowSize)
            : base(size)
        {

        }

        public void WriteToServer()
        {
            try
            {
                mIsWritingToServer = true;
                this.OpenData();
                using (IndicatorReader r = new IndicatorReader() { Indicator = this })
                {
                    r.WriteToServer(CursorName);
                }
                this.CloseData();
            }
            finally
            {
                mIsWritingToServer = false;
            }
        }
        public void WriteToServerAsync(Action<Exception> asyncDone = null)
        {
            (new Thread(() => 
            {
                try
                {
                    WriteToServer();
                }
                catch(Exception e)
                {
                    if (asyncDone != null)
                        asyncDone(e);
                    else
                        Console.WriteLine("Exception in thread {0}: {1}", Thread.CurrentThread.ManagedThreadId, e);
                    return;
                }
                if (asyncDone != null)
                    asyncDone(null);
            })).Start();
        }
        public DataTable WriteToDataTable(DataTable table = null)
        {
            if (table == null)
                table = new DataTable();
            else
                table.Rows.Clear();
            this.OpenData();
            using (IDataReader r = new IndicatorReader() { Indicator = this })
            {
                table.Load(r);
                table.PrimaryKey = new DataColumn[]{table.Columns[0]}; 
            }
            this.CloseData();
            return table;
        }
    }
}
