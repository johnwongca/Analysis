﻿namespace Algorithm.Core.Forms
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.miForceDataRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.miSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miSaveAs = new System.Windows.Forms.ToolStripMenuItem();
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
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.labelIsLoading = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miForceDataRefresh,
            this.toolStripSeparator6,
            this.miSave,
            this.miSaveAs});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "Save";
            // 
            // miForceDataRefresh
            // 
            this.miForceDataRefresh.Name = "miForceDataRefresh";
            this.miForceDataRefresh.Size = new System.Drawing.Size(172, 22);
            this.miForceDataRefresh.Text = "Force Data Refresh";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(169, 6);
            // 
            // miSave
            // 
            this.miSave.Name = "miSave";
            this.miSave.Size = new System.Drawing.Size(172, 22);
            this.miSave.Text = "Save";
            // 
            // miSaveAs
            // 
            this.miSaveAs.Name = "miSaveAs";
            this.miSaveAs.Size = new System.Drawing.Size(172, 22);
            this.miSaveAs.Text = "Save As";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // QuoteSearch
            // 
            this.QuoteSearch.Name = "QuoteSearch";
            this.QuoteSearch.Size = new System.Drawing.Size(80, 25);
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
            this.IntervalTypeMenu.Size = new System.Drawing.Size(58, 22);
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
            this.labelIsLoading});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(850, 25);
            this.toolStrip2.TabIndex = 4;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(49, 22);
            this.toolStripLabel1.Text = "Interval:";
            // 
            // nInterval
            // 
            this.nInterval.Name = "nInterval";
            this.nInterval.Size = new System.Drawing.Size(41, 22);
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
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(64, 22);
            this.toolStripLabel3.Text = "Algorithm:";
            // 
            // AlgorithmMenu
            // 
            this.AlgorithmMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AlgorithmMenu.Image = ((System.Drawing.Image)(resources.GetObject("AlgorithmMenu.Image")));
            this.AlgorithmMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AlgorithmMenu.Name = "AlgorithmMenu";
            this.AlgorithmMenu.Size = new System.Drawing.Size(47, 22);
            this.AlgorithmMenu.Text = "Basic";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(37, 22);
            this.toolStripLabel2.Text = "Scale:";
            // 
            // nScale
            // 
            this.nScale.Name = "nScale";
            this.nScale.Size = new System.Drawing.Size(41, 22);
            this.nScale.Text = "0";
            this.nScale.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnMore
            // 
            this.btnMore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMore.Image = ((System.Drawing.Image)(resources.GetObject("btnMore.Image")));
            this.btnMore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(48, 22);
            this.btnMore.Text = "More...";
            this.btnMore.Click += new System.EventHandler(this.btnMore_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 25);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(850, 396);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 404);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(850, 17);
            this.hScrollBar1.TabIndex = 7;
            // 
            // labelIsLoading
            // 
            this.labelIsLoading.Name = "labelIsLoading";
            this.labelIsLoading.Size = new System.Drawing.Size(68, 22);
            this.labelIsLoading.Text = "...Loading...";
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 421);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.toolStrip2);
            this.Name = "ChartForm";
            this.Text = "ChartForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChartForm_FormClosing);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem miForceDataRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
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
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
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


    }
}