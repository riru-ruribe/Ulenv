namespace Ulenv;

public sealed class UlKeyUniqueGenerator : IUlKeyUniqueGenerator
{
    static readonly ulong KeyGenHash = typeof(UlKeyGenTypes).ToSerialHash();
    public SerialUnique Generate(string relativePath) => new(SerialHash.Get(relativePath), KeyGenHash);
}
