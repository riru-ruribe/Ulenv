#if EXIST_REFMATA
using RefMata;
using System.Collections.Generic;
using UnityEngine;

namespace Ulenv
{
    [RefMatable]
    public sealed partial class ModuleResolver : MonoBehaviour
    {
        [RefMataChild(true, "Where(x => x.GetComponent<IModuleResolvable>() != null)")]
        [SerializeField] GameObject[] resolvables = default;

        readonly List<ModuleAddScope> scopes = new();

        public void Resolve(IModuleMap moduleMap)
        {
            for (int i = 0; i < resolvables.Length; i++)
            {
                var go = resolvables[i];
                var resolvable = go.GetComponent<IModuleResolvable>();
                scopes.Add(moduleMap.AddScoped(resolvable.Unique, resolvable));
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < scopes.Count; i++)
            {
                scopes[i].Dispose();
            }
            scopes.Clear();
        }
    }
}
#endif
