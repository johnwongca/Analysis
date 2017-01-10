namespace Console
{
	partial class FormMain
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
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miAnalyze = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowsWidnows = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowsCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowSeparatorCloseAll = new System.Windows.Forms.ToolStripSeparator();
            this.miWindowsSeparatorShowTabs = new System.Windows.Forms.ToolStripSeparator();
            this.miWindowsShowTabs = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowsShowMultiLineTabs = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowsSeparatorLayout = new System.Windows.Forms.ToolStripSeparator();
            this.miWindowsCascade = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowsHorizontal = new System.Windows.Forms.ToolStripMenuItem();
            this.miWindowsVertical = new System.Windows.Forms.ToolStripMenuItem();
            this.miTest = new System.Windows.Forms.ToolStripMenuItem();
            this.mitest1 = new System.Windows.Forms.ToolStripMenuItem();
            this.miTestForm = new System.Windows.Forms.ToolStripMenuItem();
            this.testAPICursorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.penalWindowTabs = new System.Windows.Forms.Panel();
            this.tabChildFormControl = new System.Windows.Forms.TabControl();
            this.cmTabChildFormControl = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miTabClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miTransMain = new System.Windows.Forms.ToolStripMenuItem();
            this.miAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.penalWindowTabs.SuspendLayout();
            this.cmTabChildFormControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miTransMain,
            this.miWindows,
            this.miTest});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(831, 24);
            this.mainMenu.TabIndex = 1;
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAnalyze,
            this.toolStripMenuItem1,
            this.miFileExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(35, 20);
            this.miFile.Text = "File";
            // 
            // miAnalyze
            // 
            this.miAnalyze.Name = "miAnalyze";
            this.miAnalyze.Size = new System.Drawing.Size(152, 22);
            this.miAnalyze.Text = "Analyze";
            this.miAnalyze.Click += new System.EventHandler(this.miAnalyze_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.Size = new System.Drawing.Size(152, 22);
            this.miFileExit.Text = "Exit";
            this.miFileExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miWindows
            // 
            this.miWindows.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miWindowsWidnows,
            this.miWindowsSeparatorShowTabs,
            this.miWindowsShowTabs,
            this.miWindowsShowMultiLineTabs,
            this.miWindowsSeparatorLayout,
            this.miWindowsCascade,
            this.miWindowsHorizontal,
            this.miWindowsVertical});
            this.miWindows.Name = "miWindows";
            this.miWindows.Size = new System.Drawing.Size(62, 20);
            this.miWindows.Text = "Windows";
            this.miWindows.DropDownOpening += new System.EventHandler(this.miWindows_DropDownOpening);
            // 
            // miWindowsWidnows
            // 
            this.miWindowsWidnows.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miWindowsCloseAll,
            this.miWindowSeparatorCloseAll});
            this.miWindowsWidnows.Name = "miWindowsWidnows";
            this.miWindowsWidnows.Size = new System.Drawing.Size(174, 22);
            this.miWindowsWidnows.Text = "Windows";
            this.miWindowsWidnows.DropDownOpening += new System.EventHandler(this.miWindowsWidnows_DropDownOpening);
            // 
            // miWindowsCloseAll
            // 
            this.miWindowsCloseAll.Name = "miWindowsCloseAll";
            this.miWindowsCloseAll.Size = new System.Drawing.Size(114, 22);
            this.miWindowsCloseAll.Text = "Close All";
            this.miWindowsCloseAll.Click += new System.EventHandler(this.miWindowCloseAll_Click);
            // 
            // miWindowSeparatorCloseAll
            // 
            this.miWindowSeparatorCloseAll.Name = "miWindowSeparatorCloseAll";
            this.miWindowSeparatorCloseAll.Size = new System.Drawing.Size(111, 6);
            // 
            // miWindowsSeparatorShowTabs
            // 
            this.miWindowsSeparatorShowTabs.Name = "miWindowsSeparatorShowTabs";
            this.miWindowsSeparatorShowTabs.Size = new System.Drawing.Size(171, 6);
            // 
            // miWindowsShowTabs
            // 
            this.miWindowsShowTabs.Checked = true;
            this.miWindowsShowTabs.CheckOnClick = true;
            this.miWindowsShowTabs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miWindowsShowTabs.Name = "miWindowsShowTabs";
            this.miWindowsShowTabs.Size = new System.Drawing.Size(174, 22);
            this.miWindowsShowTabs.Text = "Show Tabs";
            this.miWindowsShowTabs.Click += new System.EventHandler(this.miWindowShowTabs_Click);
            // 
            // miWindowsShowMultiLineTabs
            // 
            this.miWindowsShowMultiLineTabs.CheckOnClick = true;
            this.miWindowsShowMultiLineTabs.Name = "miWindowsShowMultiLineTabs";
            this.miWindowsShowMultiLineTabs.Size = new System.Drawing.Size(174, 22);
            this.miWindowsShowMultiLineTabs.Text = "Show Multi-Line Tabs";
            this.miWindowsShowMultiLineTabs.Visible = false;
            this.miWindowsShowMultiLineTabs.Click += new System.EventHandler(this.miWindowsShowMultiLineTabs_Click);
            // 
            // miWindowsSeparatorLayout
            // 
            this.miWindowsSeparatorLayout.Name = "miWindowsSeparatorLayout";
            this.miWindowsSeparatorLayout.Size = new System.Drawing.Size(171, 6);
            // 
            // miWindowsCascade
            // 
            this.miWindowsCascade.Name = "miWindowsCascade";
            this.miWindowsCascade.Size = new System.Drawing.Size(174, 22);
            this.miWindowsCascade.Text = "Cascade";
            this.miWindowsCascade.Click += new System.EventHandler(this.miWindowCascade_Click);
            // 
            // miWindowsHorizontal
            // 
            this.miWindowsHorizontal.Name = "miWindowsHorizontal";
            this.miWindowsHorizontal.Size = new System.Drawing.Size(174, 22);
            this.miWindowsHorizontal.Text = "Tile Horizontal";
            this.miWindowsHorizontal.Click += new System.EventHandler(this.miWindowHorizontal_Click);
            // 
            // miWindowsVertical
            // 
            this.miWindowsVertical.Name = "miWindowsVertical";
            this.miWindowsVertical.Size = new System.Drawing.Size(174, 22);
            this.miWindowsVertical.Text = "Tile Vertical";
            this.miWindowsVertical.Click += new System.EventHandler(this.miWindowVertical_Click);
            // 
            // miTest
            // 
            this.miTest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mitest1,
            this.miTestForm,
            this.testAPICursorToolStripMenuItem});
            this.miTest.Name = "miTest";
            this.miTest.Size = new System.Drawing.Size(40, 20);
            this.miTest.Text = "Test";
            // 
            // mitest1
            // 
            this.mitest1.Name = "mitest1";
            this.mitest1.Size = new System.Drawing.Size(152, 22);
            this.mitest1.Text = "Test1 New Form";
            this.mitest1.Click += new System.EventHandler(this.miTest1_Click);
            // 
            // miTestForm
            // 
            this.miTestForm.Name = "miTestForm";
            this.miTestForm.Size = new System.Drawing.Size(152, 22);
            this.miTestForm.Text = "Test Form";
            this.miTestForm.Click += new System.EventHandler(this.miTestForm_Click);
            // 
            // testAPICursorToolStripMenuItem
            // 
            this.testAPICursorToolStripMenuItem.Name = "testAPICursorToolStripMenuItem";
            this.testAPICursorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.testAPICursorToolStripMenuItem.Text = "Test API Cursor";
            this.testAPICursorToolStripMenuItem.Click += new System.EventHandler(this.testAPICursorToolStripMenuItem_Click);
            // 
            // penalWindowTabs
            // 
            this.penalWindowTabs.Controls.Add(this.tabChildFormControl);
            this.penalWindowTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this.penalWindowTabs.Location = new System.Drawing.Point(0, 24);
            this.penalWindowTabs.Name = "penalWindowTabs";
            this.penalWindowTabs.Size = new System.Drawing.Size(831, 22);
            this.penalWindowTabs.TabIndex = 3;
            this.penalWindowTabs.Visible = false;
            // 
            // tabChildFormControl
            // 
            this.tabChildFormControl.AllowDrop = true;
            this.tabChildFormControl.ContextMenuStrip = this.cmTabChildFormControl;
            this.tabChildFormControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabChildFormControl.HotTrack = true;
            this.tabChildFormControl.Location = new System.Drawing.Point(0, 0);
            this.tabChildFormControl.Name = "tabChildFormControl";
            this.tabChildFormControl.SelectedIndex = 0;
            this.tabChildFormControl.Size = new System.Drawing.Size(831, 22);
            this.tabChildFormControl.TabIndex = 0;
            this.tabChildFormControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabChildFormControl_Selected);
            this.tabChildFormControl.DragOver += new System.Windows.Forms.DragEventHandler(this.tabChildFormControl_DragOver);
            this.tabChildFormControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabChildFormControl_MouseDown);
            // 
            // cmTabChildFormControl
            // 
            this.cmTabChildFormControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miTabClose});
            this.cmTabChildFormControl.Name = "cmTabChildFormControl";
            this.cmTabChildFormControl.Size = new System.Drawing.Size(101, 26);
            this.cmTabChildFormControl.Opening += new System.ComponentModel.CancelEventHandler(this.cmTabChildFormControl_Opening);
            // 
            // miTabClose
            // 
            this.miTabClose.Name = "miTabClose";
            this.miTabClose.Size = new System.Drawing.Size(100, 22);
            this.miTabClose.Text = "Close";
            this.miTabClose.Click += new System.EventHandler(this.miTabClose_Click);
            // 
            // miTransMain
            // 
            this.miTransMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAccount,
            this.transactionToolStripMenuItem});
            this.miTransMain.Name = "miTransMain";
            this.miTransMain.Size = new System.Drawing.Size(75, 20);
            this.miTransMain.Text = "Transaction";
            // 
            // miAccount
            // 
            this.miAccount.Name = "miAccount";
            this.miAccount.Size = new System.Drawing.Size(152, 22);
            this.miAccount.Text = "Account";
            this.miAccount.Click += new System.EventHandler(this.miAccount_Click);
            // 
            // transactionToolStripMenuItem
            // 
            this.transactionToolStripMenuItem.Name = "transactionToolStripMenuItem";
            this.transactionToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.transactionToolStripMenuItem.Text = "Transaction";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 595);
            this.Controls.Add(this.penalWindowTabs);
            this.Controls.Add(this.mainMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Console";
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.penalWindowTabs.ResumeLayout(false);
            this.cmTabChildFormControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem miFile;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem miFileExit;
		private System.Windows.Forms.ToolStripMenuItem miWindows;
		private System.Windows.Forms.ToolStripSeparator miWindowsSeparatorLayout;
		private System.Windows.Forms.ToolStripMenuItem miWindowsCascade;
		private System.Windows.Forms.ToolStripMenuItem miWindowsHorizontal;
		private System.Windows.Forms.ToolStripMenuItem miWindowsVertical;
		private System.Windows.Forms.ToolStripSeparator miWindowsSeparatorShowTabs;
		private System.Windows.Forms.ToolStripMenuItem miWindowsShowTabs;
		private System.Windows.Forms.Panel penalWindowTabs;
		private System.Windows.Forms.TabControl tabChildFormControl;
		private System.Windows.Forms.ToolStripMenuItem miWindowsWidnows;
		private System.Windows.Forms.ToolStripMenuItem miWindowsCloseAll;
		private System.Windows.Forms.ToolStripSeparator miWindowSeparatorCloseAll;
		private System.Windows.Forms.ToolStripMenuItem miTest;
		private System.Windows.Forms.ToolStripMenuItem miWindowsShowMultiLineTabs;
		private System.Windows.Forms.ContextMenuStrip cmTabChildFormControl;
		private System.Windows.Forms.ToolStripMenuItem miTabClose;
        private System.Windows.Forms.ToolStripMenuItem mitest1;
        private System.Windows.Forms.ToolStripMenuItem miTestForm;
        private System.Windows.Forms.ToolStripMenuItem testAPICursorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miAnalyze;
        private System.Windows.Forms.ToolStripMenuItem miTransMain;
        private System.Windows.Forms.ToolStripMenuItem miAccount;
        private System.Windows.Forms.ToolStripMenuItem transactionToolStripMenuItem;
	}
}

