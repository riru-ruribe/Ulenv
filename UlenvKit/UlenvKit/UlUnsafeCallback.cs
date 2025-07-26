using System;
using System.Runtime.InteropServices;

namespace Ulenv;

public unsafe readonly struct UlUnsafeCallback : IDisposable
{
    internal static readonly int Size = Marshal.SizeOf<UlUnsafeCallback>();
    readonly GCHandle handle;
    readonly IntPtr dlg;
    public void Invoke(object args, int i) => ((delegate*<object, object, int, void>)dlg)(handle.Target, args, i);
    public void Dispose()
    {
        if (handle.IsAllocated) handle.Free();
    }
    public UlUnsafeCallback(object methodContainer, delegate*<object, object, int, void> dlg)
    {
        handle = GCHandle.Alloc(methodContainer, GCHandleType.Pinned);
        this.dlg = new(dlg);
    }
}
