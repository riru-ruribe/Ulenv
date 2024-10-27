using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Ulenv
{
    public sealed class Bootstrap : MonoBehaviour
    {
        IUlenv env;
        SceneGroup sceneGroup;

        void Awake()
        {
            Application.targetFrameRate = 60;
            env = GetComponent<IUlenv>();
            sceneGroup = env.GetGroup<SceneGroup>();
            sceneGroup.Change(TestScene.Name, canBack: false).Forget();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                sceneGroup.Change(TestScene.Name).Forget();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                sceneGroup.Add(DialogContent.Name, env.GetGroup<DialogGroup>()).Forget();
            }
        }
    }
}
