namespace Ulenv
{
    /// <summary>
    /// 環境内のグループモデルです<br/>
    /// 主に'Canvas'などを想定しています
    /// </summary>
    public interface IGroup : IComponent
    {
        string Name { get; }
        void Initialize(IUlenv env);
    }
}
