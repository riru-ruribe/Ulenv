using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Ulenv
{
    public class Result<T> : IResult<T>
    {
        readonly IEqualityComparer<T> comparer;
        T value;
        bool fake;

        public T Value => value;
        public bool Yet
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => comparer.Equals(value, default);
        }

        public async UniTask<T> Wait()
        {
            while (Yet) await UniTask.Yield();
            while (fake) await UniTask.Yield();
            return value;
        }

        public async UniTask<T> Wait(CancellationToken cancellationToken)
        {
            while (Yet)
            {
                if (cancellationToken.IsCancellationRequested) return default;
                await UniTask.Yield();
            }
            while (fake)
            {
                if (cancellationToken.IsCancellationRequested) return default;
                await UniTask.Yield();
            }
            return value;
        }

        public void Real(T value) => this.value = value;

        public void Fake(T value)
        {
            this.value = value;
            fake = true;
        }

        public void Approve() => fake = false;

        public Result()
        {
            comparer = EqualityComparer<T>.Default;
            value = default;
        }
        public Result(IEqualityComparer<T> comparer)
        {
            this.comparer = comparer;
            value = default;
        }
        public Result(T value)
        {
            comparer = EqualityComparer<T>.Default;
            this.value = value;
        }
        public Result(T value, IEqualityComparer<T> comparer)
        {
            this.comparer = comparer;
            this.value = value;
        }
    }
}
