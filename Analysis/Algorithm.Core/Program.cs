using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Algorithm.Core.Forms;
using Algorithm.Core;
using System.Reflection;
using Fasterflect;
namespace Algorithm.Core
{
    
    public partial class Program
    {
        static bool IsWindowsStarted = false;
        
        static void StartWindows()
        {
            IsWindowsStarted = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm.Main = new MainForm();
            Application.Run(MainForm.Main);
        }
        [STAThread]
        static void Main(string[] args)
        {
            IndicatorClass.LoadIndicatorClasses();
            ChartList.LoadCharts();

            StartWindows();

            //Test.Test.Test_Indicator_Test1Indicator();

            #region Paulse and quit

            if (!IsWindowsStarted)
            {
                Console.WriteLine("Done");
                Console.ReadKey();
            }
            #endregion
        }

        
    }

   
}
