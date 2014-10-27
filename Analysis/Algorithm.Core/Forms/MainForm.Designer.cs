namespace Algorithm.Core.Forms
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnChart = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.miCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miSymbols = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnChart,
            this.miWindows,
            this.miSymbols});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(291, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnChart
            // 
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(48, 20);
            this.btnChart.Text = "Chart";
            this.btnChart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // miWindows
            // 
            this.miWindows.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCloseAll,
            this.toolStripMenuItem1});
            this.miWindows.Name = "miWindows";
            this.miWindows.Size = new System.Drawing.Size(68, 20);
            this.miWindows.Text = "Windows";
            this.miWindows.DropDownOpening += new System.EventHandler(this.miWindows_DropDownOpening);
            // 
            // miCloseAll
            // 
            this.miCloseAll.Name = "miCloseAll";
            this.miCloseAll.Size = new System.Drawing.Size(120, 22);
            this.miCloseAll.Text = "Close All";
            this.miCloseAll.Click += new System.EventHandler(this.miCloseAll_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(117, 6);
            // 
            // miSymbols
            // 
            this.miSymbols.Name = "miSymbols";
            this.miSymbols.Size = new System.Drawing.Size(64, 20);
            this.miSymbols.Text = "Symbols";
            this.miSymbols.Visible = false;
            this.miSymbols.Click += new System.EventHandler(this.miSymbols_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(291, 26);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "A!";
            this.TopMost = true;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miSymbols;
        private System.Windows.Forms.ToolStripMenuItem miWindows;
        private System.Windows.Forms.ToolStripMenuItem miCloseAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem btnChart;
    }
}