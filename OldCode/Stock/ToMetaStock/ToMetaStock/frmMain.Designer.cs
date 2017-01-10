namespace ToMetaStock
{
	partial class frmMain
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lvSecurity = new System.Windows.Forms.ListView();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
			this.cmSecurity = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miDeleteSecurity = new System.Windows.Forms.ToolStripMenuItem();
			this.lvSecPrices = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.cmPrice = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miDeletePrices = new System.Windows.Forms.ToolStripMenuItem();
			this.btnReadFolder = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tbRead_Folder = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.nDaysBack = new System.Windows.Forms.NumericUpDown();
			this.l1 = new System.Windows.Forms.Label();
			this.p1 = new System.Windows.Forms.ProgressBar();
			this.btnExportAll = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.tbWrite_Folder = new System.Windows.Forms.TextBox();
			this.fBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.conn = new System.Data.SqlClient.SqlConnection();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.ckSort = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.cmSecurity.SuspendLayout();
			this.cmPrice.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nDaysBack)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(589, 407);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.btnRefresh);
			this.tabPage1.Controls.Add(this.splitContainer1);
			this.tabPage1.Controls.Add(this.btnReadFolder);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.tbRead_Folder);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(581, 381);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Read";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.Location = new System.Drawing.Point(474, 6);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(75, 23);
			this.btnRefresh.TabIndex = 2;
			this.btnRefresh.Text = "Refresh All";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(6, 34);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lvSecurity);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lvSecPrices);
			this.splitContainer1.Size = new System.Drawing.Size(545, 321);
			this.splitContainer1.SplitterDistance = 181;
			this.splitContainer1.TabIndex = 3;
			// 
			// lvSecurity
			// 
			this.lvSecurity.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15});
			this.lvSecurity.ContextMenuStrip = this.cmSecurity;
			this.lvSecurity.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvSecurity.FullRowSelect = true;
			this.lvSecurity.GridLines = true;
			this.lvSecurity.Location = new System.Drawing.Point(0, 0);
			this.lvSecurity.Name = "lvSecurity";
			this.lvSecurity.Size = new System.Drawing.Size(181, 321);
			this.lvSecurity.TabIndex = 0;
			this.lvSecurity.UseCompatibleStateImageBehavior = false;
			this.lvSecurity.View = System.Windows.Forms.View.Details;
			this.lvSecurity.SelectedIndexChanged += new System.EventHandler(this.lvSecurity_SelectedIndexChanged);
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Symbol";
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Name";
			this.columnHeader8.Width = 100;
			// 
			// columnHeader9
			// 
			this.columnHeader9.Text = "First Date";
			// 
			// columnHeader10
			// 
			this.columnHeader10.Text = "Last Date";
			// 
			// columnHeader11
			// 
			this.columnHeader11.Text = "Periodicity";
			// 
			// columnHeader12
			// 
			this.columnHeader12.Text = "Interval";
			// 
			// columnHeader13
			// 
			this.columnHeader13.Text = "Round";
			// 
			// columnHeader14
			// 
			this.columnHeader14.Text = "Fields";
			// 
			// columnHeader15
			// 
			this.columnHeader15.Text = "File Name";
			this.columnHeader15.Width = 200;
			// 
			// cmSecurity
			// 
			this.cmSecurity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDeleteSecurity});
			this.cmSecurity.Name = "cmSecurity";
			this.cmSecurity.Size = new System.Drawing.Size(106, 26);
			this.cmSecurity.Opening += new System.ComponentModel.CancelEventHandler(this.cmSecurity_Opening);
			// 
			// miDeleteSecurity
			// 
			this.miDeleteSecurity.Name = "miDeleteSecurity";
			this.miDeleteSecurity.Size = new System.Drawing.Size(105, 22);
			this.miDeleteSecurity.Text = "Delete";
			this.miDeleteSecurity.Click += new System.EventHandler(this.miDeleteSecurity_Click);
			// 
			// lvSecPrices
			// 
			this.lvSecPrices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
			this.lvSecPrices.ContextMenuStrip = this.cmPrice;
			this.lvSecPrices.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvSecPrices.FullRowSelect = true;
			this.lvSecPrices.GridLines = true;
			this.lvSecPrices.Location = new System.Drawing.Point(0, 0);
			this.lvSecPrices.Name = "lvSecPrices";
			this.lvSecPrices.Size = new System.Drawing.Size(360, 321);
			this.lvSecPrices.Sorting = System.Windows.Forms.SortOrder.Descending;
			this.lvSecPrices.TabIndex = 2;
			this.lvSecPrices.UseCompatibleStateImageBehavior = false;
			this.lvSecPrices.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Date";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Open";
			this.columnHeader2.Width = 70;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "High";
			this.columnHeader3.Width = 70;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Low";
			this.columnHeader4.Width = 70;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Close";
			this.columnHeader5.Width = 70;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Volume";
			this.columnHeader6.Width = 80;
			// 
			// cmPrice
			// 
			this.cmPrice.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDeletePrices});
			this.cmPrice.Name = "cmPrice";
			this.cmPrice.Size = new System.Drawing.Size(106, 26);
			this.cmPrice.Opening += new System.ComponentModel.CancelEventHandler(this.cmPrice_Opening);
			// 
			// miDeletePrices
			// 
			this.miDeletePrices.Name = "miDeletePrices";
			this.miDeletePrices.Size = new System.Drawing.Size(105, 22);
			this.miDeletePrices.Text = "Delete";
			this.miDeletePrices.Click += new System.EventHandler(this.miDeletePrices_Click);
			// 
			// btnReadFolder
			// 
			this.btnReadFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReadFolder.Location = new System.Drawing.Point(443, 6);
			this.btnReadFolder.Name = "btnReadFolder";
			this.btnReadFolder.Size = new System.Drawing.Size(24, 23);
			this.btnReadFolder.TabIndex = 1;
			this.btnReadFolder.Text = "...";
			this.btnReadFolder.UseVisualStyleBackColor = true;
			this.btnReadFolder.Click += new System.EventHandler(this.btnReadFolder_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Folder Name:";
			// 
			// tbRead_Folder
			// 
			this.tbRead_Folder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbRead_Folder.Location = new System.Drawing.Point(82, 8);
			this.tbRead_Folder.Name = "tbRead_Folder";
			this.tbRead_Folder.Size = new System.Drawing.Size(355, 20);
			this.tbRead_Folder.TabIndex = 0;
			this.tbRead_Folder.Text = "D:\\Stock Data";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.ckSort);
			this.tabPage2.Controls.Add(this.textBox1);
			this.tabPage2.Controls.Add(this.button2);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.nDaysBack);
			this.tabPage2.Controls.Add(this.l1);
			this.tabPage2.Controls.Add(this.p1);
			this.tabPage2.Controls.Add(this.btnExportAll);
			this.tabPage2.Controls.Add(this.button1);
			this.tabPage2.Controls.Add(this.label2);
			this.tabPage2.Controls.Add(this.tbWrite_Folder);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(581, 381);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Write";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(8, 96);
			this.textBox1.MaxLength = 0;
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(564, 277);
			this.textBox1.TabIndex = 19;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(433, 39);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 18;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Visible = false;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(213, 39);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(31, 13);
			this.label4.TabIndex = 17;
			this.label4.Text = "Days";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(100, 39);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 13);
			this.label3.TabIndex = 16;
			this.label3.Text = "Back To";
			// 
			// nDaysBack
			// 
			this.nDaysBack.Location = new System.Drawing.Point(154, 36);
			this.nDaysBack.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.nDaysBack.Name = "nDaysBack";
			this.nDaysBack.Size = new System.Drawing.Size(53, 20);
			this.nDaysBack.TabIndex = 6;
			// 
			// l1
			// 
			this.l1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.l1.Location = new System.Drawing.Point(8, 65);
			this.l1.Name = "l1";
			this.l1.Size = new System.Drawing.Size(564, 12);
			this.l1.TabIndex = 8;
			// 
			// p1
			// 
			this.p1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.p1.Location = new System.Drawing.Point(8, 80);
			this.p1.Name = "p1";
			this.p1.Size = new System.Drawing.Size(564, 10);
			this.p1.TabIndex = 7;
			// 
			// btnExportAll
			// 
			this.btnExportAll.Location = new System.Drawing.Point(9, 34);
			this.btnExportAll.Name = "btnExportAll";
			this.btnExportAll.Size = new System.Drawing.Size(75, 23);
			this.btnExportAll.TabIndex = 5;
			this.btnExportAll.Text = "Export";
			this.btnExportAll.UseVisualStyleBackColor = true;
			this.btnExportAll.Click += new System.EventHandler(this.btnExportAll_Click);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(549, 6);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(24, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "...";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 11);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Folder Name:";
			// 
			// tbWrite_Folder
			// 
			this.tbWrite_Folder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbWrite_Folder.Location = new System.Drawing.Point(82, 8);
			this.tbWrite_Folder.Name = "tbWrite_Folder";
			this.tbWrite_Folder.Size = new System.Drawing.Size(461, 20);
			this.tbWrite_Folder.TabIndex = 2;
			this.tbWrite_Folder.Text = "D:\\Stock Data";
			// 
			// conn
			// 
			this.conn.ConnectionString = "Data Source=localhost;Initial Catalog=Stock;Integrated Security=True;Persist Secu" +
				"rity Info=True;MultipleActiveResultSets=True";
			this.conn.FireInfoMessageEventOnUserErrors = false;
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
			this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
			// 
			// ckSort
			// 
			this.ckSort.AutoSize = true;
			this.ckSort.Location = new System.Drawing.Point(251, 39);
			this.ckSort.Name = "ckSort";
			this.ckSort.Size = new System.Drawing.Size(45, 17);
			this.ckSort.TabIndex = 20;
			this.ckSort.Text = "Sort";
			this.ckSort.UseVisualStyleBackColor = true;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(589, 407);
			this.Controls.Add(this.tabControl1);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Reader";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.cmSecurity.ResumeLayout(false);
			this.cmPrice.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nDaysBack)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Button btnReadFolder;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbRead_Folder;
		private System.Windows.Forms.FolderBrowserDialog fBrowserDialog;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListView lvSecurity;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.ListView lvSecPrices;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.Windows.Forms.ColumnHeader columnHeader12;
		private System.Windows.Forms.ColumnHeader columnHeader13;
		private System.Windows.Forms.ColumnHeader columnHeader14;
		private System.Windows.Forms.ColumnHeader columnHeader15;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbWrite_Folder;
		private System.Windows.Forms.Button btnExportAll;
		private System.Data.SqlClient.SqlConnection conn;
		private System.Windows.Forms.ProgressBar p1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown nDaysBack;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private System.Windows.Forms.Label l1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ContextMenuStrip cmSecurity;
		private System.Windows.Forms.ContextMenuStrip cmPrice;
		private System.Windows.Forms.ToolStripMenuItem miDeleteSecurity;
		private System.Windows.Forms.ToolStripMenuItem miDeletePrices;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.CheckBox ckSort;
	}
}

