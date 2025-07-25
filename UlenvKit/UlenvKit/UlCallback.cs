using System.Runtime.CompilerServices;
using System.Threading;

namespace Ulenv;

public unsafe readonly struct UlCallback
{
    readonly object methodContainer;
    readonly delegate*<object, object, int, void> dlg;
    readonly CancellationToken ct;
    public bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => methodContainer != null;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Invoke(object args, int code)
    {
        if (ct.IsCancellationRequested || methodContainer == null) return;
        dlg(methodContainer, args, code);
    }
    public UlCallback(object methodContainer, delegate*<object, object, int, void> dlg, CancellationToken ct)
    {
        this.methodContainer = methodContainer;
        this.dlg = dlg;
        this.ct = ct;
    }
}
