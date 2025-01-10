namespace Ulenv
{
    /// <summary>
    /// 'ModuleResolvableAttribute'で'Unique'を自動実装します
    /// </summary>
    public interface IModuleResolvable
    {
        ModuleAddScope Resolve(IModuleMap moduleMap);
    }
}
