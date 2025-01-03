using System;
using System.Threading;

namespace Ulenv
{
    /// <summary>
    /// 破棄可能ユニークモデルです
    /// </summary>
    public interface ICancelUnique : IDisposable
    {
        CancellationToken CancellationToken { get; }
        Unique Unique { get; }
    }
}
