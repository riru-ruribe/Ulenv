using System;
using System.Runtime.CompilerServices;

namespace Ulenv;

public unsafe readonly struct UlUnsafeCallback<T> where T : unmanaged
{
    internal static readonly int Size = Unsafe.SizeOf<UlUnsafeCallback<T>>();
    readonly T identifier;
    readonly IntPtr dlg;
    public void Invoke(object args, int i) => ((delegate*<T, object, int, void>)dlg)(identifier, args, i);
    public UlUnsafeCallback(T identifier, delegate*<T, object, int, void> dlg)
    {
        this.identifier = identifier;
        this.dlg = new(dlg);
    }
}
