namespace Trade.Base
{
    partial class SChart
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("aaa", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "aaa", System.Drawing.SystemColors.MenuHighlight, System.Drawing.SystemColors.Window, new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)))),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "aaaaaaa", System.Drawing.Color.Red, System.Drawing.SystemColors.Window, new System.Drawing.Font("Wingdings 3", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2))))}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("bbb");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("ccc");
            this.panelBaseBar = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbItemSize = new System.Windows.Forms.TextBox();
            this.btnItemSizeMinus = new System.Windows.Forms.Button();
            this.btnItemSizePlus = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.sbCurrent = new System.Windows.Forms.HScrollBar();
            this.sc0 = new System.Windows.Forms.SplitContainer();
            this.sc1 = new System.Windows.Forms.SplitContainer();
            this.xAxisChart = new Trade.Base.Canvas();
            this.lvData = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSizeNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miSwapChart = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.panelBaseBar.SuspendLayout();
            this.panel2.SuspendLayout();
            this.sc0.Panel1.SuspendLayout();
            this.sc0.Panel2.SuspendLayout();
            this.sc0.SuspendLayout();
            this.sc1.SuspendLayout();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBaseBar
            // 
            this.panelBaseBar.Controls.Add(this.panel2);
            this.panelBaseBar.Controls.Add(this.sbCurrent);
            this.panelBaseBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBaseBar.Location = new System.Drawing.Point(0, 179);
            this.panelBaseBar.Name = "panelBaseBar";
            this.panelBaseBar.Size = new System.Drawing.Size(352, 22);
            this.panelBaseBar.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tbItemSize);
            this.panel2.Controls.Add(this.btnItemSizeMinus);
            this.panel2.Controls.Add(this.btnItemSizePlus);
            this.panel2.Controls.Add(this.btnMenu);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(196, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(156, 22);
            this.panel2.TabIndex = 1;
            // 
            // tbItemSize
            // 
            this.tbItemSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbItemSize.Location = new System.Drawing.Point(28, 0);
            this.tbItemSize.Name = "tbItemSize";
            this.tbItemSize.ReadOnly = true;
            this.tbItemSize.Size = new System.Drawing.Size(51, 20);
            this.tbItemSize.TabIndex = 3;
            this.tbItemSize.Text = "12";
            this.tbItemSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnItemSizeMinus
            // 
            this.btnItemSizeMinus.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnItemSizeMinus.Location = new System.Drawing.Point(0, 0);
            this.btnItemSizeMinus.Name = "btnItemSizeMinus";
            this.btnItemSizeMinus.Size = new System.Drawing.Size(28, 22);
            this.btnItemSizeMinus.TabIndex = 2;
            this.btnItemSizeMinus.Text = "-";
            this.btnItemSizeMinus.UseVisualStyleBackColor = true;
            this.btnItemSizeMinus.Click += new System.EventHandler(this.btnItemSizeMinus_Click);
            // 
            // btnItemSizePlus
            // 
            this.btnItemSizePlus.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnItemSizePlus.Location = new System.Drawing.Point(79, 0);
            this.btnItemSizePlus.Name = "btnItemSizePlus";
            this.btnItemSizePlus.Size = new System.Drawing.Size(26, 22);
            this.btnItemSizePlus.TabIndex = 0;
            this.btnItemSizePlus.Text = "+";
            this.btnItemSizePlus.UseVisualStyleBackColor = true;
            this.btnItemSizePlus.Click += new System.EventHandler(this.btnItemSizePlus_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMenu.Location = new System.Drawing.Point(105, 0);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(51, 22);
            this.btnMenu.TabIndex = 1;
            this.btnMenu.Text = "Menu";
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // sbCurrent
            // 
            this.sbCurrent.AllowDrop = true;
            this.sbCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sbCurrent.Location = new System.Drawing.Point(0, 2);
            this.sbCurrent.Name = "sbCurrent";
            this.sbCurrent.Size = new System.Drawing.Size(195, 17);
            this.sbCurrent.TabIndex = 0;
            this.sbCurrent.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sbCurrent_Scroll);
            // 
            // sc0
            // 
            this.sc0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sc0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc0.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.sc0.Location = new System.Drawing.Point(0, 0);
            this.sc0.Name = "sc0";
            // 
            // sc0.Panel1
            // 
            this.sc0.Panel1.Controls.Add(this.sc1);
            this.sc0.Panel1.Controls.Add(this.xAxisChart);
            this.sc0.Panel1.Controls.Add(this.panelBaseBar);
            // 
            // sc0.Panel2
            // 
            this.sc0.Panel2.Controls.Add(this.lvData);
            this.sc0.Size = new System.Drawing.Size(503, 201);
            this.sc0.SplitterDistance = 352;
            this.sc0.SplitterWidth = 1;
            this.sc0.TabIndex = 0;
            // 
            // sc1
            // 
            this.sc1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sc1.Location = new System.Drawing.Point(0, 0);
            this.sc1.Name = "sc1";
            this.sc1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sc1.Panel1Collapsed = true;
            this.sc1.Size = new System.Drawing.Size(352, 139);
            this.sc1.SplitterDistance = 69;
            this.sc1.TabIndex = 2;
            // 
            // xAxisChart
            // 
            this.xAxisChart.Current = 0;
            this.xAxisChart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.xAxisChart.EnableXYPosition = false;
            this.xAxisChart.ItemWidth = 1;
            this.xAxisChart.Location = new System.Drawing.Point(0, 139);
            this.xAxisChart.MinimumSize = new System.Drawing.Size(200, 20);
            this.xAxisChart.Name = "xAxisChart";
            this.xAxisChart.Selected = false;
            this.xAxisChart.Size = new System.Drawing.Size(352, 40);
            this.xAxisChart.TabIndex = 1;
            this.xAxisChart.XYPositionRefreshInterval = 100;
            this.xAxisChart.ChartSelected += new System.EventHandler(this.xAxisChart_ChartSelected);
            this.xAxisChart.ChartXY += new Trade.Base.ChartXY(this.OnChartXY);
            this.xAxisChart.CanvasMouseMove += new System.Windows.Input.MouseEventHandler(this.PositionXY);
            // 
            // lvData
            // 
            this.lvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            listViewGroup2.Header = "aaa";
            listViewGroup2.Name = "listViewGroup1";
            this.lvData.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup2});
            listViewItem4.Group = listViewGroup2;
            listViewItem5.Group = listViewGroup2;
            listViewItem6.Group = listViewGroup2;
            this.lvData.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.lvData.Location = new System.Drawing.Point(0, 0);
            this.lvData.Name = "lvData";
            this.lvData.Size = new System.Drawing.Size(146, 197);
            this.lvData.TabIndex = 1;
            this.lvData.UseCompatibleStateImageBehavior = false;
            this.lvData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Detail";
            this.columnHeader2.Width = 188;
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSizeNormal,
            this.toolStripMenuItem1,
            this.miData,
            this.toolStripSeparator1,
            this.miSwapChart,
            this.miDeselectAll});
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(137, 104);
            this.menu.Opening += new System.ComponentModel.CancelEventHandler(this.menu_Opening);
            // 
            // miSizeNormal
            // 
            this.miSizeNormal.Name = "miSizeNormal";
            this.miSizeNormal.Size = new System.Drawing.Size(136, 22);
            this.miSizeNormal.Text = "Normal Zoom";
            this.miSizeNormal.Click += new System.EventHandler(this.miSizeNormal_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(133, 6);
            // 
            // miData
            // 
            this.miData.Name = "miData";
            this.miData.Size = new System.Drawing.Size(136, 22);
            this.miData.Text = "Data";
            this.miData.Click += new System.EventHandler(this.miData_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            // 
            // miSwapChart
            // 
            this.miSwapChart.Name = "miSwapChart";
            this.miSwapChart.Size = new System.Drawing.Size(136, 22);
            this.miSwapChart.Text = "Swap Chart";
            this.miSwapChart.Click += new System.EventHandler(this.miSwapChart_Click);
            // 
            // miDeselectAll
            // 
            this.miDeselectAll.Name = "miDeselectAll";
            this.miDeselectAll.Size = new System.Drawing.Size(136, 22);
            this.miDeselectAll.Text = "Deselect All";
            this.miDeselectAll.Click += new System.EventHandler(this.miDeselectAll_Click);
            // 
            // SChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sc0);
            this.MinimumSize = new System.Drawing.Size(400, 100);
            this.Name = "SChart";
            this.Size = new System.Drawing.Size(503, 201);
            this.panelBaseBar.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.sc0.Panel1.ResumeLayout(false);
            this.sc0.Panel2.ResumeLayout(false);
            this.sc0.ResumeLayout(false);
            this.sc1.ResumeLayout(false);
            this.menu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBaseBar;
        private System.Windows.Forms.HScrollBar sbCurrent;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbItemSize;
        private System.Windows.Forms.Button btnItemSizeMinus;
        private System.Windows.Forms.Button btnItemSizePlus;
        private System.Windows.Forms.Button btnMenu;
        
        private Canvas xAxisChart;
        private System.Windows.Forms.SplitContainer sc0;
        private System.Windows.Forms.SplitContainer sc1;
        private System.Windows.Forms.ContextMenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem miSizeNormal;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miSwapChart;
        private System.Windows.Forms.ToolStripMenuItem miDeselectAll;
        private System.Windows.Forms.ToolStripMenuItem miData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ListView lvData;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}
