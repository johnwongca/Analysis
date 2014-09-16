using System;
namespace Algorithm.Core
{
    public interface IWindowBase
    {
        object Value { get; set; }
    }
    public interface IWindow<T> : IComparableEnumerable<T> where T : IComparable<T>, IEquatable<T>, IConvertible
    {
        T[] Buffer { get; }
        long CurrentLocation { get; }
        T First { get; }
        bool HasValue(int index);
        Window<T> Push(params Window<T>[] values);
        T[] RawBuffer { get; }
        Window<T> Set(int index, T value);
        Window<T> Set(T value);
        int Size { get; set; }
        T this[int index] { get; }
        T Value { get; set; }
        int ValuesInWindow { get; }
    }
}
