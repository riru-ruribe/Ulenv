#pragma warning disable UNT0008
using UnityEngine;

namespace Ulenv
{
    public static class TransformExtensions
    {
        const string Ignore = "Canvas (Environment)";

        public static string NameWithParent(this Transform self)
        {
            var ret = self?.name;
            var t = self?.parent;
            while (t != null)
            {
                ret = $"{t.name}/{ret}";
                t = t.parent;
            }
            return ret;
        }

        public static string NameWithRoot(this Transform self)
        {
            var ret = self?.name;
            var t = self?.parent;
            while (t?.parent is { } p && p.name != Ignore) t = t.parent;
            return t != null && t.name != Ignore ? $"{t.name}/{ret}" : ret;
        }
    }
}
