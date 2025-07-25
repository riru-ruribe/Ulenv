using UnityEngine;
using UnityEngine.UI;

namespace Ulenv
{
#if EXIST_REFMATA
    [RefMata.RefMatable]
#endif
    sealed partial class SerialButton : SerialComponent<Button>
    {
#if EXIST_REFMATA
        [RefMata.RefMataMe]
#endif
        [SerializeField] Button value = default;
        public override Button Value => value;
    }
}
