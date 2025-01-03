using System;
using System.Runtime.CompilerServices;

namespace Ulenv
{
    public readonly struct ModuleAddScope : IDisposable
    {
        readonly ModuleMap moduleMap;
        readonly Unique unique;

        public void Dispose() => moduleMap.Remove(unique);

        public ModuleAddScope(ModuleMap moduleMap, Unique unique)
        {
            this.moduleMap = moduleMap;
            this.unique = unique;
        }
    }

    public static class ModuleMapExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ModuleAddScope AddScoped(this ModuleMap self, Unique unique, object value)
        {
            self[unique] = value;
            return new(self, unique);
        }
    }
}
