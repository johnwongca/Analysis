namespace Algorithm.Core.Forms
{
    partial class FormSymbol
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSymbol));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.textSearch = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOk = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gSymbol = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gExchange = new System.Windows.Forms.DataGridView();
            this.bsSymbol = new System.Windows.Forms.BindingSource(this.components);
            this.bsExchange = new System.Windows.Forms.BindingSource(this.components);
            this.ddExchange = new System.Windows.Forms.ToolStripDropDownButton();
            this.aaaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gSymbol)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gExchange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSymbol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsExchange)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.ddExchange,
            this.textSearch,
            this.toolStripSeparator1,
            this.btnOk});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(713, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(45, 22);
            this.toolStripLabel1.Text = "Search:";
            // 
            // textSearch
            // 
            this.textSearch.Name = "textSearch";
            this.textSearch.Size = new System.Drawing.Size(100, 25);
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
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(713, 426);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gSymbol);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(705, 400);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Symbols";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gSymbol
            // 
            this.gSymbol.AllowUserToAddRows = false;
            this.gSymbol.AllowUserToDeleteRows = false;
            this.gSymbol.AllowUserToOrderColumns = true;
            this.gSymbol.AllowUserToResizeRows = false;
            this.gSymbol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gSymbol.ContextMenuStrip = this.contextMenuStrip1;
            this.gSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gSymbol.Location = new System.Drawing.Point(3, 3);
            this.gSymbol.Name = "gSymbol";
            this.gSymbol.ReadOnly = true;
            this.gSymbol.Size = new System.Drawing.Size(699, 394);
            this.gSymbol.TabIndex = 2;
            this.gSymbol.DoubleClick += new System.EventHandler(this.gSymbol_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(114, 26);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gExchange);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(705, 400);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Exchanges";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            this.gExchange.Size = new System.Drawing.Size(699, 394);
            this.gExchange.TabIndex = 1;
            // 
            // bsSymbol
            // 
            this.bsSymbol.CurrentChanged += new System.EventHandler(this.bsSymbol_CurrentChanged);
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
            this.aaaToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aaaToolStripMenuItem.Text = "aaa";
            // 
            // FormSymbol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(713, 451);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormSymbol";
            this.Text = "Symbol";
            this.Activated += new System.EventHandler(this.FormSymbol_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSymbol_FormClosing);
            this.Load += new System.EventHandler(this.FormSymbol_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gSymbol)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gExchange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSymbol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsExchange)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox textSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnOk;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView gExchange;
        private System.Windows.Forms.DataGridView gSymbol;
        private System.Windows.Forms.BindingSource bsSymbol;
        private System.Windows.Forms.BindingSource bsExchange;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton ddExchange;
        private System.Windows.Forms.ToolStripMenuItem aaaToolStripMenuItem;
    }
}
