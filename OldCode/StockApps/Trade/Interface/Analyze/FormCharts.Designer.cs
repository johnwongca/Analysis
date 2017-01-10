namespace Trade.Interface.Analyze
{
    partial class FormCharts
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
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDaily = new System.Windows.Forms.ToolStripMenuItem();
            this.miWeekly = new System.Windows.Forms.ToolStripMenuItem();
            this.miMonthly = new System.Windows.Forms.ToolStripMenuItem();
            this.miQuarterly = new System.Windows.Forms.ToolStripMenuItem();
            this.miYearly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miDays = new System.Windows.Forms.ToolStripMenuItem();
            this.chart = new Trade.Base.SChart();
            this.miHalfYear = new System.Windows.Forms.ToolStripMenuItem();
            this.cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // cms
            // 
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDaily,
            this.miWeekly,
            this.miMonthly,
            this.miQuarterly,
            this.miHalfYear,
            this.miYearly,
            this.toolStripMenuItem1,
            this.miDays});
            this.cms.Name = "contextMenuStrip1";
            this.cms.Size = new System.Drawing.Size(153, 186);
            // 
            // miDaily
            // 
            this.miDaily.Checked = true;
            this.miDaily.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miDaily.Name = "miDaily";
            this.miDaily.Size = new System.Drawing.Size(152, 22);
            this.miDaily.Text = "Daily";
            this.miDaily.Click += new System.EventHandler(this.miDaily_Click);
            // 
            // miWeekly
            // 
            this.miWeekly.Name = "miWeekly";
            this.miWeekly.Size = new System.Drawing.Size(152, 22);
            this.miWeekly.Text = "Weekly";
            this.miWeekly.Click += new System.EventHandler(this.miDaily_Click);
            // 
            // miMonthly
            // 
            this.miMonthly.Name = "miMonthly";
            this.miMonthly.Size = new System.Drawing.Size(152, 22);
            this.miMonthly.Text = "Monthly";
            this.miMonthly.Click += new System.EventHandler(this.miDaily_Click);
            // 
            // miQuarterly
            // 
            this.miQuarterly.Name = "miQuarterly";
            this.miQuarterly.Size = new System.Drawing.Size(152, 22);
            this.miQuarterly.Text = "Quarterly";
            this.miQuarterly.Click += new System.EventHandler(this.miDaily_Click);
            // 
            // miYearly
            // 
            this.miYearly.Name = "miYearly";
            this.miYearly.Size = new System.Drawing.Size(152, 22);
            this.miYearly.Text = "Yearly";
            this.miYearly.Click += new System.EventHandler(this.miDaily_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // miDays
            // 
            this.miDays.Name = "miDays";
            this.miDays.Size = new System.Drawing.Size(152, 22);
            this.miDays.Text = "5 Days";
            this.miDays.Click += new System.EventHandler(this.miDaily_Click);
            // 
            // chart
            // 
            this.chart.Caption = "xAxisChart";
            this.chart.Current = 0;
            this.chart.DefaultItemWidth = 6;
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.ItemWidth = 6;
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.MaxItemWidth = 25;
            this.chart.MinimumSize = new System.Drawing.Size(400, 100);
            this.chart.MinItemWidth = 1;
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(914, 951);
            this.chart.TabIndex = 1;
            // 
            // miHalfYear
            // 
            this.miHalfYear.Name = "miHalfYear";
            this.miHalfYear.Size = new System.Drawing.Size(152, 22);
            this.miHalfYear.Text = "Half Year";
            this.miHalfYear.Click += new System.EventHandler(this.miDaily_Click);
            // 
            // FormCharts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 951);
            this.ContextMenuStrip = this.cms;
            this.Controls.Add(this.chart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormCharts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCharts_FormClosing);
            this.cms.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem miDaily;
        private Trade.Base.SChart chart;
        private System.Windows.Forms.ToolStripMenuItem miWeekly;
        private System.Windows.Forms.ToolStripMenuItem miMonthly;
        private System.Windows.Forms.ToolStripMenuItem miYearly;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miDays;
        private System.Windows.Forms.ToolStripMenuItem miQuarterly;
        private System.Windows.Forms.ToolStripMenuItem miHalfYear;
    }
}