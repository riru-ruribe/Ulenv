using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ulenv
{
    /// <summary>
    /// ダイアログの実体です
    /// </summary>
    public sealed class Dialog : MonoBehaviour, IScene, IEquatable<Dialog>, IComparable<Dialog>
    {
        // [SerializeReference] IDialogContent content = default;
        // [SerializeReference] IDialogTransfer transfer = default;
        [SerializeField] DialogContent content = default;
        [SerializeField] DialogTransfer transfer = default;

        DialogGroup dialogGroup;
        Unique unique;

        IList<IScene> IScene.Childs => throw new NotImplementedException();
        public bool Touchable { set => transfer.Touchable = value; }

        async UniTask<ISceneGift> IScene.Initialize(IModuleMap moduleMap, ISceneGift prev)
        {
            unique = Unique.Get();
            dialogGroup = moduleMap.Get<DialogGroup>(SceneModuleUniques.Dialog);
            dialogGroup.Active(this);
            await transfer.Initialize(moduleMap, prev);
            var gift = await content.Initialize(moduleMap, prev, unique);
            UnityEngine.Assertions.Assert.IsNotNull(gift);
            await transfer.Open();
            content.OnOpen();
            return gift;
        }

        async UniTask<bool> IScene.ShouldStay(string next)
        {
            return await content.GetCode() > 0;
        }

        async UniTask IScene.Destroy(string next)
        {
            await transfer.Close();
            transfer.Dispose();
            content.Dispose();
            GameObject.Destroy(gameObject);
            dialogGroup.Deactive(this);
        }

        void IScene.DestroyImmediate(string next)
        {
            transfer.Dispose();
            content.Dispose();
            GameObject.Destroy(gameObject);
            dialogGroup.Deactive(this);
        }

        bool IEquatable<Dialog>.Equals(Dialog other) => unique == other.unique;
        public int CompareTo(Dialog other) => unique.CompareTo(other.unique);
        public static bool operator >(Dialog a, Dialog b) => a.unique > b.unique;
        public static bool operator <(Dialog a, Dialog b) => a.unique < b.unique;
    }
}
