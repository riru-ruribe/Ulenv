using System;
using System.Runtime.CompilerServices;

namespace Ulenv;

public readonly struct ModuleScope : IDisposable
{
    readonly IModuleMap moduleMap;
    readonly Unique unique;
    public bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => moduleMap != null;
    }
    public void Dispose()
    {
        if (moduleMap.Remove(unique, out var obj) &&
            obj is IDisposable d)
            d.Dispose();
    }
    public ModuleScope(IModuleMap moduleMap, Unique unique)
    {
        this.moduleMap = moduleMap;
        this.unique = unique;
    }
}
