namespace Ulenv
{
    /// <summary>
    /// 依存解決モデル取得モデルです
    /// </summary>
    public interface IModuleMapProvider
    {
        IModuleMap ModuleMap { get; }
    }
}
