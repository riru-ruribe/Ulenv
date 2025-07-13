#if EXIST_REFMATA
using RefMata;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ulenv
{
    [RefMatable]
    public sealed partial class ModuleResolver : MonoBehaviour, IDisposable
    {
        [RefMataChild(true, "Where(x => x.GetComponent<IResolvable>() != null)")]
        [SerializeField] GameObject[] resolvables = default;

        readonly List<ModuleScope> scopes = new();

        public void Resolve(IModuleMap moduleMap)
        {
            for (int i = 0; i < resolvables.Length; i++)
                scopes.Add(moduleMap.Resolve(resolvables[i].GetComponent<IResolvable>()));
        }

        public void Resolve(IUlenv env, IModuleMap moduleMap)
        {
            for (int i = 0; i < resolvables.Length; i++)
            {
                var resolvable = resolvables[i].GetComponent<IResolvable>();
                scopes.Add(moduleMap.Resolve(resolvable));
                (resolvable as IAwakable)?.Awaken(env, moduleMap);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < scopes.Count; i++)
                scopes[i].Dispose();
            scopes.Clear();
        }

        void OnDestroy() => Dispose();
    }
}
#endif
