using UnityEngine;

namespace Ulenv
{
    [ExecuteInEditMode]
    public abstract class SerialComponent<T> : MonoBehaviour, IResolvable where T : Component
    {
        [SerializeField] SerialUnique unique = default;
        [SerializeField, HideInInspector] string savedPath = default;
        Unique IResolvable.Unique => unique;
        public abstract T Value { get; }
#if UNITY_EDITOR
        void LateUpdate()
        {
            if (UnityEditor.EditorApplication.isPlaying) return;
            var path = transform.NameWithRoot();
            if (savedPath != path || !SerialMap.Any(savedPath))
            {
                unique = new(SerialHash.Get(path), typeof(T).ToSerialHash());
                SerialMap.Replace(savedPath, path, unique);
                savedPath = path;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
        protected virtual void OnDestroy()
        {
            if (UnityEditor.EditorApplication.isPlaying || !SerialMap.Staging) return;
            SerialMap.Remove(savedPath);
        }
#endif
    }
}
