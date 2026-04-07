using System.Collections.Generic;

namespace Ulenv;

public sealed class ModuleMap : Dictionary<Unique, object>, IModuleMap
{
    readonly List<IAwakable> awakables;
    readonly List<IReawakable> reawakables;
    object IModuleMap.this[Unique key]
    {
        get => this[key];
        set
        {
            this[key] = value;
            if (value is IAwakable a) awakables.Add(a);
            if (value is IReawakable r) reawakables.Add(r);
        }
    }
    IEnumerable<Unique> IModuleMap.Keys => Keys;
    IEnumerable<object> IModuleMap.Values => Values;
    T IModuleMap.Get<T>(Unique key) => (T)this[key];
    bool IModuleMap.TryGetValue<T>(Unique key, out T value)
    {
        if (TryGetValue(key, out var obj))
        {
            value = (T)obj;
            return true;
        }
        value = default;
        return false;
    }
    void IModuleMap.Awaken()
    {
        var i = 0;
        while (i < awakables.Count)
        {
            awakables[i].Awaken(this);
            if (awakables[i].Once)
            {
                awakables.RemoveAt(i);
                continue;
            }
            i++;
        }
    }
    void IModuleMap.Reawaken()
    {
        var i = 0;
        while (i < reawakables.Count)
        {
            reawakables[i].Reawaken();
            if (reawakables[i].Once)
            {
                reawakables.RemoveAt(i);
                continue;
            }
            i++;
        }
    }
    public ModuleMap() : base(new UniqueComparer())
    {
        awakables = new();
        reawakables = new();
    }
    public ModuleMap(int capacity) : base(new UniqueComparer())
    {
        awakables = new(capacity);
        reawakables = new(capacity);
    }
}
