namespace Algorithm.Core.Forms
{
    partial class ChartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartForm));
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.miSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miCharts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.QuoteSearch = new System.Windows.Forms.ToolStripTextBox();
            this.IntervalTypeMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.minuteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.weekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.nInterval = new Algorithm.Core.Forms.ToolStripNumericUpDown();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.AlgorithmMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.nScale = new Algorithm.Core.Forms.ToolStripNumericUpDown();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMore = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.infoLabel = new System.Windows.Forms.ToolStripLabel();
            this.labelIsLoading = new System.Windows.Forms.ToolStripLabel();
            this.dpStartDate = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nLargeChange = new System.Windows.Forms.NumericUpDown();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStrip2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nLargeChange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSave,
            this.miSaveAs,
            this.toolStripMenuItem1,
            this.miCharts});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 23);
            this.toolStripDropDownButton1.Text = "Save";
            // 
            // miSave
            // 
            this.miSave.Name = "miSave";
            this.miSave.Size = new System.Drawing.Size(114, 22);
            this.miSave.Text = "Save";
            this.miSave.Click += new System.EventHandler(this.miSave_Click);
            // 
            // miSaveAs
            // 
            this.miSaveAs.Name = "miSaveAs";
            this.miSaveAs.Size = new System.Drawing.Size(114, 22);
            this.miSaveAs.Text = "Save As";
            this.miSaveAs.Click += new System.EventHandler(this.miSaveAs_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(111, 6);
            // 
            // miCharts
            // 
            this.miCharts.Name = "miCharts";
            this.miCharts.Size = new System.Drawing.Size(114, 22);
            this.miCharts.Text = "Charts";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 26);
            // 
            // QuoteSearch
            // 
            this.QuoteSearch.Name = "QuoteSearch";
            this.QuoteSearch.Size = new System.Drawing.Size(80, 26);
            this.QuoteSearch.TextChanged += new System.EventHandler(this.QuoteSearch_TextChanged);
            // 
            // IntervalTypeMenu
            // 
            this.IntervalTypeMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.IntervalTypeMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minuteToolStripMenuItem,
            this.dayToolStripMenuItem,
            this.weekToolStripMenuItem,
            this.monthToolStripMenuItem,
            this.yearToolStripMenuItem});
            this.IntervalTypeMenu.Image = ((System.Drawing.Image)(resources.GetObject("IntervalTypeMenu.Image")));
            this.IntervalTypeMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.IntervalTypeMenu.Name = "IntervalTypeMenu";
            this.IntervalTypeMenu.Size = new System.Drawing.Size(58, 23);
            this.IntervalTypeMenu.Text = "Minute";
            // 
            // minuteToolStripMenuItem
            // 
            this.minuteToolStripMenuItem.Checked = true;
            this.minuteToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.minuteToolStripMenuItem.Name = "minuteToolStripMenuItem";
            this.minuteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.minuteToolStripMenuItem.Text = "Minutes";
            this.minuteToolStripMenuItem.Click += new System.EventHandler(this.IntervalChange);
            // 
            // dayToolStripMenuItem
            // 
            this.dayToolStripMenuItem.Name = "dayToolStripMenuItem";
            this.dayToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.dayToolStripMenuItem.Text = "Days";
            this.dayToolStripMenuItem.Click += new System.EventHandler(this.IntervalChange);
            // 
            // weekToolStripMenuItem
            // 
            this.weekToolStripMenuItem.Name = "weekToolStripMenuItem";
            this.weekToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.weekToolStripMenuItem.Text = "Weeks";
            this.weekToolStripMenuItem.Click += new System.EventHandler(this.IntervalChange);
            // 
            // monthToolStripMenuItem
            // 
            this.monthToolStripMenuItem.Name = "monthToolStripMenuItem";
            this.monthToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.monthToolStripMenuItem.Text = "Months";
            this.monthToolStripMenuItem.Click += new System.EventHandler(this.IntervalChange);
            // 
            // yearToolStripMenuItem
            // 
            this.yearToolStripMenuItem.Name = "yearToolStripMenuItem";
            this.yearToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.yearToolStripMenuItem.Text = "Years";
            this.yearToolStripMenuItem.Click += new System.EventHandler(this.IntervalChange);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSeparator4,
            this.QuoteSearch,
            this.IntervalTypeMenu,
            this.toolStripLabel1,
            this.nInterval,
            this.toolStripSeparator3,
            this.toolStripLabel3,
            this.AlgorithmMenu,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.nScale,
            this.toolStripSeparator1,
            this.btnMore,
            this.toolStripSeparator5,
            this.infoLabel,
            this.labelIsLoading});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(909, 26);
            this.toolStrip2.TabIndex = 4;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(49, 23);
            this.toolStripLabel1.Text = "Interval:";
            // 
            // nInterval
            // 
            this.nInterval.Name = "nInterval";
            this.nInterval.Size = new System.Drawing.Size(41, 23);
            this.nInterval.Text = "1";
            this.nInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nInterval.ValueChanged += new System.EventHandler(this.nInterval_ValueChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 26);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(64, 23);
            this.toolStripLabel3.Text = "Algorithm:";
            // 
            // AlgorithmMenu
            // 
            this.AlgorithmMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AlgorithmMenu.Image = ((System.Drawing.Image)(resources.GetObject("AlgorithmMenu.Image")));
            this.AlgorithmMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AlgorithmMenu.Name = "AlgorithmMenu";
            this.AlgorithmMenu.Size = new System.Drawing.Size(47, 23);
            this.AlgorithmMenu.Text = "Basic";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 26);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(37, 23);
            this.toolStripLabel2.Text = "Scale:";
            // 
            // nScale
            // 
            this.nScale.Name = "nScale";
            this.nScale.Size = new System.Drawing.Size(41, 23);
            this.nScale.Text = "0";
            this.nScale.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nScale.ValueChanged += new System.EventHandler(this.nScale_ValueChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 26);
            // 
            // btnMore
            // 
            this.btnMore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMore.Image = ((System.Drawing.Image)(resources.GetObject("btnMore.Image")));
            this.btnMore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(48, 23);
            this.btnMore.Text = "More...";
            this.btnMore.Click += new System.EventHandler(this.btnMore_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 26);
            // 
            // infoLabel
            // 
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(0, 23);
            this.infoLabel.Visible = false;
            // 
            // labelIsLoading
            // 
            this.labelIsLoading.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.labelIsLoading.Name = "labelIsLoading";
            this.labelIsLoading.Size = new System.Drawing.Size(61, 23);
            this.labelIsLoading.Text = "Loading...";
            // 
            // dpStartDate
            // 
            this.dpStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dpStartDate.CustomFormat = "yyyy-MM-dd";
            this.dpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpStartDate.Location = new System.Drawing.Point(817, 2);
            this.dpStartDate.MaxDate = new System.DateTime(2020, 12, 31, 0, 0, 0, 0);
            this.dpStartDate.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dpStartDate.Name = "dpStartDate";
            this.dpStartDate.ShowUpDown = true;
            this.dpStartDate.Size = new System.Drawing.Size(92, 20);
            this.dpStartDate.TabIndex = 9;
            this.dpStartDate.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.nLargeChange);
            this.panel1.Controls.Add(this.hScrollBar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 441);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(909, 20);
            this.panel1.TabIndex = 10;
            // 
            // nLargeChange
            // 
            this.nLargeChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nLargeChange.Location = new System.Drawing.Point(847, 0);
            this.nLargeChange.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nLargeChange.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nLargeChange.Name = "nLargeChange";
            this.nLargeChange.Size = new System.Drawing.Size(59, 20);
            this.nLargeChange.TabIndex = 9;
            this.nLargeChange.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nLargeChange.ValueChanged += new System.EventHandler(this.nLargeChange_ValueChanged);
            // 
            // hScrollBar
            // 
            this.hScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar.Location = new System.Drawing.Point(0, 0);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(844, 20);
            this.hScrollBar.TabIndex = 8;
            this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
            // 
            // chart1
            // 
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 26);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(909, 415);
            this.chart1.TabIndex = 11;
            this.chart1.Text = "chart1";
            this.chart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseDown);
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            this.chart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseUp);
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 461);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dpStartDate);
            this.Controls.Add(this.toolStrip2);
            this.Name = "ChartForm";
            this.Text = "ChartForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChartForm_FormClosing);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nLargeChange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem miSave;
        private System.Windows.Forms.ToolStripMenuItem miSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripTextBox QuoteSearch;
        private System.Windows.Forms.ToolStripDropDownButton IntervalTypeMenu;
        private System.Windows.Forms.ToolStripMenuItem minuteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem weekToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yearToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private ToolStripNumericUpDown nInterval;
        private ToolStripNumericUpDown nScale;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnMore;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripDropDownButton AlgorithmMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel labelIsLoading;
        private System.Windows.Forms.DateTimePicker dpStartDate;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miCharts;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown nLargeChange;
        private System.Windows.Forms.HScrollBar hScrollBar;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ToolStripLabel infoLabel;


    }
}