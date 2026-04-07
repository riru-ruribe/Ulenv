namespace Ulenv
{
    /// <summary>
    /// 自由記述型解決モデルです
    /// </summary>
    public interface INobodyResolvable : IAwakable
    {
        void OnDestroy();
        void OnNotify();
    }
}
