using RefMata;
using UnityEngine;
using UnityEngine.UI;

namespace Ulenv
{
    [RefMatable]
    sealed partial class SerialScrollRect : SerialComponent<ScrollRect>
    {
        [SerializeField, RefMataMe] ScrollRect value = default;
        public override ScrollRect Value => value;
    }
}
