using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ulenv;

public unsafe struct UlUnsafeCallbackArray : IDisposable
{
    UlUnsafeCallback* ptr;
    readonly int capacity;
    int length;
    public readonly int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => length;
    }
    public readonly bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ptr != null;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Invoke(object args)
    {
        for (int i = 0; i < length; i++)
            ptr[i].Invoke(args, i);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(UlUnsafeCallback value)
    {
        if (length >= capacity) return;
        ptr[length++] = value;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Fill()
    {
        for (int i = 0; i < capacity; i++)
            ptr[i] = default;
        length = capacity;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() => length = 0;
    public void Dispose()
    {
        if (ptr == null) return;
        for (int i = 0; i < capacity; i++)
            ptr[i].Dispose();
        Marshal.FreeHGlobal((IntPtr)ptr);
        ptr = null;
        length = 0;
    }
    public UlUnsafeCallbackArray(int capacity)
    {
        ptr = (UlUnsafeCallback*)Marshal.AllocHGlobal(UlUnsafeCallback.Size * capacity);
        this.capacity = capacity;
        length = 0;
    }
}
