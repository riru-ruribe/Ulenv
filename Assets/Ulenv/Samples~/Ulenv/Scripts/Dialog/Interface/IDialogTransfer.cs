using Cysharp.Threading.Tasks;
using System;

namespace Ulenv
{
    /// <summary>
    /// ダイアログの開閉動作を抽象化します
    /// </summary>
    public interface IDialogTransfer : IDisposable
    {
        bool Touchable { set; }
        UniTask Initialize(IModuleMap moduleMap, ISceneGift prev);
        UniTask Open();
        UniTask Close();
    }
}
