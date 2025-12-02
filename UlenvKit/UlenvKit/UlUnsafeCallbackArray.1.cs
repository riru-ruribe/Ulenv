using System;
using System.Runtime.InteropServices;

namespace Ulenv;

public unsafe struct UlUnsafeCallbackArray<T> : IDisposable where T : unmanaged
{
    UlUnsafeCallback<T>* ptr;
    readonly int capacity;
    int length;
    public readonly int Length => length;
    public readonly bool IsValid => ptr != null;
    public readonly void Invoke(object args)
    {
        for (int i = 0; i < length; i++)
            ptr[i].Invoke(args, i);
    }
    public void Add(UlUnsafeCallback<T> value)
    {
        if (length >= capacity) return;
        ptr[length++] = value;
    }
    public void Fill()
    {
        for (int i = 0; i < capacity; i++)
            ptr[i] = default;
    }
    public void Clear() => length = 0;
    public void Dispose()
    {
        if (ptr == null) return;
        Marshal.FreeHGlobal((IntPtr)ptr);
        ptr = null;
        length = 0;
    }
    public UlUnsafeCallbackArray(int capacity)
    {
        ptr = (UlUnsafeCallback<T>*)Marshal.AllocHGlobal(UlUnsafeCallback<T>.Size * capacity);
        this.capacity = capacity;
        length = 0;
    }
}
