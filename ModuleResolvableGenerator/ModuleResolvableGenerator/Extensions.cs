using Microsoft.CodeAnalysis;

namespace ModuleResolvableGenerator;

public static class Extensions
{
    public static bool IsParent(this INamedTypeSymbol symbol, string disp)
    {
        var type = symbol.BaseType;
        while (type != null)
        {
            if (type.ToString() == disp) return true;
            type = type.BaseType;
        }
        return false;
    }
}
