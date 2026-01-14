namespace Ulenv;

/// <summary>
/// ユニーク監視可能モデルです
/// </summary>
public interface IUlObservable
{
    void Subscribe<T>(IResolvable observer) where T : unmanaged, IUlCallback<Unique>;
}
