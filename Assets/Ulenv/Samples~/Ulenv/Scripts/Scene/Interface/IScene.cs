using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Ulenv
{
    /// <summary>
    /// シーンのモデルです<br/>
    /// 主にPrefabを想定しています
    /// </summary>
    public interface IScene : IComponent
    {
        /// <summary>
        /// Addした場合にシーンが追加されていきます
        /// </summary>
        IList<IScene> Childs { get; }

        /// <summary>
        /// シーンを初期化します
        /// </summary>
        /// <param name="moduleMap">コンテナが持っているオブジェクトマップ</param>
        /// <param name="prev">1つ前のシーンのギフト</param>
        /// <returns>自分のギフト</returns>
        UniTask<ISceneGift> Initialize(IModuleMap moduleMap, ISceneGift prev);

        /// <summary>
        /// シーンを離れる際に通信する場合などで待機可能です<br/>
        /// ロジックエラー等でシーンを離れられない場合にtrueを返します
        /// </summary>
        /// <param name="next">次のシーンのアドレス</param>
        /// <returns>まだ滞在すべきかどうか</returns>
        UniTask<bool> ShouldStay(string next);

        /// <summary>
        /// シーンを破棄します
        /// </summary>
        /// <param name="next">次のシーンのアドレス</param>
        UniTask Destroy(string next);

        /// <summary>
        /// シーンをすぐに破棄します
        /// </summary>
        /// <param name="next">次のシーンのアドレス</param>
        void DestroyImmediate(string next);
    }
}
