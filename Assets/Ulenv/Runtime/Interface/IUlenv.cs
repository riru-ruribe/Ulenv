using UnityEngine;

namespace Ulenv
{
    /// <summary>
    /// メイン環境モデルです
    /// </summary>
    public interface IUlenv
    {
        Camera RootCamera { get; }
        Transform RootTransform { get; }

        IGroup GetGroup(string groupName);
        T GetGroup<T>(string groupName = null) where T : IGroup;

        void AddGroup(IGroup group);

        IGroup CreateGroup(IGroup prefab);
        T CreateGroup<T>(IGroup prefab) where T : IGroup;

        void RemoveGroup(string groupName);
    }
}
