using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Trade.Base;

namespace Trade
{
    class FixedDataGridView : DataGridView
    {
        private bool reEntrent = false;
        protected override bool SetCurrentCellAddressCore(int columnIndex, int rowIndex, bool setAnchorCellAddress, bool validateCurrentCell, bool throughMouseClick)
        {
            bool rv = true;
            if (!reEntrent)
            {
                reEntrent = true;
                rv = base.SetCurrentCellAddressCore(columnIndex, rowIndex, setAnchorCellAddress, validateCurrentCell, throughMouseClick);
                reEntrent = false;
            }
            return rv;
        }
    }
}
