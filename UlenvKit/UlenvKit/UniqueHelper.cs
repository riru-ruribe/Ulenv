using System.Runtime.CompilerServices;

namespace Ulenv;

public readonly struct UniqueHelper<T>
{
    static readonly ulong count = typeof(T).ToSerialHash();
    readonly ulong loop;
    public readonly Unique Unique => new(count, loop);
    public UniqueHelper(ulong loop) => this.loop = loop;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Unique(UniqueHelper<T> x) => new(count, x.loop);
}
