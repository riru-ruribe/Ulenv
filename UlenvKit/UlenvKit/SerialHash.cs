#pragma warning disable IDE0079
#pragma warning disable IDE0005
using System;
using System.Runtime.CompilerServices;

namespace Ulenv;

public static class SerialHash
{
    public static ulong Get(string str)
    {
        var hash = 0uL;
        for (int i = 0; i < str.Length; i++)
        {
            hash *= 31;
            hash += str[i];
        }
        return hash;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ToSerialHash(this Type type) => Get(type.FullName);
}
