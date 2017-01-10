using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Base
{
    namespace Controls
    {
        [System.ComponentModel.ToolboxItem(true),System.ComponentModel.DesignTimeVisible(true)]
        public class CDataTable : DataTable
        {
        
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                }
                base.Dispose(disposing);
            }
            private void InitializeComponent()
            {
                //components = new System.ComponentModel.Container();
            }
            public CDataTable(IContainer container): base()
            {
            }
            public CDataTable()
            {
                InitializeComponent();
            }
        }
    }
}
