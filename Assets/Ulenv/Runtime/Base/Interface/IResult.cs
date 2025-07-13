using Cysharp.Threading.Tasks;
using System.Threading;

namespace Ulenv
{
    /// <summary>
    /// 結果待機可能モデルです
    /// </summary>
    public interface IResult<T>
    {
        T Value { get; }
        bool Yet { get; }
        UniTask<T> Wait();
        UniTask<T> Wait(CancellationToken cancellationToken);
        void Real(T value);
        void Fake(T value);
        void Approve();
    }
}
