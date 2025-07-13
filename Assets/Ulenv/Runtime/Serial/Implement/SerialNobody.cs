using RefMata;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Ulenv
{
    [RefMatable]
    public sealed partial class SerialNobody : SerialComponent<SerialNobody>, IAwakable, IDisposable
    {
        [SerializeReference, InterfaceField] INobodyResolvable resolvable = default;
        public override SerialNobody Value => this;
        void IAwakable.Awaken(IUlenv env, IModuleMap moduleMap) => resolvable.Awaken(env, moduleMap);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Notify() => resolvable.OnNotify();
        public void Dispose() => resolvable.OnDestroy();
    }
}
