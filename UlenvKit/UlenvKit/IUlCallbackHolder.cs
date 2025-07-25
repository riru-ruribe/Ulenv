#pragma warning disable IDE1006
using System.Threading;

namespace Ulenv;

public interface IUlCallbackHolder
{
    CancellationToken destroyCancellationToken { get; }
    UlCallback Callback { set; }
}
