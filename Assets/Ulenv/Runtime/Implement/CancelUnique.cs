using System.Threading;

namespace Ulenv
{
    public sealed class CancelUnique : ICancelUnique
    {
        readonly CancellationTokenSource cancellationTokenSource = new();
        public CancellationToken CancellationToken => cancellationTokenSource.Token;
        public Unique Unique { get; }
        public void Dispose() => cancellationTokenSource.Cancel();
        public CancelUnique(Unique unique) => Unique = unique;
    }
}
