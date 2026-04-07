namespace Ulenv
{
    /// <summary>
    /// 自由記述型解決モデルです
    /// </summary>
    public interface INobodyResolvable : IAwakable, IReawakable
    {
        void OnDestroy();
    }
}
