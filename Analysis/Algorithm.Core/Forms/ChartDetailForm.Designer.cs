namespace Algorithm.Core.Forms
{
    partial class ChartDetailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartDetailForm));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "ssss",
            "sss1",
            "sss2",
            "sss3"}, -1);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.gSymbol = new System.Windows.Forms.DataGridView();
            this.gSymbolContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.gExchange = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ddExchange = new System.Windows.Forms.ToolStripDropDownButton();
            this.aaaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textSearch = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOk = new System.Windows.Forms.ToolStripButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.chartPropertyAll = new System.Windows.Forms.PropertyGrid();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnSetSplitter = new System.Windows.Forms.ToolStripButton();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.chartPropertySelected = new System.Windows.Forms.PropertyGrid();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.bsExchange = new System.Windows.Forms.BindingSource(this.components);
            this.bsSymbol = new System.Windows.Forms.BindingSource(this.components);
            this.lvData = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gSymbol)).BeginInit();
            this.gSymbolContextMenu.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gExchange)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsExchange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSymbol)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(552, 648);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControl2);
            this.tabPage1.Controls.Add(this.toolStrip1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(515, 622);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Search";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 28);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(509, 591);
            this.tabControl2.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.gSymbol);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(501, 565);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Symbols";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // gSymbol
            // 
            this.gSymbol.AllowUserToAddRows = false;
            this.gSymbol.AllowUserToDeleteRows = false;
            this.gSymbol.AllowUserToOrderColumns = true;
            this.gSymbol.AllowUserToResizeRows = false;
            this.gSymbol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gSymbol.ContextMenuStrip = this.gSymbolContextMenu;
            this.gSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gSymbol.Location = new System.Drawing.Point(3, 3);
            this.gSymbol.Name = "gSymbol";
            this.gSymbol.ReadOnly = true;
            this.gSymbol.Size = new System.Drawing.Size(495, 559);
            this.gSymbol.TabIndex = 2;
            this.gSymbol.DoubleClick += new System.EventHandler(this.btnOk_Click);
            this.gSymbol.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gSymbol_KeyUp);
            // 
            // gSymbolContextMenu
            // 
            this.gSymbolContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem,
            this.toolStripMenuItem1,
            this.refreshToolStripMenuItem});
            this.gSymbolContextMenu.Name = "contextMenuStrip1";
            this.gSymbolContextMenu.Size = new System.Drawing.Size(114, 54);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.selectToolStripMenuItem.Text = "Select";
            this.selectToolStripMenuItem.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(110, 6);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.gExchange);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(501, 565);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Exchanges";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // gExchange
            // 
            this.gExchange.AllowUserToAddRows = false;
            this.gExchange.AllowUserToDeleteRows = false;
            this.gExchange.AllowUserToOrderColumns = true;
            this.gExchange.AllowUserToResizeRows = false;
            this.gExchange.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gExchange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gExchange.Location = new System.Drawing.Point(3, 3);
            this.gExchange.Name = "gExchange";
            this.gExchange.ReadOnly = true;
            this.gExchange.Size = new System.Drawing.Size(495, 559);
            this.gExchange.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.ddExchange,
            this.textSearch,
            this.toolStripSeparator1,
            this.btnOk});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(509, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(45, 22);
            this.toolStripLabel1.Text = "Search:";
            // 
            // ddExchange
            // 
            this.ddExchange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddExchange.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aaaToolStripMenuItem});
            this.ddExchange.Image = ((System.Drawing.Image)(resources.GetObject("ddExchange.Image")));
            this.ddExchange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddExchange.Name = "ddExchange";
            this.ddExchange.Size = new System.Drawing.Size(70, 22);
            this.ddExchange.Text = "Exchange";
            // 
            // aaaToolStripMenuItem
            // 
            this.aaaToolStripMenuItem.Name = "aaaToolStripMenuItem";
            this.aaaToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.aaaToolStripMenuItem.Text = "aaa";
            // 
            // textSearch
            // 
            this.textSearch.Name = "textSearch";
            this.textSearch.Size = new System.Drawing.Size(100, 25);
            this.textSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textSearch_KeyPress);
            this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnOk
            // 
            this.btnOk.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(27, 22);
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabControl3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(515, 622);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Chart";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(509, 616);
            this.tabControl3.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.chartPropertyAll);
            this.tabPage5.Controls.Add(this.toolStrip2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(501, 590);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "All";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // chartPropertyAll
            // 
            this.chartPropertyAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartPropertyAll.Location = new System.Drawing.Point(3, 28);
            this.chartPropertyAll.Name = "chartPropertyAll";
            this.chartPropertyAll.Size = new System.Drawing.Size(495, 559);
            this.chartPropertyAll.TabIndex = 2;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSetSplitter});
            this.toolStrip2.Location = new System.Drawing.Point(3, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(495, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "btnSetSplitter";
            // 
            // btnSetSplitter
            // 
            this.btnSetSplitter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSetSplitter.Image = ((System.Drawing.Image)(resources.GetObject("btnSetSplitter.Image")));
            this.btnSetSplitter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetSplitter.Name = "btnSetSplitter";
            this.btnSetSplitter.Size = new System.Drawing.Size(48, 22);
            this.btnSetSplitter.Text = "Splitter";
            this.btnSetSplitter.Click += new System.EventHandler(this.btnSetSplitter_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.chartPropertySelected);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(501, 590);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "Selected";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // chartPropertySelected
            // 
            this.chartPropertySelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartPropertySelected.Location = new System.Drawing.Point(3, 3);
            this.chartPropertySelected.Name = "chartPropertySelected";
            this.chartPropertySelected.Size = new System.Drawing.Size(495, 584);
            this.chartPropertySelected.TabIndex = 1;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.lvData);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(544, 622);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "Data";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // lvData
            // 
            this.lvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvData.GridLines = true;
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = "Current";
            listViewGroup2.Header = "ListViewGroup";
            listViewGroup2.Name = "Price";
            listViewGroup3.Header = "ListViewGroup";
            listViewGroup3.Name = "Volume";
            this.lvData.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            listViewItem1.Group = listViewGroup1;
            this.lvData.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvData.Location = new System.Drawing.Point(3, 3);
            this.lvData.MultiSelect = false;
            this.lvData.Name = "lvData";
            this.lvData.Size = new System.Drawing.Size(538, 616);
            this.lvData.TabIndex = 0;
            this.lvData.UseCompatibleStateImageBehavior = false;
            this.lvData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 103;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 103;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 103;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Value";
            this.columnHeader4.Width = 103;
            // 
            // ChartDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 648);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ChartDetailForm";
            this.Text = "Details";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChartDetailForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gSymbol)).EndInit();
            this.gSymbolContextMenu.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gExchange)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsExchange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSymbol)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripDropDownButton ddExchange;
        private System.Windows.Forms.ToolStripMenuItem aaaToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox textSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnOk;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView gSymbol;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView gExchange;
        private System.Windows.Forms.BindingSource bsExchange;
        private System.Windows.Forms.BindingSource bsSymbol;
        private System.Windows.Forms.ContextMenuStrip gSymbolContextMenu;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        public System.Windows.Forms.PropertyGrid chartPropertySelected;
        public System.Windows.Forms.PropertyGrid chartPropertyAll;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnSetSplitter;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.ListView lvData;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}