using System;

namespace CheatMod.Core.Extensions;

public static class GuidExtensions
{
    public static string ToBase64String(this Guid guid) => Convert.ToBase64String(guid.ToByteArray())
        .Replace('+', '-')
        .Replace('/', '_')
        .Substring(0, 22);
}