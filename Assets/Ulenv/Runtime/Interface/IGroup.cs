namespace Ulenv
{
    /// <summary>
    /// 環境内のグループモデルです<br/>
    /// 主に'Canvas'などを想定しています
    /// </summary>
    public interface IGroup : IComponent
    {
        void Initialize(IUlenv env);
    }
}
