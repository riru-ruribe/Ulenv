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
        IModuleMap Module { get; }
    }
}
