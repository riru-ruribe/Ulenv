namespace Ulenv
{
    /// <summary>
    /// モジュール解決時のハンドラです
    /// </summary>
    public interface IAwakable
    {
        void Awaken(IUlenv env, IModuleMap moduleMap);
    }
}
