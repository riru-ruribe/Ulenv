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
        void AddGroup(IGroup group);
        IGroup CreateGroup(IGroup prefab);
        void RemoveGroup(string groupName);
    }
}
