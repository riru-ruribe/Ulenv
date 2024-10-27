using System;

namespace Ulenv
{
    using Count = System.UInt64;
    using Loop = System.UInt64;

    public readonly struct Unique : IEquatable<Unique>, IComparable<Unique>
    {
        static Count sCount;
        static Loop sLoop;
        readonly Count count;
        readonly Loop loop;
        public bool Equals(Unique other) => count == other.count && loop == other.loop;
        public int CompareTo(Unique other) => this > other ? 1 : (this < other ? -1 : 0);
        public override bool Equals(object o) => throw new NotImplementedException();
        public override int GetHashCode() => 0;
        public override string ToString() => $"{loop},{count}";
        Unique(Count count, Loop loop)
        {
            this.count = count;
            this.loop = loop;
        }
        public static Unique Get()
        {
            if (++sCount == 0) sLoop++;
            return new(sCount, sLoop);
        }
        public static bool operator ==(Unique a, Unique b) => a.loop == b.loop && a.count == b.count;
        public static bool operator !=(Unique a, Unique b) => a.loop != b.loop || a.count != b.count;
        public static bool operator >(Unique a, Unique b) => a.loop != b.loop ? a.loop > b.loop : a.count > b.count;
        public static bool operator <(Unique a, Unique b) => a.loop != b.loop ? a.loop < b.loop : a.count < b.count;
    }
}
