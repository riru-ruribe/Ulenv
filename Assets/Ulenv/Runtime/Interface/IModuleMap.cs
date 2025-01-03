﻿using System.Collections.Generic;

namespace Ulenv
{
    /// <summary>
    /// 依存解決モデルです<br/>
    /// IReadOnlyDictionaryで持ち回すのが面倒なので抽象化します
    /// </summary>
    public interface IModuleMap
    {
        object this[Unique key] { get; }
        IEnumerable<Unique> Keys { get; }
        IEnumerable<object> Values { get; }
        T Get<T>(Unique key);
        bool ContainsKey(Unique key);
        bool TryGetValue<T>(Unique key, out T value);
    }
}
