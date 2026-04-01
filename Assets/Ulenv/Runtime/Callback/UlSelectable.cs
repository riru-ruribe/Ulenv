using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ulenv
{
#if EXIST_REFMATA
    [RefMata.RefMatable]
#endif
    public sealed partial class UlSelectable : MonoBehaviour, IUlCallbackHolder,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler,
        IPointerClickHandler
    {
#if EXIST_REFMATA
        [RefMata.RefMataMe]
#endif
        [SerializeField] Selectable selectable = default;

        UlCallback callback;

        public bool Interactable
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => selectable == null ? enabled : selectable.interactable;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (selectable == null) enabled = value;
                else selectable.interactable = value;
            }
        }

        public bool IsValidCallback
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => callback.IsValid;
        }

        UlCallback IUlCallbackHolder.Callback { set => callback = value; }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (selectable is not { interactable: true }) return;
            callback.Invoke(eventData, 1);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (selectable is not { interactable: true }) return;
            callback.Invoke(eventData, 2);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (selectable is not { interactable: true }) return;
            callback.Invoke(eventData, 3);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (selectable is not { interactable: true }) return;
            callback.Invoke(eventData, 4);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (selectable is not { interactable: true }) return;
            callback.Invoke(eventData, 5);
        }
    }
}
