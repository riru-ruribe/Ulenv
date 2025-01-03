using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ulenv
{
    [DefaultExecutionOrder(-10000)]
    public sealed class DefaultUlenv : MonoBehaviour, IUlenv
    {
        [SerializeField] Camera rootCamera = default;
        [SerializeField] GameObject[] initialGroups = default;

        readonly Dictionary<string, IGroup> groupMap = new();

        public Camera RootCamera => rootCamera;
        public Transform RootTransform => transform;

        public IGroup GetGroup(string groupName) => groupMap[groupName];

        public T GetGroup<T>(string groupName = null) where T : IGroup
            => (T)groupMap[groupName ?? typeof(T).Name];

        public void AddGroup(IGroup group)
        {
            UnityEngine.Assertions.Assert.IsFalse(groupMap.ContainsKey(group.Name));
            group.transform.SetParent(transform);
            group.Initialize(this);
            groupMap[group.Name] = group;
        }

        public IGroup CreateGroup(IGroup prefab) => CreateGroup<IGroup>(prefab);

        public T CreateGroup<T>(IGroup prefab) where T : IGroup
        {
            UnityEngine.Assertions.Assert.IsFalse(groupMap.ContainsKey(prefab.Name));
            var group = GameObject
                .Instantiate(prefab.gameObject, transform, false)
                .GetComponent<T>();
            group.Initialize(this);
            groupMap[group.Name] = group;
            return group;
        }

        public void RemoveGroup(string groupName)
        {
            UnityEngine.Assertions.Assert.IsTrue(groupMap.ContainsKey(groupName));
            GameObject.Destroy(groupMap[groupName].gameObject);
            groupMap.Remove(groupName);
        }

        void Awake()
        {
            foreach (var group in initialGroups.Select(x => x.GetComponent<IGroup>()))
            {
                groupMap[group.Name] = group;
            }
            foreach (var group in groupMap.Values)
            {
                group.Initialize(this);
            }
        }
    }
}
