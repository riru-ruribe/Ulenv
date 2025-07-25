using UnityEngine;
using UnityEngine.UI;

namespace Ulenv
{
#if EXIST_REFMATA
    [RefMata.RefMatable]
#endif
    sealed partial class SerialScrollRect : SerialComponent<ScrollRect>
    {
#if EXIST_REFMATA
        [RefMata.RefMataMe]
#endif
        [SerializeField] ScrollRect value = default;
        public override ScrollRect Value => value;
    }
}
