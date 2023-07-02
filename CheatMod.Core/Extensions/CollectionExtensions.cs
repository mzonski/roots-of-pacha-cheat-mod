using System;
using System.Collections.Generic;
using System.Linq;

namespace CheatMod.Core.Extensions;

public static class CollectionExtensions
{
    private static Random random = new Random();

    public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        // If there are no elements in the collection, return the default value of T
        return list.Count == 0 ? default : list.ElementAt(random.Next(list.Count));
    }
}