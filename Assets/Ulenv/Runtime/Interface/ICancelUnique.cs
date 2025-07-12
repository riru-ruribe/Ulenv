using System;

namespace Ulenv
{
    /// <summary>
    /// 破棄可能ユニークモデルです
    /// </summary>
    public interface ICancelUnique : IDisposable
    {
        VersionToken VersionToken { get; }
        Unique Unique { get; }
    }
}
