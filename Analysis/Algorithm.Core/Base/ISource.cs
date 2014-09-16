using System;
namespace Algorithm.Core
{
    public interface ISource : IDisposable
    {
        Window<DateTime> DateFrom { get; }
        Window<DateTime> DateTo { get; }
        Window<double> Open { get; }
        Window<double> High { get; }
        Window<double> Low { get; }
        Window<double> Close { get; }
        Window<double> Volume { get; }
        Window<int> ItemCount { get; }
        Window<double> TypicalPrice { get; }
        int SymbolID { get; set; }
        IntervalType IntervalType { get; set; }
        int Interval { get; set; }
        int SourceWindowSize { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        bool IsClosed { get; }



        void OpenData();
        void CloseData();
        bool Read(bool pushAfterRead = true);

    }
}
