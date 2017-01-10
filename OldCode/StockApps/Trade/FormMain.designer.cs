namespace Trade
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
            System.Windows.Forms.ToolStripMenuItem miSymbol;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miImport = new System.Windows.Forms.ToolStripMenuItem();
            this.miImportProvider = new System.Windows.Forms.ToolStripMenuItem();
            this.miUnprocessedFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.miArchivedFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miTransMain = new System.Windows.Forms.ToolStripMenuItem();
            this.miAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.miTransaction = new System.Windows.Forms.ToolStripMenuItem();
            this.miAnalyzeMain = new System.Windows.Forms.ToolStripMenuItem();
            this.miExprorer = new System.Windows.Forms.ToolStripMenuItem();
            this.miAnalyzeTest = new System.Windows.Forms.ToolStripMenuItem();
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
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miTabSort = new System.Windows.Forms.ToolStripMenuItem();
            this.bnMain = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lbOpeningStatus = new System.Windows.Forms.ToolStripLabel();
            this.bwFreeResource = new System.ComponentModel.BackgroundWorker();
            this.tmFreeResource = new System.Windows.Forms.Timer(this.components);
            miSymbol = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.penalWindowTabs.SuspendLayout();
            this.cmTabChildFormControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bnMain)).BeginInit();
            this.bnMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // miSymbol
            // 
            miSymbol.Name = "miSymbol";
            miSymbol.Size = new System.Drawing.Size(130, 22);
            miSymbol.Text = "Symbol";
            miSymbol.Click += new System.EventHandler(this.miSymbol_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miTransMain,
            this.miAnalyzeMain,
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
            this.miImport,
            this.toolStripMenuItem1,
            this.miFileExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(35, 20);
            this.miFile.Text = "File";
            // 
            // miImport
            // 
            this.miImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miImportProvider,
            this.miUnprocessedFiles,
            this.toolStripMenuItem3,
            this.miArchivedFiles});
            this.miImport.Name = "miImport";
            this.miImport.Size = new System.Drawing.Size(106, 22);
            this.miImport.Text = "Import";
            // 
            // miImportProvider
            // 
            this.miImportProvider.Name = "miImportProvider";
            this.miImportProvider.Size = new System.Drawing.Size(160, 22);
            this.miImportProvider.Text = "Provider";
            this.miImportProvider.Click += new System.EventHandler(this.miImportProvider_Click);
            // 
            // miUnprocessedFiles
            // 
            this.miUnprocessedFiles.Name = "miUnprocessedFiles";
            this.miUnprocessedFiles.Size = new System.Drawing.Size(160, 22);
            this.miUnprocessedFiles.Text = "Unprocessed Files";
            this.miUnprocessedFiles.Click += new System.EventHandler(this.miUnprocessedFiles_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(157, 6);
            // 
            // miArchivedFiles
            // 
            this.miArchivedFiles.Name = "miArchivedFiles";
            this.miArchivedFiles.Size = new System.Drawing.Size(160, 22);
            this.miArchivedFiles.Text = "Archived Files";
            this.miArchivedFiles.Click += new System.EventHandler(this.miArchivedFiles_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(103, 6);
            // 
            // miFileExit
            // 
            this.miFileExit.Name = "miFileExit";
            this.miFileExit.Size = new System.Drawing.Size(106, 22);
            this.miFileExit.Text = "Exit";
            this.miFileExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miTransMain
            // 
            this.miTransMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAccount,
            miSymbol,
            this.miTransaction});
            this.miTransMain.Name = "miTransMain";
            this.miTransMain.Size = new System.Drawing.Size(75, 20);
            this.miTransMain.Text = "Transaction";
            // 
            // miAccount
            // 
            this.miAccount.Name = "miAccount";
            this.miAccount.Size = new System.Drawing.Size(130, 22);
            this.miAccount.Text = "Account";
            this.miAccount.Click += new System.EventHandler(this.miAccount_Click);
            // 
            // miTransaction
            // 
            this.miTransaction.Name = "miTransaction";
            this.miTransaction.Size = new System.Drawing.Size(130, 22);
            this.miTransaction.Text = "Transaction";
            this.miTransaction.Click += new System.EventHandler(this.miTransaction_Click);
            // 
            // miAnalyzeMain
            // 
            this.miAnalyzeMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miExprorer,
            this.miAnalyzeTest});
            this.miAnalyzeMain.Name = "miAnalyzeMain";
            this.miAnalyzeMain.Size = new System.Drawing.Size(57, 20);
            this.miAnalyzeMain.Text = "Analyze";
            // 
            // miExprorer
            // 
            this.miExprorer.Name = "miExprorer";
            this.miExprorer.Size = new System.Drawing.Size(116, 22);
            this.miExprorer.Text = "Exprorer";
            this.miExprorer.Click += new System.EventHandler(this.miExprorer_Click);
            // 
            // miAnalyzeTest
            // 
            this.miAnalyzeTest.Name = "miAnalyzeTest";
            this.miAnalyzeTest.Size = new System.Drawing.Size(116, 22);
            this.miAnalyzeTest.Text = "Test";
            this.miAnalyzeTest.Click += new System.EventHandler(this.miAnalyzeTest_Click);
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
            this.miTabClose,
            this.toolStripMenuItem2,
            this.miTabSort});
            this.cmTabChildFormControl.Name = "cmTabChildFormControl";
            this.cmTabChildFormControl.Size = new System.Drawing.Size(101, 54);
            this.cmTabChildFormControl.Opening += new System.ComponentModel.CancelEventHandler(this.cmTabChildFormControl_Opening);
            // 
            // miTabClose
            // 
            this.miTabClose.Name = "miTabClose";
            this.miTabClose.Size = new System.Drawing.Size(100, 22);
            this.miTabClose.Text = "Close";
            this.miTabClose.Click += new System.EventHandler(this.miTabClose_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(97, 6);
            // 
            // miTabSort
            // 
            this.miTabSort.Name = "miTabSort";
            this.miTabSort.Size = new System.Drawing.Size(100, 22);
            this.miTabSort.Text = "Sort";
            this.miTabSort.Click += new System.EventHandler(this.miTabSort_Click);
            // 
            // bnMain
            // 
            this.bnMain.AddNewItem = this.bindingNavigatorAddNewItem;
            this.bnMain.CountItem = this.bindingNavigatorCountItem;
            this.bnMain.DeleteItem = this.bindingNavigatorDeleteItem;
            this.bnMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bnMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.toolStripSeparator2,
            this.btnSave,
            this.toolStripSeparator3,
            this.btnRefresh,
            this.toolStripSeparator1,
            this.lbOpeningStatus});
            this.bnMain.Location = new System.Drawing.Point(0, 570);
            this.bnMain.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bnMain.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bnMain.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bnMain.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bnMain.Name = "bnMain";
            this.bnMain.PositionItem = this.bindingNavigatorPositionItem;
            this.bnMain.Size = new System.Drawing.Size(831, 25);
            this.bnMain.TabIndex = 5;
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(36, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 21);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Enabled = false;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "&Save";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Visible = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Enabled = false;
            this.btnRefresh.Image = global::Trade.Properties.Resources.BMP_Refresh;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.White;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnRefresh.Text = "Refresh";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lbOpeningStatus
            // 
            this.lbOpeningStatus.Name = "lbOpeningStatus";
            this.lbOpeningStatus.Size = new System.Drawing.Size(84, 22);
            this.lbOpeningStatus.Text = "opening table...";
            this.lbOpeningStatus.Visible = false;
            // 
            // bwFreeResource
            // 
            this.bwFreeResource.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwFreeResource_DoWork);
            this.bwFreeResource.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwFreeResource_RunWorkerCompleted);
            // 
            // tmFreeResource
            // 
            this.tmFreeResource.Interval = 60000;
            this.tmFreeResource.Tick += new System.EventHandler(this.tmFreeResource_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 595);
            this.Controls.Add(this.bnMain);
            this.Controls.Add(this.penalWindowTabs);
            this.Controls.Add(this.mainMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "FormMain";
            this.Text = "Trade";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.penalWindowTabs.ResumeLayout(false);
            this.cmTabChildFormControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bnMain)).EndInit();
            this.bnMain.ResumeLayout(false);
            this.bnMain.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem miTransMain;
        private System.Windows.Forms.ToolStripMenuItem miAccount;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem miTabSort;
        private System.Windows.Forms.ToolStripMenuItem miImport;
        private System.Windows.Forms.ToolStripMenuItem miImportProvider;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.BindingNavigator bnMain;
        public System.Windows.Forms.ToolStripButton btnSave;
        public System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripLabel lbOpeningStatus;
        private System.Windows.Forms.ToolStripMenuItem miUnprocessedFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem miArchivedFiles;
        private System.Windows.Forms.ToolStripMenuItem miTransaction;
        private System.Windows.Forms.ToolStripMenuItem miAnalyzeMain;
        private System.Windows.Forms.ToolStripMenuItem miExprorer;
        private System.Windows.Forms.ToolStripMenuItem miAnalyzeTest;
        private System.ComponentModel.BackgroundWorker bwFreeResource;
        private System.Windows.Forms.Timer tmFreeResource;
	}
}

