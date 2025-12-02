using System.Threading;
using UnityEngine;

namespace Ulenv
{
    public unsafe static class UlCallbackExtensions
    {
        public static UlUnsafeCallback ToUlUnsafeCallback<T>(this object mc) where T : unmanaged, IUlCallback
        {
            static void dlg(object obj, object args, int i) => new T().Recept(obj, args, i);
            return new(mc, &dlg);
        }

        public static void Register<T>(this IUlCallbackHolder h, MonoBehaviour mc) where T : unmanaged, IUlCallback
            => Register<T>(h, mc, mc.destroyCancellationToken);

        public static void Register<T>(this IUlCallbackHolder h, MonoBehaviour mc, CancellationToken ct) where T : unmanaged, IUlCallback
        {
            static void dlg(object obj, object args, int code) => new T().Recept(obj, args, code);
            h.Callback = new(mc, &dlg, ct);
        }

        public static void Register<T>(this IUlCallbackHolder h, object mc) where T : unmanaged, IUlCallback
            => Register<T>(h, mc, h.destroyCancellationToken);

        public static void Register<T>(this IUlCallbackHolder h, object mc, CancellationToken ct) where T : unmanaged, IUlCallback
        {
            static void dlg(object obj, object args, int code) => new T().Recept(obj, args, code);
            h.Callback = new(mc, &dlg, ct);
        }
    }
}
