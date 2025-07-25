using System.Threading;
using UnityEngine;

namespace Ulenv
{
    public unsafe static class UlCallbackExtensions
    {
        public static void Register<T>(this UlSelectable us, MonoBehaviour mc) where T : unmanaged, IUlCallback
            => Register<T>(us, mc, mc.destroyCancellationToken);

        public static void Register<T>(this UlSelectable us, MonoBehaviour mc, CancellationToken ct) where T : unmanaged, IUlCallback
        {
            static void dlg(object obj, object args, int code) => new T().Recept(obj, args, code);
            us.Callback = new(mc, &dlg, ct);
        }

        public static void Register<T>(this UlSelectable us, object mc) where T : unmanaged, IUlCallback
            => Register<T>(us, mc, us.destroyCancellationToken);

        public static void Register<T>(this UlSelectable us, object mc, CancellationToken ct) where T : unmanaged, IUlCallback
        {
            static void dlg(object obj, object args, int code) => new T().Recept(obj, args, code);
            us.Callback = new(mc, &dlg, ct);
        }
    }
}
