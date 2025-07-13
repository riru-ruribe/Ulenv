using RefMata;
using UnityEngine;
using UnityEngine.UI;

namespace Ulenv
{
    [RefMatable]
    sealed partial class SerialButton : SerialComponent<Button>
    {
        [SerializeField, RefMataMe] Button value = default;
        public override Button Value => value;
    }
}
