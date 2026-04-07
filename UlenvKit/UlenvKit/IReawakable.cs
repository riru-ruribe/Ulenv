namespace Ulenv;

/// <summary>
/// モジュール解決後の通知ハンドラです
/// </summary>
public interface IReawakable
{
    bool Once { get; }
    void Reawaken();
}
