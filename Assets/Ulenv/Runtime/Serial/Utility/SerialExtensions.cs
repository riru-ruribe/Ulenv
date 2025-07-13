using System.Runtime.CompilerServices;
using UnityEngine;

namespace Ulenv
{
    public static class SerialExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Serialized<T>(this IModuleMap moduleMap, Unique unique) where T : Component
            => moduleMap.Get<SerialComponent<T>>(unique).Value;
    }
}
