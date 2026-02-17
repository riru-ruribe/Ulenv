using System;
using System.Runtime.CompilerServices;

namespace Ulenv;

public readonly struct ModuleScope : IDisposable
{
    readonly IModuleMap moduleMap;
    readonly Unique unique;
    readonly NullableReference prev;
    public bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => moduleMap != null;
    }
    public void Dispose()
    {
        if (moduleMap.Remove(unique, out var obj))
        {
            if (obj is IDisposable d)
                d.Dispose();
            if (prev.Is)
                moduleMap[unique] = prev.Target;
        }
    }
    public ModuleScope(IModuleMap moduleMap, Unique unique)
    {
        this.moduleMap = moduleMap;
        this.unique = unique;
        prev = default;
    }
    public ModuleScope(IModuleMap moduleMap, Unique unique, NullableReference prev)
    {
        this.moduleMap = moduleMap;
        this.unique = unique;
        this.prev = prev;
    }
}
