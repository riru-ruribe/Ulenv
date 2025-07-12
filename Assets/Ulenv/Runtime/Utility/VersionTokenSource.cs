using System.Runtime.CompilerServices;
using System.Threading;

namespace Ulenv
{
    public sealed class VersionTokenSource
    {
        int version;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VersionToken Token() => new(this, version);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VersionToken Token(CancellationToken ct) => new(this, version, ct);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Outdated(int v) => version != v;
        public void Progress() => Interlocked.Increment(ref version);
        public override string ToString() => version.ToString();
    }
}
