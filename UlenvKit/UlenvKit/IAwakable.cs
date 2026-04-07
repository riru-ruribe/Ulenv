namespace Ulenv;

/// <summary>
/// モジュール解決時のハンドラです
/// </summary>
public interface IAwakable
{
    bool Once { get; }
    void Awaken(IModuleMap moduleMap);
}
