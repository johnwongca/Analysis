using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algorithm.Core.Forms
{
    public partial class FormBase : Form
    {
        FormSymbol _Symbol;
        public FormBase()
        {
            InitializeComponent();
        }

        private void FormBase_Load(object sender, EventArgs e)
        {
            _Symbol = MainForm.SymbolForm;
            if (this != _Symbol)
                MainForm.Children.Add(this);
        }

        private void FormBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this != MainForm.SymbolForm)
                MainForm.Children.Remove(this);
        }
    }
}
