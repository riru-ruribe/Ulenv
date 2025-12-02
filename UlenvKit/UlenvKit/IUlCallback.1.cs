namespace Ulenv;

public interface IUlCallback<T> where T : unmanaged
{
    void Recept(T identifier, object args, int code);
}
