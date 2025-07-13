using RefMata;
using UnityEngine;

namespace Ulenv
{
    [RefMatable]
    sealed partial class SerialRectTransform : SerialComponent<RectTransform>
    {
        [SerializeField, RefMataMe] RectTransform value = default;
        public override RectTransform Value => value;
    }
}
