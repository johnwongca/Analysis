﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trade.Interface.Analyze
{
    public partial class FormChartDays : Form
    {
        public FormChartDays()
        {
            InitializeComponent();
        }
        public int Days
        {
            get { return (int)numericUpDown1.Value; }
        }
        public event EventHandler ValueChanged;
        private void FormChartDays_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }
    }
}
