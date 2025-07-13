using System.Threading;

namespace Ulenv;

public sealed class CancelUnique : ICancelUnique
{
    static readonly VersionTokenSource source = new();
    readonly VersionToken token;
    public bool IsCancellationRequested => token.IsCancellationRequested;
    public VersionToken VersionToken => token;
    public Unique Unique { get; }
    public void Dispose() => source.Progress();
    public CancelUnique(Unique unique)
    {
        token = source.Token();
        Unique = unique;
    }
    public CancelUnique(Unique unique, CancellationToken ct)
    {
        token = source.Token(ct);
        Unique = unique;
    }
}
