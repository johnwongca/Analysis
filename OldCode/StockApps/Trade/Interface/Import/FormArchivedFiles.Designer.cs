namespace Trade.Interface.Import
{
    partial class FormArchivedFiles
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new Trade.FixedDataGridView();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fileSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.archiveDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsSourceFileArchive = new Trade.Base.BindingSourceAdapter(this.components);
            this.dataSetProvider = new Trade.Interface.Import.DataSetProvider();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bsSourceFileArchiveBody = new Trade.Base.BindingSourceAdapter(this.components);
            this.daSourceFileArchive = new Trade.Interface.Import.DataSetProviderTableAdapters.daSourceFileArchive();
            this.daSourceFileArchiveBody = new Trade.Interface.Import.DataSetProviderTableAdapters.DaSourceFileArchiveBody();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSourceFileArchive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSourceFileArchiveBody)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Size = new System.Drawing.Size(539, 403);
            this.splitContainer1.SplitterDistance = 189;
            this.splitContainer1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.locationIDDataGridViewTextBoxColumn,
            this.fileNameDataGridViewTextBoxColumn,
            this.fileDateDataGridViewTextBoxColumn,
            this.fileSizeDataGridViewTextBoxColumn,
            this.importDateDataGridViewTextBoxColumn,
            this.archiveDateDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.bsSourceFileArchive;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(539, 189);
            this.dataGridView1.TabIndex = 0;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            // 
            // locationIDDataGridViewTextBoxColumn
            // 
            this.locationIDDataGridViewTextBoxColumn.DataPropertyName = "LocationID";
            this.locationIDDataGridViewTextBoxColumn.HeaderText = "LocationID";
            this.locationIDDataGridViewTextBoxColumn.Name = "locationIDDataGridViewTextBoxColumn";
            // 
            // fileNameDataGridViewTextBoxColumn
            // 
            this.fileNameDataGridViewTextBoxColumn.DataPropertyName = "FileName";
            this.fileNameDataGridViewTextBoxColumn.HeaderText = "FileName";
            this.fileNameDataGridViewTextBoxColumn.Name = "fileNameDataGridViewTextBoxColumn";
            // 
            // fileDateDataGridViewTextBoxColumn
            // 
            this.fileDateDataGridViewTextBoxColumn.DataPropertyName = "FileDate";
            this.fileDateDataGridViewTextBoxColumn.HeaderText = "FileDate";
            this.fileDateDataGridViewTextBoxColumn.Name = "fileDateDataGridViewTextBoxColumn";
            // 
            // fileSizeDataGridViewTextBoxColumn
            // 
            this.fileSizeDataGridViewTextBoxColumn.DataPropertyName = "FileSize";
            this.fileSizeDataGridViewTextBoxColumn.HeaderText = "FileSize";
            this.fileSizeDataGridViewTextBoxColumn.Name = "fileSizeDataGridViewTextBoxColumn";
            // 
            // importDateDataGridViewTextBoxColumn
            // 
            this.importDateDataGridViewTextBoxColumn.DataPropertyName = "ImportDate";
            this.importDateDataGridViewTextBoxColumn.HeaderText = "ImportDate";
            this.importDateDataGridViewTextBoxColumn.Name = "importDateDataGridViewTextBoxColumn";
            // 
            // archiveDateDataGridViewTextBoxColumn
            // 
            this.archiveDateDataGridViewTextBoxColumn.DataPropertyName = "ArchiveDate";
            this.archiveDateDataGridViewTextBoxColumn.HeaderText = "ArchiveDate";
            this.archiveDateDataGridViewTextBoxColumn.Name = "archiveDateDataGridViewTextBoxColumn";
            // 
            // bsSourceFileArchive
            // 
            this.bsSourceFileArchive.Adapter = null;
            this.bsSourceFileArchive.DataSource = this.dataSetProvider.SourceFileArchive;
            this.bsSourceFileArchive.DataTable = this.dataSetProvider.SourceFileArchive;
            this.bsSourceFileArchive.InternalPositionChangedEventAttached = false;
            this.bsSourceFileArchive.ReadOnly = true;
            // 
            // dataSetProvider
            // 
            this.dataSetProvider.DataSetName = "DataSetProvider";
            this.dataSetProvider.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsSourceFileArchiveBody, "Body", true));
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.MaxLength = 0;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(539, 210);
            this.textBox1.TabIndex = 1;
            this.textBox1.WordWrap = false;
            // 
            // bsSourceFileArchiveBody
            // 
            this.bsSourceFileArchiveBody.Adapter = null;
            this.bsSourceFileArchiveBody.DataSource = this.dataSetProvider.SourceFileArchiveBody;
            this.bsSourceFileArchiveBody.DataTable = this.dataSetProvider.SourceFileArchiveBody;
            this.bsSourceFileArchiveBody.InternalPositionChangedEventAttached = false;
            this.bsSourceFileArchiveBody.ReadOnly = true;
            // 
            // daSourceFileArchive
            // 
            this.daSourceFileArchive.ClearBeforeFill = true;
            // 
            // daSourceFileArchiveBody
            // 
            this.daSourceFileArchiveBody.ClearBeforeFill = true;
            // 
            // FormArchivedFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(539, 403);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormArchivedFiles";
            this.Text = "Archived Files";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSourceFileArchive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSourceFileArchiveBody)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DataSetProvider dataSetProvider;
        private Trade.Interface.Import.DataSetProviderTableAdapters.daSourceFileArchive daSourceFileArchive;
        private Trade.Interface.Import.DataSetProviderTableAdapters.DaSourceFileArchiveBody daSourceFileArchiveBody;
        private Trade.Base.BindingSourceAdapter bsSourceFileArchive;
        private Trade.Base.BindingSourceAdapter bsSourceFileArchiveBody;
        private Trade.FixedDataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn importDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn archiveDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox textBox1;
    }
}
