namespace Trade.Interface.Transaction {
    
    
    public partial class DataSetTransaction {
    }

}
/*
namespace Trade.Interface.Transaction.DataSetTransactionTableAdapters
 */
namespace Trade.Interface.Transaction.DataSetTransactionTableAdapters
{
    public partial class DaTransSecurity
    {
        public virtual int Fill(Trade.Interface.Transaction.DataSetTransaction.TransSecurityDataTable dataTable)
        {
            if (Program.fromTransactions == null)
                return 0;
            return this.Fill(dataTable, Program.fromTransactions.SelectedStatus);
        }
    }
}