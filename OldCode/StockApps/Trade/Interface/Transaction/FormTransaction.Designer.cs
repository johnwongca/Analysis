namespace Trade.Interface.Transaction
{
    partial class FormTransaction
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dg1 = new Trade.FixedDataGridView();
            this.transIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SymbolDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityAvailableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.payableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receivedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.symbolIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmBuy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miBuy = new System.Windows.Forms.ToolStripMenuItem();
            this.miBuyRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.miSellInitialize = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miShowExtendedChart = new System.Windows.Forms.ToolStripMenuItem();
            this.miShowExtendedNewChart = new System.Windows.Forms.ToolStripMenuItem();
            this.bsSecurity = new Trade.Base.BindingSourceAdapter(this.components);
            this.daTransSecurity1 = new Trade.Interface.Transaction.DataSetTransactionTableAdapters.DaTransSecurity();
            this.dataSetTransaction = new Trade.Interface.Transaction.DataSetTransaction();
            this.fixedDataGridView2 = new Trade.FixedDataGridView();
            this.transIDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rateFromDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rateToDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tradingFeeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchangeFeeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Parent1ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmSell = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSell = new System.Windows.Forms.ToolStripMenuItem();
            this.miSellremove = new System.Windows.Forms.ToolStripMenuItem();
            this.bsTrans = new Trade.Base.BindingSourceAdapter(this.components);
            this.daTrans1 = new Trade.Interface.Transaction.DataSetTransactionTableAdapters.DaTrans();
            this.miShowExtendedChart1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbMain.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).BeginInit();
            this.cmBuy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSecurity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransaction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fixedDataGridView2)).BeginInit();
            this.cmSell.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsTrans)).BeginInit();
            this.SuspendLayout();
            // 
            // tbMain
            // 
            this.tbMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tbMain.Controls.Add(this.tabPage1);
            this.tbMain.Controls.Add(this.tabPage2);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbMain.Location = new System.Drawing.Point(0, 0);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(1050, 30);
            this.tbMain.TabIndex = 0;
            this.tbMain.SelectedIndexChanged += new System.EventHandler(this.tbMain_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1042, 1);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1042, 1);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 30);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dg1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fixedDataGridView2);
            this.splitContainer1.Size = new System.Drawing.Size(1050, 568);
            this.splitContainer1.SplitterDistance = 244;
            this.splitContainer1.TabIndex = 1;
            // 
            // dg1
            // 
            this.dg1.AutoGenerateColumns = false;
            this.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.transIDDataGridViewTextBoxColumn,
            this.dateDataGridViewTextBoxColumn,
            this.accountNumberDataGridViewTextBoxColumn,
            this.SymbolDesc,
            this.quantityDataGridViewTextBoxColumn,
            this.quantityAvailableDataGridViewTextBoxColumn,
            this.payableDataGridViewTextBoxColumn,
            this.paidDataGridViewTextBoxColumn,
            this.receivableDataGridViewTextBoxColumn,
            this.receivedDataGridViewTextBoxColumn,
            this.symbolIDDataGridViewTextBoxColumn});
            this.dg1.ContextMenuStrip = this.cmBuy;
            this.dg1.DataSource = this.bsSecurity;
            this.dg1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg1.Location = new System.Drawing.Point(0, 0);
            this.dg1.Name = "dg1";
            this.dg1.ReadOnly = true;
            this.dg1.Size = new System.Drawing.Size(1050, 244);
            this.dg1.TabIndex = 0;
            this.dg1.DoubleClick += new System.EventHandler(this.dg1_DoubleClick);
            // 
            // transIDDataGridViewTextBoxColumn
            // 
            this.transIDDataGridViewTextBoxColumn.DataPropertyName = "TransID";
            this.transIDDataGridViewTextBoxColumn.HeaderText = "TransID";
            this.transIDDataGridViewTextBoxColumn.Name = "transIDDataGridViewTextBoxColumn";
            this.transIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dateDataGridViewTextBoxColumn
            // 
            this.dateDataGridViewTextBoxColumn.DataPropertyName = "Date";
            this.dateDataGridViewTextBoxColumn.HeaderText = "Date";
            this.dateDataGridViewTextBoxColumn.Name = "dateDataGridViewTextBoxColumn";
            this.dateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // accountNumberDataGridViewTextBoxColumn
            // 
            this.accountNumberDataGridViewTextBoxColumn.DataPropertyName = "AccountNumber";
            this.accountNumberDataGridViewTextBoxColumn.HeaderText = "AccountNumber";
            this.accountNumberDataGridViewTextBoxColumn.Name = "accountNumberDataGridViewTextBoxColumn";
            this.accountNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // SymbolDesc
            // 
            this.SymbolDesc.DataPropertyName = "SymbolDesc";
            this.SymbolDesc.HeaderText = "Symbol";
            this.SymbolDesc.Name = "SymbolDesc";
            this.SymbolDesc.ReadOnly = true;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // quantityAvailableDataGridViewTextBoxColumn
            // 
            this.quantityAvailableDataGridViewTextBoxColumn.DataPropertyName = "QuantityAvailable";
            this.quantityAvailableDataGridViewTextBoxColumn.HeaderText = "QuantityAvailable";
            this.quantityAvailableDataGridViewTextBoxColumn.Name = "quantityAvailableDataGridViewTextBoxColumn";
            this.quantityAvailableDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // payableDataGridViewTextBoxColumn
            // 
            this.payableDataGridViewTextBoxColumn.DataPropertyName = "Payable";
            this.payableDataGridViewTextBoxColumn.HeaderText = "Payable";
            this.payableDataGridViewTextBoxColumn.Name = "payableDataGridViewTextBoxColumn";
            this.payableDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // paidDataGridViewTextBoxColumn
            // 
            this.paidDataGridViewTextBoxColumn.DataPropertyName = "Paid";
            this.paidDataGridViewTextBoxColumn.HeaderText = "Paid";
            this.paidDataGridViewTextBoxColumn.Name = "paidDataGridViewTextBoxColumn";
            this.paidDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // receivableDataGridViewTextBoxColumn
            // 
            this.receivableDataGridViewTextBoxColumn.DataPropertyName = "Receivable";
            this.receivableDataGridViewTextBoxColumn.HeaderText = "Receivable";
            this.receivableDataGridViewTextBoxColumn.Name = "receivableDataGridViewTextBoxColumn";
            this.receivableDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // receivedDataGridViewTextBoxColumn
            // 
            this.receivedDataGridViewTextBoxColumn.DataPropertyName = "Received";
            this.receivedDataGridViewTextBoxColumn.HeaderText = "Received";
            this.receivedDataGridViewTextBoxColumn.Name = "receivedDataGridViewTextBoxColumn";
            this.receivedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // symbolIDDataGridViewTextBoxColumn
            // 
            this.symbolIDDataGridViewTextBoxColumn.DataPropertyName = "SymbolID";
            this.symbolIDDataGridViewTextBoxColumn.HeaderText = "SymbolID";
            this.symbolIDDataGridViewTextBoxColumn.Name = "symbolIDDataGridViewTextBoxColumn";
            this.symbolIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cmBuy
            // 
            this.cmBuy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miBuy,
            this.miBuyRemove,
            this.miSellInitialize,
            this.toolStripSeparator1,
            this.miShowExtendedChart,
            this.miShowExtendedChart1,
            this.toolStripSeparator2,
            this.miShowExtendedNewChart});
            this.cmBuy.Name = "cmBuy";
            this.cmBuy.Size = new System.Drawing.Size(204, 170);
            this.cmBuy.Opening += new System.ComponentModel.CancelEventHandler(this.cmBuy_Opening);
            // 
            // miBuy
            // 
            this.miBuy.Name = "miBuy";
            this.miBuy.Size = new System.Drawing.Size(203, 22);
            this.miBuy.Text = "Buy";
            this.miBuy.Click += new System.EventHandler(this.miBuy_Click);
            // 
            // miBuyRemove
            // 
            this.miBuyRemove.Name = "miBuyRemove";
            this.miBuyRemove.Size = new System.Drawing.Size(203, 22);
            this.miBuyRemove.Text = "Remove";
            this.miBuyRemove.Click += new System.EventHandler(this.miBuyRemove_Click);
            // 
            // miSellInitialize
            // 
            this.miSellInitialize.Name = "miSellInitialize";
            this.miSellInitialize.Size = new System.Drawing.Size(203, 22);
            this.miSellInitialize.Text = "Sell Initialize";
            this.miSellInitialize.Click += new System.EventHandler(this.miSellInitialize_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(200, 6);
            // 
            // miShowExtendedChart
            // 
            this.miShowExtendedChart.Name = "miShowExtendedChart";
            this.miShowExtendedChart.Size = new System.Drawing.Size(203, 22);
            this.miShowExtendedChart.Text = "Show Extended Chart";
            this.miShowExtendedChart.Click += new System.EventHandler(this.dg1_DoubleClick);
            // 
            // miShowExtendedNewChart
            // 
            this.miShowExtendedNewChart.Name = "miShowExtendedNewChart";
            this.miShowExtendedNewChart.Size = new System.Drawing.Size(203, 22);
            this.miShowExtendedNewChart.Text = "Show Extended New Chart";
            this.miShowExtendedNewChart.Click += new System.EventHandler(this.miShowExtendedNewChart_Click);
            // 
            // bsSecurity
            // 
            this.bsSecurity.Adapter = this.daTransSecurity1;
            this.bsSecurity.AllowNew = false;
            this.bsSecurity.DataSource = this.dataSetTransaction.TransSecurity;
            this.bsSecurity.DataTable = this.dataSetTransaction.TransSecurity;
            this.bsSecurity.InternalPositionChangedEventAttached = false;
            this.bsSecurity.ReadOnly = true;
            this.bsSecurity.CurrentChanged += new System.EventHandler(this.bsSecurity_CurrentChanged);
            // 
            // daTransSecurity1
            // 
            this.daTransSecurity1.ClearBeforeFill = true;
            // 
            // dataSetTransaction
            // 
            this.dataSetTransaction.DataSetName = "DataSetTransaction";
            this.dataSetTransaction.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // fixedDataGridView2
            // 
            this.fixedDataGridView2.AutoGenerateColumns = false;
            this.fixedDataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fixedDataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.transIDDataGridViewTextBoxColumn1,
            this.dateDataGridViewTextBoxColumn1,
            this.typeDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn1,
            this.rateFromDataGridViewTextBoxColumn,
            this.rateToDataGridViewTextBoxColumn,
            this.tradingFeeDataGridViewTextBoxColumn,
            this.exchangeFeeDataGridViewTextBoxColumn,
            this.amountDataGridViewTextBoxColumn,
            this.Parent1ID});
            this.fixedDataGridView2.ContextMenuStrip = this.cmSell;
            this.fixedDataGridView2.DataSource = this.bsTrans;
            this.fixedDataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fixedDataGridView2.Location = new System.Drawing.Point(0, 0);
            this.fixedDataGridView2.Name = "fixedDataGridView2";
            this.fixedDataGridView2.ReadOnly = true;
            this.fixedDataGridView2.Size = new System.Drawing.Size(1050, 320);
            this.fixedDataGridView2.TabIndex = 0;
            // 
            // transIDDataGridViewTextBoxColumn1
            // 
            this.transIDDataGridViewTextBoxColumn1.DataPropertyName = "TransID";
            this.transIDDataGridViewTextBoxColumn1.HeaderText = "TransID";
            this.transIDDataGridViewTextBoxColumn1.Name = "transIDDataGridViewTextBoxColumn1";
            this.transIDDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dateDataGridViewTextBoxColumn1
            // 
            this.dateDataGridViewTextBoxColumn1.DataPropertyName = "Date";
            this.dateDataGridViewTextBoxColumn1.HeaderText = "Date";
            this.dateDataGridViewTextBoxColumn1.Name = "dateDataGridViewTextBoxColumn1";
            this.dateDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // typeDataGridViewTextBoxColumn
            // 
            this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
            this.typeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
            this.typeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // quantityDataGridViewTextBoxColumn1
            // 
            this.quantityDataGridViewTextBoxColumn1.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn1.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn1.Name = "quantityDataGridViewTextBoxColumn1";
            this.quantityDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // rateFromDataGridViewTextBoxColumn
            // 
            this.rateFromDataGridViewTextBoxColumn.DataPropertyName = "RateFrom";
            this.rateFromDataGridViewTextBoxColumn.HeaderText = "RateFrom";
            this.rateFromDataGridViewTextBoxColumn.Name = "rateFromDataGridViewTextBoxColumn";
            this.rateFromDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rateToDataGridViewTextBoxColumn
            // 
            this.rateToDataGridViewTextBoxColumn.DataPropertyName = "RateTo";
            this.rateToDataGridViewTextBoxColumn.HeaderText = "RateTo";
            this.rateToDataGridViewTextBoxColumn.Name = "rateToDataGridViewTextBoxColumn";
            this.rateToDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tradingFeeDataGridViewTextBoxColumn
            // 
            this.tradingFeeDataGridViewTextBoxColumn.DataPropertyName = "TradingFee";
            this.tradingFeeDataGridViewTextBoxColumn.HeaderText = "TradingFee";
            this.tradingFeeDataGridViewTextBoxColumn.Name = "tradingFeeDataGridViewTextBoxColumn";
            this.tradingFeeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // exchangeFeeDataGridViewTextBoxColumn
            // 
            this.exchangeFeeDataGridViewTextBoxColumn.DataPropertyName = "ExchangeFee";
            this.exchangeFeeDataGridViewTextBoxColumn.HeaderText = "ExchangeFee";
            this.exchangeFeeDataGridViewTextBoxColumn.Name = "exchangeFeeDataGridViewTextBoxColumn";
            this.exchangeFeeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // amountDataGridViewTextBoxColumn
            // 
            this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
            this.amountDataGridViewTextBoxColumn.HeaderText = "Amount";
            this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
            this.amountDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Parent1ID
            // 
            this.Parent1ID.DataPropertyName = "Parent1ID";
            this.Parent1ID.HeaderText = "Parent1ID";
            this.Parent1ID.Name = "Parent1ID";
            this.Parent1ID.ReadOnly = true;
            // 
            // cmSell
            // 
            this.cmSell.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSell,
            this.miSellremove});
            this.cmSell.Name = "cmSell";
            this.cmSell.Size = new System.Drawing.Size(114, 48);
            this.cmSell.Opening += new System.ComponentModel.CancelEventHandler(this.cmSell_Opening);
            // 
            // miSell
            // 
            this.miSell.Name = "miSell";
            this.miSell.Size = new System.Drawing.Size(113, 22);
            this.miSell.Text = "Sell";
            this.miSell.Click += new System.EventHandler(this.miSell_Click);
            // 
            // miSellremove
            // 
            this.miSellremove.Name = "miSellremove";
            this.miSellremove.Size = new System.Drawing.Size(113, 22);
            this.miSellremove.Text = "Remove";
            this.miSellremove.Click += new System.EventHandler(this.miSellremove_Click);
            // 
            // bsTrans
            // 
            this.bsTrans.Adapter = this.daTrans1;
            this.bsTrans.AllowNew = false;
            this.bsTrans.DataSource = this.dataSetTransaction.Trans;
            this.bsTrans.DataTable = this.dataSetTransaction.Trans;
            this.bsTrans.InternalPositionChangedEventAttached = false;
            this.bsTrans.ReadOnly = true;
            // 
            // daTrans1
            // 
            this.daTrans1.ClearBeforeFill = true;
            // 
            // miShowExtendedChart1
            // 
            this.miShowExtendedChart1.Name = "miShowExtendedChart1";
            this.miShowExtendedChart1.Size = new System.Drawing.Size(203, 22);
            this.miShowExtendedChart1.Text = "Show Extended Chart1";
            this.miShowExtendedChart1.Click += new System.EventHandler(this.miShowExtendedChart1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(200, 6);
            // 
            // FormTransaction
            // 
            this.AutoOpen = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1050, 598);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tbMain);
            this.Name = "FormTransaction";
            this.Text = "All Transactions";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTransaction_FormClosed);
            this.tbMain.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).EndInit();
            this.cmBuy.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsSecurity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransaction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fixedDataGridView2)).EndInit();
            this.cmSell.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsTrans)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataSetTransaction dataSetTransaction;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Trade.Interface.Transaction.DataSetTransactionTableAdapters.DaTrans daTrans1;
        private Trade.Interface.Transaction.DataSetTransactionTableAdapters.DaTransSecurity daTransSecurity1;
        private Trade.Base.BindingSourceAdapter bsSecurity;
        private Trade.Base.BindingSourceAdapter bsTrans;
        private FixedDataGridView dg1;
        private FixedDataGridView fixedDataGridView2;
        private System.Windows.Forms.ContextMenuStrip cmBuy;
        private System.Windows.Forms.ToolStripMenuItem miBuyRemove;
        private System.Windows.Forms.ToolStripMenuItem miBuy;
        private System.Windows.Forms.ToolStripMenuItem miSellInitialize;
        private System.Windows.Forms.ContextMenuStrip cmSell;
        private System.Windows.Forms.ToolStripMenuItem miSell;
        private System.Windows.Forms.ToolStripMenuItem miSellremove;
        private System.Windows.Forms.DataGridViewTextBoxColumn transIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn rateFromDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rateToDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tradingFeeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchangeFeeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parent1ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn transIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SymbolDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityAvailableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn payableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receivedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn symbolIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripMenuItem miShowExtendedChart;
        private System.Windows.Forms.ToolStripMenuItem miShowExtendedNewChart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miShowExtendedChart1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        
    }
}
