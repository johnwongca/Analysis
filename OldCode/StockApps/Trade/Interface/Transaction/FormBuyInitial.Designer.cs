namespace Trade.Interface.Transaction
{
    partial class FormBuyInitial
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
            this.lbSymbol = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mAccount = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mdate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.mQuantity = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.mRateFrom = new System.Windows.Forms.TextBox();
            this.mRateTo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mTradeFee = new System.Windows.Forms.TextBox();
            this.mExchangeFee = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.mTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbSymbol
            // 
            this.lbSymbol.AutoSize = true;
            this.lbSymbol.Location = new System.Drawing.Point(13, 13);
            this.lbSymbol.Name = "lbSymbol";
            this.lbSymbol.Size = new System.Drawing.Size(44, 13);
            this.lbSymbol.TabIndex = 0;
            this.lbSymbol.Text = "Symbol:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Account";
            // 
            // mAccount
            // 
            this.mAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mAccount.FormattingEnabled = true;
            this.mAccount.Location = new System.Drawing.Point(97, 32);
            this.mAccount.Name = "mAccount";
            this.mAccount.Size = new System.Drawing.Size(206, 21);
            this.mAccount.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Date";
            // 
            // mdate
            // 
            this.mdate.Location = new System.Drawing.Point(97, 59);
            this.mdate.Name = "mdate";
            this.mdate.Size = new System.Drawing.Size(206, 20);
            this.mdate.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Quantity";
            // 
            // mQuantity
            // 
            this.mQuantity.AcceptsReturn = true;
            this.mQuantity.Location = new System.Drawing.Point(97, 85);
            this.mQuantity.Name = "mQuantity";
            this.mQuantity.Size = new System.Drawing.Size(100, 20);
            this.mQuantity.TabIndex = 6;
            this.mQuantity.Text = "0";
            this.mQuantity.TextChanged += new System.EventHandler(this.mQuantity_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Rate";
            // 
            // mRateFrom
            // 
            this.mRateFrom.Location = new System.Drawing.Point(97, 111);
            this.mRateFrom.Name = "mRateFrom";
            this.mRateFrom.Size = new System.Drawing.Size(100, 20);
            this.mRateFrom.TabIndex = 8;
            this.mRateFrom.Text = "0";
            this.mRateFrom.TextChanged += new System.EventHandler(this.mQuantity_TextChanged);
            // 
            // mRateTo
            // 
            this.mRateTo.Location = new System.Drawing.Point(203, 111);
            this.mRateTo.Name = "mRateTo";
            this.mRateTo.Size = new System.Drawing.Size(100, 20);
            this.mRateTo.TabIndex = 9;
            this.mRateTo.Text = "0";
            this.mRateTo.TextChanged += new System.EventHandler(this.mQuantity_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Trade Fee";
            // 
            // mTradeFee
            // 
            this.mTradeFee.Location = new System.Drawing.Point(97, 137);
            this.mTradeFee.Name = "mTradeFee";
            this.mTradeFee.Size = new System.Drawing.Size(100, 20);
            this.mTradeFee.TabIndex = 11;
            this.mTradeFee.Text = "0";
            this.mTradeFee.TextChanged += new System.EventHandler(this.mQuantity_TextChanged);
            // 
            // mExchangeFee
            // 
            this.mExchangeFee.Location = new System.Drawing.Point(97, 163);
            this.mExchangeFee.Name = "mExchangeFee";
            this.mExchangeFee.Size = new System.Drawing.Size(100, 20);
            this.mExchangeFee.TabIndex = 13;
            this.mExchangeFee.Text = "0";
            this.mExchangeFee.TextChanged += new System.EventHandler(this.mQuantity_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Exchange Fee";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(203, 136);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 21);
            this.button1.TabIndex = 14;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(147, 198);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Ok";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(228, 198);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 16;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // mTotal
            // 
            this.mTotal.AutoSize = true;
            this.mTotal.Location = new System.Drawing.Point(203, 169);
            this.mTotal.Name = "mTotal";
            this.mTotal.Size = new System.Drawing.Size(0, 13);
            this.mTotal.TabIndex = 17;
            // 
            // FormBuyInitial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 233);
            this.Controls.Add(this.mTotal);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mExchangeFee);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.mTradeFee);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.mRateTo);
            this.Controls.Add(this.mRateFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.mQuantity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mdate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mAccount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbSymbol);
            this.Name = "FormBuyInitial";
            this.Text = "Buy Initialize";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbSymbol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox mAccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker mdate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox mQuantity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox mRateFrom;
        private System.Windows.Forms.TextBox mRateTo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox mTradeFee;
        private System.Windows.Forms.TextBox mExchangeFee;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label mTotal;
    }
}