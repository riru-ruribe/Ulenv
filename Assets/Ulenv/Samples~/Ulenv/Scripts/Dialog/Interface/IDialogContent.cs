using Cysharp.Threading.Tasks;
using System;

namespace Ulenv
{
    /// <summary>
    /// ダイアログの表示を抽象化します
    /// </summary>
    public interface IDialogContent : IDisposable
    {
        UniTask<ISceneGift> Initialize(IModuleMap moduleMap, ISceneGift prev, Unique unique);
        void OnOpen();
        UniTask<byte> GetCode();
    }
}
