using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ulenv
{
#if EXIST_REFMATA
    [RefMata.RefMatable]
#endif
    public sealed partial class ModuleResolver : MonoBehaviour, IDisposable
    {
#if EXIST_REFMATA
        [RefMata.RefMataChild(true, "Where(x => x.GetComponent<IResolvable>() != null)")]
#endif
        [SerializeField] GameObject[] resolvables = default;

        readonly List<ModuleScope> scopes = new();

        public void Resolve(IModuleMap moduleMap)
        {
            for (int i = 0; i < resolvables.Length; i++)
                scopes.Add(moduleMap.Resolve(resolvables[i].GetComponent<IResolvable>()));
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
