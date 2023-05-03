using System;

namespace RootsOfPachaCheatMod.Extensions;

public static class GuidExtensions
{
    public static string ToBase64String(this Guid guid) => Convert.ToBase64String(guid.ToByteArray())
        .Replace('+', '-')
        .Replace('/', '_')
        .Substring(0, 22);
}