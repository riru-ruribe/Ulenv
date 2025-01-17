using System;
using System.Runtime.CompilerServices;

namespace Ulenv
{
    public readonly struct ModuleScope : IDisposable
    {
        readonly IModuleMap moduleMap;
        readonly Unique unique;

        public void Dispose()
        {
            if (moduleMap.Remove(unique, out IDisposable d))
                d.Dispose();
        }

        public ModuleScope(IModuleMap moduleMap, Unique unique)
        {
            this.moduleMap = moduleMap;
            this.unique = unique;
        }
    }

    public static class ModuleScopeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ModuleScope Resolve(this IModuleMap self, Unique unique, object value)
        {
            self[unique] = value;
            return new(self, unique);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ModuleScope Resolve<T>(this IModuleMap self, T value)
            where T : IResolvable
        {
            self[value.Unique] = value;
            return new(self, value.Unique);
        }
    }
}
