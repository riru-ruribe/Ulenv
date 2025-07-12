using System;

namespace Ulenv
{
    /// <summary>
    /// 破棄可能ユニークモデルです
    /// </summary>
    public interface ICancelUnique : IDisposable
    {
        bool IsCancellationRequested { get; }
        VersionToken VersionToken { get; }
        Unique Unique { get; }
    }
}
