#pragma warning disable IDE0220
using System.Collections.Generic;
using UnityEngine;

namespace Ulenv
{
    [DefaultExecutionOrder(-10000)]
#if EXIST_REFMATA
    [RefMata.RefMatable]
#endif
    public sealed partial class DefaultUlenv : MonoBehaviour, IUlenv
    {
#if EXIST_REFMATA
        [RefMata.RefMataChild]
#endif
        [SerializeField] Camera rootCamera = default;
#if EXIST_REFMATA
        [RefMata.RefMataChild(true, "Where(x => x.GetComponent<IGroup>() != null && x.GetComponent<IResolvable>() != null)")]
#endif
        [SerializeField] GameObject[] resolvables = default;

        readonly ModuleMap moduleMap = new();
        readonly List<ModuleScope> scopes = new();

        public Camera RootCamera => rootCamera;
        public Transform RootTransform => transform;
        public IModuleMap Module => moduleMap;

        void Awake()
        {
            for (int i = 0; i < resolvables.Length; i++)
                scopes.Add(moduleMap.Resolve(resolvables[i].GetComponent<IResolvable>()));
            foreach (IGroup group in moduleMap.Values)
                group.Initialize(this);
        }

        void OnDestroy()
        {
            for (int i = 0; i < scopes.Count; i++)
                scopes[i].Dispose();
            scopes.Clear();
        }
    }
}
