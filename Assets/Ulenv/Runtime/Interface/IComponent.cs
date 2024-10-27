#pragma warning disable IDE1006
using UnityEngine;

namespace Ulenv
{
    /// <summary>
    /// 'MonoBehaviour'抽象モデルです
    /// </summary>
    public interface IComponent
    {
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}
