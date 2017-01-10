namespace Trade.Interface.Transaction
{
    partial class FormBuySell
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
            this.button1 = new System.Windows.Forms.Button();
            this.mExchangeFee = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.mTradeFee = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mRateFrom = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.mQuantity = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mdate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lbSymbol = new System.Windows.Forms.Label();
            this.lbQty = new System.Windows.Forms.Label();
            this.mRateTo = new System.Windows.Forms.TextBox();
            this.mTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(203, 130);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 21);
            this.button1.TabIndex = 25;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // mExchangeFee
            // 
            this.mExchangeFee.Location = new System.Drawing.Point(97, 157);
            this.mExchangeFee.Name = "mExchangeFee";
            this.mExchangeFee.Size = new System.Drawing.Size(100, 20);
            this.mExchangeFee.TabIndex = 24;
            this.mExchangeFee.Text = "0";
            this.mExchangeFee.TextChanged += new System.EventHandler(this.mQuantity_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Exchange Fee";
            // 
            // mTradeFee
            // 
            this.mTradeFee.Location = new System.Drawing.Point(97, 130);
            this.mTradeFee.Name = "mTradeFee";
            this.mTradeFee.Size = new System.Drawing.Size(100, 20);
            this.mTradeFee.TabIndex = 22;
            this.mTradeFee.Text = "0";
            this.mTradeFee.TextChanged += new System.EventHandler(this.mQuantity_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Trade Fee";
            // 
            // mRateFrom
            // 
            this.mRateFrom.AcceptsTab = true;
            this.mRateFrom.Location = new System.Drawing.Point(97, 105);
            this.mRateFrom.Name = "mRateFrom";
            this.mRateFrom.Size = new System.Drawing.Size(100, 20);
            this.mRateFrom.TabIndex = 20;
            this.mRateFrom.Text = "0";
            this.mRateFrom.TextChanged += new System.EventHandler(this.mQuantity_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Rate";
            // 
            // mQuantity
            // 
            this.mQuantity.AcceptsReturn = true;
            this.mQuantity.Location = new System.Drawing.Point(97, 79);
            this.mQuantity.Name = "mQuantity";
            this.mQuantity.Size = new System.Drawing.Size(100, 20);
            this.mQuantity.TabIndex = 18;
            this.mQuantity.Text = "0";
            this.mQuantity.TextChanged += new System.EventHandler(this.mQuantity_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Quantity";
            // 
            // mdate
            // 
            this.mdate.Location = new System.Drawing.Point(97, 53);
            this.mdate.Name = "mdate";
            this.mdate.Size = new System.Drawing.Size(206, 20);
            this.mdate.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Date";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(228, 187);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 27;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(144, 187);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 26;
            this.button2.Text = "Ok";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbSymbol
            // 
            this.lbSymbol.AutoSize = true;
            this.lbSymbol.Location = new System.Drawing.Point(13, 13);
            this.lbSymbol.Name = "lbSymbol";
            this.lbSymbol.Size = new System.Drawing.Size(35, 13);
            this.lbSymbol.TabIndex = 28;
            this.lbSymbol.Text = "label1";
            // 
            // lbQty
            // 
            this.lbQty.AutoSize = true;
            this.lbQty.Location = new System.Drawing.Point(13, 35);
            this.lbQty.Name = "lbQty";
            this.lbQty.Size = new System.Drawing.Size(35, 13);
            this.lbQty.TabIndex = 29;
            this.lbQty.Text = "label1";
            // 
            // mRateTo
            // 
            this.mRateTo.Location = new System.Drawing.Point(203, 105);
            this.mRateTo.Name = "mRateTo";
            this.mRateTo.Size = new System.Drawing.Size(100, 20);
            this.mRateTo.TabIndex = 30;
            this.mRateTo.Text = "0";
            this.mRateTo.TextChanged += new System.EventHandler(this.mQuantity_TextChanged);
            // 
            // mTotal
            // 
            this.mTotal.AutoSize = true;
            this.mTotal.Location = new System.Drawing.Point(200, 160);
            this.mTotal.Name = "mTotal";
            this.mTotal.Size = new System.Drawing.Size(0, 13);
            this.mTotal.TabIndex = 31;
            // 
            // FormBuySell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 226);
            this.Controls.Add(this.mTotal);
            this.Controls.Add(this.mRateTo);
            this.Controls.Add(this.lbQty);
            this.Controls.Add(this.lbSymbol);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mExchangeFee);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.mTradeFee);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.mRateFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.mQuantity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mdate);
            this.Controls.Add(this.label2);
            this.Name = "FormBuySell";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox mExchangeFee;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox mTradeFee;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox mRateFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox mQuantity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker mdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lbSymbol;
        private System.Windows.Forms.Label lbQty;
        private System.Windows.Forms.TextBox mRateTo;
        private System.Windows.Forms.Label mTotal;
    }
}