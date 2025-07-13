using System.Runtime.CompilerServices;
using System.Threading;

namespace Ulenv;

public readonly struct VersionToken
{
    static readonly VersionTokenSource @default = new();
    readonly VersionTokenSource source;
    readonly int version;
    readonly CancellationToken ct;
    public bool IsCancellationRequested
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => source.Outdated(version) || ct.IsCancellationRequested;
    }
    public override string ToString() => $"VersionToken {version} -> {source}";
    internal VersionToken(VersionTokenSource source, int version)
    {
        this.source = source;
        this.version = version;
        ct = default;
    }
    internal VersionToken(VersionTokenSource source, int version, CancellationToken ct)
    {
        this.source = source;
        this.version = version;
        this.ct = ct;
    }
    VersionToken(CancellationToken ct)
    {
        source = @default;
        version = default;
        this.ct = ct;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator VersionToken(CancellationToken x) => new(x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator CancellationToken(VersionToken x) => x.ct;
}
