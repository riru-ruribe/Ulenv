using System;
using UnityEngine;

namespace Ulenv
{
#if EXIST_REFMATA
    [RefMata.RefMatable]
#endif
    public sealed partial class SerialNobody : SerialComponent<SerialNobody>, IAwakable, IReawakable, IDisposable
    {
        [SerializeReference, InterfaceField] INobodyResolvable resolvable = default;
        public override SerialNobody Value => this;
        bool IAwakable.Once => (resolvable as IAwakable).Once;
        bool IReawakable.Once => (resolvable as IReawakable).Once;
        void IAwakable.Awaken(IModuleMap moduleMap) => resolvable.Awaken(moduleMap);
        public void Reawaken() => resolvable.Reawaken();
        public void Dispose() => resolvable.OnDestroy();
    }
}
