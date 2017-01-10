using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Trade.Base
{
    /// <summary>
    /// Interaction logic for WPFCanvas.xaml
    /// </summary>
    public partial class WPFCanvas : UserControl
    {
        
        public WPFCanvas()
        {
            InitializeComponent();
        }
        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            rectangle1.Width = this.ActualWidth;
            rectangle1.Height = this.ActualHeight; 
        }
    }
}
