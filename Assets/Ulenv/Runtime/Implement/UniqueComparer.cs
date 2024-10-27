using System.Collections.Generic;

namespace Ulenv
{
    public sealed class UniqueComparer : IEqualityComparer<Unique>
    {
        bool IEqualityComparer<Unique>.Equals(Unique x, Unique y) => x == y;
        int IEqualityComparer<Unique>.GetHashCode(Unique obj) => 0;
    }
}
