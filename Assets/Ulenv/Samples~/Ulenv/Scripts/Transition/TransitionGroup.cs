using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Ulenv
{
    public sealed class TransitionGroup : MonoBehaviour, IGroup
    {
        public const string Name = nameof(TransitionGroup);

        [SerializeField] Camera transitionCamera = default;
        [SerializeField] Image image = default;
        [SerializeField] float duration = 0.5f;

        UniversalAdditionalCameraData cameraData;

        string IGroup.GroupName => Name;

        void IGroup.Initialize(IUlenv env)
        {
            cameraData = env.RootCamera.GetUniversalAdditionalCameraData();
            gameObject.SetActive(false);
        }

        public async UniTask Open(IScene prev, string next)
        {
            gameObject.SetActive(true);
            cameraData.cameraStack.Add(transitionCamera);

            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                SetAlpha(Mathf.Lerp(0f, 1f, elapsed / duration));
                await UniTask.Yield();
            }
            SetAlpha(1f);
        }

        public async UniTask Close()
        {
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                SetAlpha(Mathf.Lerp(1f, 0f, elapsed / duration));
                await UniTask.Yield();
            }
            SetAlpha(0f);

            cameraData.cameraStack.Remove(transitionCamera);
            gameObject.SetActive(false);
        }

        void SetAlpha(float a)
        {
            var color = image.color;
            color.a = a;
            image.color = color;
        }
    }
}
