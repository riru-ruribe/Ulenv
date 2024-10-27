using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Ulenv
{
    public sealed class DialogGroup : MonoBehaviour, IGroup
    {
        public const string Name = nameof(DialogGroup);

        [SerializeField] Camera dialogCamera = default;
        [SerializeField] RectTransform container = default;

        UniversalAdditionalCameraData cameraData;
        readonly List<Dialog> dialogs = new();

        string IGroup.GroupName => Name;
        Transform IComponent.transform => container;

        void IGroup.Initialize(IUlenv env)
        {
            cameraData = env.RootCamera.GetUniversalAdditionalCameraData();
            gameObject.SetActive(false);
        }

        public void Active(Dialog added)
        {
            if (dialogs.Count == 0)
            {
                gameObject.SetActive(true);
                cameraData.cameraStack.Add(dialogCamera);
            }
            foreach (var dialog in dialogs)
            {
                if (dialog < added)
                {
                    dialog.Touchable = false;
                }
            }
            dialogs.Add(added);
        }

        public void Deactive(Dialog removed)
        {
            dialogs.Remove(removed);
            if (dialogs.Count == 0)
            {
                cameraData.cameraStack.Remove(dialogCamera);
                gameObject.SetActive(false);
                return;
            }
            dialogs.Max().Touchable = true;
        }
    }
}
