using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EODDataService
{
    class Program
    {
        static void Main(string[] args)
        {
            G.SetDatabaseConnection(args);
            G.RemoveSessionStatus();
            ThreadPool.SetMinThreads(50, 50);
            DownloadTask t;
            while(true)
            {
                t = new DownloadTask();
                while (t.Status == DownloadTaskStatus.Pending)
                    Thread.Sleep(1);
                if ((DateTime.Now - G.LastTaskExecutionTime).TotalMinutes >= G.MaxIdleTimeInMinute)
                    break;
            }
            DownloadTask.WaitAll();
        }
    }
}
