using System;
using System.Runtime.CompilerServices;

namespace Ulenv;

using Count = UInt64;
using Loop = UInt64;

public readonly struct Unique : IEquatable<Unique>, IComparable<Unique>
{
    static Count sCount;
    static Loop sLoop;
    internal readonly Count count;
    internal readonly Loop loop;
    public bool Equals(Unique other) => count == other.count && loop == other.loop;
    public int CompareTo(Unique other) => this > other ? 1 : (this < other ? -1 : 0);
    public override bool Equals(object o) => throw new NotImplementedException();
    public override int GetHashCode() => HashCode.Combine(count, loop);
    public override string ToString() => $"{loop},{count}";
    public Unique(Count count, Loop loop)
    {
        this.count = count;
        this.loop = loop;
    }
    public static Unique Get()
    {
        if (++sCount == 0) sLoop++;
        return new(sCount, sLoop);
    }
    public static readonly Unique Zero = new();
    public static bool operator ==(Unique a, Unique b) => a.loop == b.loop && a.count == b.count;
    public static bool operator !=(Unique a, Unique b) => a.loop != b.loop || a.count != b.count;
    public static bool operator >(Unique a, Unique b) => a.loop != b.loop ? a.loop > b.loop : a.count > b.count;
    public static bool operator <(Unique a, Unique b) => a.loop != b.loop ? a.loop < b.loop : a.count < b.count;
}

[Serializable]
public struct SerialUnique : IEquatable<SerialUnique>
{
    public Count count;
    public Loop loop;
    public readonly bool Equals(SerialUnique other) => count == other.count && loop == other.loop;
    public readonly override bool Equals(object o) => throw new NotImplementedException();
    public readonly override int GetHashCode() => HashCode.Combine(count, loop);
    public readonly override string ToString() => $"{loop},{count}";
    public SerialUnique(Count count, Loop loop)
    {
        this.count = count;
        this.loop = loop;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Unique(SerialUnique x) => new(x.count, x.loop);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator SerialUnique(Unique x) => new(x.count, x.loop);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(SerialUnique a, Unique b) => (Unique)a == b;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(SerialUnique a, Unique b) => (Unique)a != b;
}
