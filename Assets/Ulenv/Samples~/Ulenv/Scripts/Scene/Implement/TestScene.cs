using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Ulenv
{
    public sealed class TestScene : MonoBehaviour, IScene
    {
        public const string Name = nameof(TestScene);

        sealed class Gift : ISceneGift
        {
            readonly List<ISceneGift> childs = new();
            public string Address => Name;
            public string GroupName => SceneGroup.Name;
            public IList<ISceneGift> Childs => childs;
        }

        IUlenv env;
        SceneGroup sceneGroup;
        Gift gift;
        readonly List<IScene> childs = new();

        IList<IScene> IScene.Childs => childs;

        async UniTask<ISceneGift> IScene.Initialize(IModuleMap moduleMap, ISceneGift prev)
        {
            env = moduleMap.Get<IUlenv>(SceneModuleUniques.Env);
            sceneGroup = env.GetGroup<SceneGroup>();
            await UniTask.Yield();
            gift = new();
            return gift;
        }

        UniTask<bool> IScene.ShouldStay(string next)
        {
            if (string.IsNullOrEmpty(next))
            {
                Debug.Log("アプリを終了しますか?");
                return UniTask.FromResult(true);
            }
            return UniTask.FromResult(false);
        }

        UniTask IScene.Destroy(string next)
        {
            GameObject.Destroy(gameObject);
            return UniTask.CompletedTask;
        }

        void IScene.DestroyImmediate(string next)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
