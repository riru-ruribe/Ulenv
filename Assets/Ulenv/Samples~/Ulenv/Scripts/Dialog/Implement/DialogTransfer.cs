using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ulenv
{
    public sealed class DialogTransfer : MonoBehaviour, IDialogTransfer
    {
        [SerializeField] CanvasGroup canvasGroup = default;
        [SerializeField] float duration = 0.5f;

        public bool Touchable
        {
            set => canvasGroup.blocksRaycasts = value;
        }

        public UniTask Initialize(IModuleMap moduleMap, ISceneGift prev)
        {
            return UniTask.CompletedTask;
        }

        public async UniTask Open()
        {
            canvasGroup.blocksRaycasts = false;

            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
                await UniTask.Yield();
            }
            canvasGroup.alpha = 1f;

            canvasGroup.blocksRaycasts = true;
        }

        public async UniTask Close()
        {
            canvasGroup.blocksRaycasts = false;

            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                await UniTask.Yield();
            }
            canvasGroup.alpha = 0f;
        }

        public void Dispose()
        {
        }
    }
}
