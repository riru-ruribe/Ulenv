#pragma warning disable IDE1006
using System.Threading;
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
        CancellationToken destroyCancellationToken { get; }
    }
}
