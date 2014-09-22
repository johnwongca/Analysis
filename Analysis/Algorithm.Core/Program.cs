using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Algorithm.Core.Forms;
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
            Application.Run(new MainForm());
        }
        static void Main(string[] args)
        {
            //StartWindows();
            Test.Test.Test_Indicator_VolumeWeightedMovingAverage();
            

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
