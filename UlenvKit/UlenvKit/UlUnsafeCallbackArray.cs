using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ulenv;

public unsafe struct UlUnsafeCallbackArray : IDisposable
{
    IntPtr iptr;
    readonly int capacity;
    int length;
    public static UlUnsafeCallbackArray Dummy = new();
    public readonly int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => length;
    }
    public readonly bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => iptr != IntPtr.Zero;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Invoke(object args)
    {
        var ptr = (UlUnsafeCallback*)iptr;
        for (int i = 0; i < length; i++)
            ptr[i].Invoke(args, i);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(UlUnsafeCallback value)
    {
        if (length >= capacity) return;
        var ptr = (UlUnsafeCallback*)iptr;
        ptr[length++] = value;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Fill()
    {
        var ptr = (UlUnsafeCallback*)iptr;
        for (int i = 0; i < capacity; i++)
            ptr[i] = default;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        var ptr = (UlUnsafeCallback*)iptr;
        for (int i = 0; i < length; i++)
            ptr[i].Dispose();
        length = 0;
    }
    public void Dispose()
    {
        if (iptr == IntPtr.Zero) return;
        var ptr = (UlUnsafeCallback*)iptr;
        for (int i = 0; i < length; i++)
            ptr[i].Dispose();
        Marshal.FreeHGlobal(iptr);
        iptr = IntPtr.Zero;
        length = 0;
    }
    public UlUnsafeCallbackArray(int capacity)
    {
        iptr = Marshal.AllocHGlobal(UlUnsafeCallback.Size * capacity);
        this.capacity = capacity;
        length = 0;
    }
}
