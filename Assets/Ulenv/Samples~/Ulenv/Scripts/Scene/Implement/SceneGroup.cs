using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Ulenv
{
    public sealed class SceneGroup : MonoBehaviour, IGroup
    {
        public const string Name = nameof(SceneGroup);

        [SerializeField, GroupSelector] string rootGroupName = default;

        readonly ModuleMap moduleMap = new();
        readonly Stack<ISceneGift> stack = new();
        CancellationToken cancellationToken;
        IUlenv env;
        IGroup rootGroup;
        TransitionGroup transitionGroup;
        IScene currentScene;
        ISceneGift currentGift;
        bool isProgress;

        string IGroup.GroupName => Name;
        public bool IsCurrent => !string.IsNullOrEmpty(currentGift?.Address);

        void IGroup.Initialize(IUlenv env)
        {
            cancellationToken = destroyCancellationToken;
            this.env = env;
            rootGroup = env.GetGroup(rootGroupName);
            moduleMap[SceneModuleUniques.Env] = env;
            moduleMap[SceneModuleUniques.Dialog] = env.GetGroup<DialogGroup>();
            transitionGroup = env.GetGroup<TransitionGroup>();
            moduleMap[SceneModuleUniques.Transition] = transitionGroup;
        }

        public async UniTask<IScene> Change(string address, bool canBack = true)
        {
            return await Change(address, rootGroup, canBack);
        }
        public async UniTask<IScene> Change(string address, string groupName, bool canBack = true)
        {
            return await Change(address, env.GetGroup(groupName) ?? rootGroup, canBack);
        }
        public async UniTask<IScene> Change(string address, IGroup group, bool canBack = true)
        {
            if (isProgress) return null;
            isProgress = true;

            // check logic current scene.
            if (currentScene != null && await currentScene.ShouldStay(address))
            {
                isProgress = false;
                return null;
            }
            if (cancellationToken.IsCancellationRequested) return null;

            UnityEngine.Assertions.Assert.IsNotNull(address);
            UnityEngine.Assertions.Assert.IsNotNull(group);

            // open transition, hide screens.
            await transitionGroup.Open(currentScene, address);
            if (cancellationToken.IsCancellationRequested) return null;

            // destroy current scene.
            if (currentScene != null)
            {
                await currentScene.Destroy(address);
                if (cancellationToken.IsCancellationRequested) return null;
                if (currentScene.Childs.Count > 0)
                {
                    Debug.LogWarning("yet exists additional scenes. 'ShouldStay' is not called.");
                    foreach (var child in currentScene.Childs)
                    {
                        child.DestroyImmediate(address);
                    }
                }
            }

            // create new scene.
            var go = await Addressables
                .InstantiateAsync(address, group.transform, false, true)
                .ToUniTask();
            if (cancellationToken.IsCancellationRequested) return null;

            // initialize new scene.
            var scene = go.GetComponent<IScene>();
            var gift = await scene.Initialize(moduleMap, currentGift);
            if (cancellationToken.IsCancellationRequested) return null;

            UnityEngine.Assertions.Assert.IsNotNull(gift);
            if (canBack) stack.Push(currentGift);
            currentScene = scene;
            currentGift = gift;

            // close transition, show screens.
            await transitionGroup.Close();
            if (cancellationToken.IsCancellationRequested) return null;

            isProgress = false;
            return scene;
        }

        public async UniTask<IScene> Add(string address)
        {
            return await Add(address, rootGroup);
        }
        public async UniTask<IScene> Add(string address, string groupName)
        {
            return await Add(address, env.GetGroup(groupName) ?? rootGroup);
        }
        public async UniTask<IScene> Add(string address, IGroup group)
        {
            if (isProgress) return null;
            isProgress = true;

            UnityEngine.Assertions.Assert.IsNotNull(address);
            UnityEngine.Assertions.Assert.IsNotNull(group);
            UnityEngine.Assertions.Assert.IsNotNull(currentScene);
            UnityEngine.Assertions.Assert.IsNotNull(currentGift);
            UnityEngine.Assertions.Assert.IsTrue(currentScene.Childs.Count == currentGift.Childs.Count);

            // create new scene.
            var go = await Addressables
                .InstantiateAsync(address, group.transform, false, true)
                .ToUniTask();
            if (cancellationToken.IsCancellationRequested) return null;

            // initialize new scene.
            var scene = go.GetComponent<IScene>();
            var gift = await scene.Initialize(moduleMap, currentGift);
            if (cancellationToken.IsCancellationRequested) return null;

            UnityEngine.Assertions.Assert.IsNotNull(gift);
            currentScene.Childs.Add(scene);
            currentGift.Childs.Add(gift);

            isProgress = false;
            return scene;
        }

        public async UniTask Back()
        {
            if (isProgress) return;
            isProgress = true;

            // destroy top additional scene.
            if (currentScene != null && currentScene.Childs.Count > 0)
            {
                var prevAddress = currentGift.Childs.Count >= 2 ? currentGift.Childs[^2].Address : null;
                if (await currentScene.Childs[^1].ShouldStay(prevAddress))
                {
                    isProgress = false;
                    return;
                }
                if (cancellationToken.IsCancellationRequested) return;

                await currentScene.Childs[^1].Destroy(prevAddress);
                if (cancellationToken.IsCancellationRequested) return;

                currentScene.Childs.RemoveAt(currentScene.Childs.Count - 1);
                currentGift.Childs.RemoveAt(currentGift.Childs.Count - 1);
                isProgress = false;
                return;
            }

            // destroy current scene. and back previous scene.
            if (stack.Count > 0)
            {
                if (await currentScene.ShouldStay(stack.Peek().Address))
                {
                    isProgress = false;
                    return;
                }
                if (cancellationToken.IsCancellationRequested) return;

                isProgress = false; // to execute 'Change'.
                var prev = stack.Pop();
                if (null == await Change(prev.Address, prev.GroupName, canBack: false))
                {
                    isProgress = false;
                    return;
                }
                if (cancellationToken.IsCancellationRequested) return;

                foreach (var child in prev.Childs)
                {
                    isProgress = false; // to execute 'Add'.
                    if (null == await Add(child.Address, child.GroupName))
                    {
                        isProgress = false;
                        return;
                    }
                    if (cancellationToken.IsCancellationRequested) return;
                }

                isProgress = false;
                return;
            }

            // destroy current scene.
            if (currentScene == null || await currentScene.ShouldStay(null))
            {
                isProgress = false;
                return;
            }
            if (cancellationToken.IsCancellationRequested) return;

            await currentScene.Destroy(null);
            isProgress = false;
        }

        public void BackImmediate()
        {
            // immediate means not operate 'isProgress'.

            // assumes destroying the additional scene.
            UnityEngine.Assertions.Assert.IsTrue(currentScene != null && currentScene.Childs.Count > 0);

            // destroy top additional scene.
            var prevAddress = currentGift.Childs.Count >= 2 ? currentGift.Childs[^2].Address : null;
            currentScene.Childs[^1].Destroy(prevAddress).Forget();

            currentScene.Childs.RemoveAt(currentScene.Childs.Count - 1);
            currentGift.Childs.RemoveAt(currentGift.Childs.Count - 1);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back().Forget();
            }
        }
    }
}
