using System.Runtime.CompilerServices;

namespace Ulenv;

public static class ModuleScopeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ModuleScope Resolve(this IModuleMap self, Unique unique, object value)
    {
        self[unique] = value;
        return new(self, unique);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ModuleScope Resolve<T>(this IModuleMap self, T value, UniqueHelper<T> helper)
    {
        self[helper] = value;
        return new(self, helper);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ModuleScope Resolve<T>(this IModuleMap self, T value) where T : IResolvable
    {
        self[value.Unique] = value;
        return new(self, value.Unique);
    }
}
