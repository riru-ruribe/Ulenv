using System;

namespace Ulenv;

[Serializable]
public struct UniqueKeyResult
{
    public SerialUnique unique;
    public int index;
    public UniqueKeyResult(SerialUnique unique, int index)
    {
        this.unique = unique;
        this.index = index;
    }
}
