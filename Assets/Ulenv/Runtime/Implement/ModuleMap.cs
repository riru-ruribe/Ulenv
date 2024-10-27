using System.Collections.Generic;

namespace Ulenv
{
    public sealed class ModuleMap : Dictionary<Unique, object>, IModuleMap
    {
        IEnumerable<Unique> IModuleMap.Keys => Keys;
        IEnumerable<object> IModuleMap.Values => Values;
        public T Get<T>(Unique key) => (T)this[key];
        public ModuleMap() : base(new UniqueComparer()) { }
    }
}
