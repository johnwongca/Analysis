namespace Trade.Base
{
    partial class DbTree
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
            this.SuspendLayout();
            // 
            // DbTree
            // 
            this.LabelEdit = true;
            this.LineColor = System.Drawing.Color.Black;
            this.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DbTree_AfterSelect);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DbTree_KeyUp);
            this.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.DbTree_BeforeLabelEdit);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
