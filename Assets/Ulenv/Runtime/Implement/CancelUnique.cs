using System.Threading;

namespace Ulenv
{
    public sealed class CancelUnique : ICancelUnique
    {
        static readonly VersionTokenSource source = new();
        readonly CancellationToken ct;
        public VersionToken VersionToken => source.Token(ct);
        public Unique Unique { get; }
        public void Dispose() => source.Progress();
        public CancelUnique(Unique unique) => Unique = unique;
        public CancelUnique(Unique unique, CancellationToken ct) : this(unique) => this.ct = ct;
    }
}
