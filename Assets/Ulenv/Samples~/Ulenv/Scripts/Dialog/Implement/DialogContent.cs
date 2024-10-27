using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ulenv
{
    public sealed class DialogContent : MonoBehaviour, IDialogContent
    {
        public const string Name = nameof(DialogContent);

        sealed class Gift : ISceneGift, IUnique
        {
            public string Address { get; set; }
            public string GroupName { get; set; }
            public IList<ISceneGift> Childs => throw new NotImplementedException();
            public Unique Unique { get; set; }
        }

        [SerializeField, GroupSelector] string groupName = default;
        [SerializeField] Button closeButton = default;

        IUlenv env;
        SceneGroup sceneGroup;

        public UniTask<ISceneGift> Initialize(IModuleMap moduleMap, ISceneGift prev, Unique unique)
        {
            env = moduleMap.Get<IUlenv>(SceneModuleUniques.Env);
            sceneGroup = env.GetGroup<SceneGroup>();
            closeButton.onClick.AddListener(() => sceneGroup.Back().Forget());
            return UniTask.FromResult<ISceneGift>(new Gift
            {
                Address = Name,
                GroupName = groupName,
                Unique = unique,
            });
        }

        public void OnOpen()
        {
        }

        public UniTask<byte> GetCode()
        {
            return UniTask.FromResult<byte>(0);
        }

        public void Dispose()
        {
        }
    }
}
