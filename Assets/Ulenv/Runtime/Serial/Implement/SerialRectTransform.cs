using UnityEngine;

namespace Ulenv
{
#if EXIST_REFMATA
    [RefMata.RefMatable]
#endif
    sealed partial class SerialRectTransform : SerialComponent<RectTransform>
    {
#if EXIST_REFMATA
        [RefMata.RefMataMe]
#endif
        [SerializeField] RectTransform value = default;
        public override RectTransform Value => value;
    }
}
