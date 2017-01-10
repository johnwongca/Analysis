namespace Trade.Base
{
    partial class Canvas
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
            this.m_Host = new System.Windows.Forms.Integration.ElementHost();
            this.wpfCanvas = new Trade.Base.WPFCanvas();
            this.mouseCapture = new Trade.Base.ScreenMousePosition();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // m_Host
            // 
            this.m_Host.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Host.Location = new System.Drawing.Point(0, 0);
            this.m_Host.Name = "m_Host";
            this.m_Host.Size = new System.Drawing.Size(200, 47);
            this.m_Host.TabIndex = 0;
            this.m_Host.Child = this.wpfCanvas;
            // 
            // mouseCapture
            // 
            this.mouseCapture.Enabled = false;
            this.mouseCapture.RefreshInterval = 100;
            this.mouseCapture.PositionChanged += new Trade.Base.ScreenMousePositionMouseChangeEventHandler(this.mouseCapture_PositionChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_Host);
            this.MinimumSize = new System.Drawing.Size(200, 20);
            this.Name = "Canvas";
            this.Size = new System.Drawing.Size(200, 47);
            this.Resize += new System.EventHandler(this.Canvas_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost m_Host;
        private WPFCanvas wpfCanvas;
        private Trade.Base.ScreenMousePosition mouseCapture;
        private System.Windows.Forms.Timer timer1;
    }
}
