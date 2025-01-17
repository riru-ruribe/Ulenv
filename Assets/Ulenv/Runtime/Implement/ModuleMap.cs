using System.Collections.Generic;

namespace Ulenv
{
    public sealed class ModuleMap : Dictionary<Unique, object>, IModuleMap
    {
        IEnumerable<Unique> IModuleMap.Keys => Keys;
        IEnumerable<object> IModuleMap.Values => Values;
        public T Get<T>(Unique key) => (T)this[key];
        public bool Remove<T>(Unique key, out T value)
        {
            if (Remove(key, out var obj))
            {
                value = (T)obj;
                return true;
            }
            value = default;
            return false;
        }
        public bool TryGetValue<T>(Unique key, out T value)
        {
            if (TryGetValue(key, out var obj))
            {
                value = (T)obj;
                return true;
            }
            value = default;
            return false;
        }
        public ModuleMap() : base(new UniqueComparer()) { }
    }
}
