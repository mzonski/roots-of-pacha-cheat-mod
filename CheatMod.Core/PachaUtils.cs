using System.Collections.Generic;
using System.Reflection;
//using MelonLoader;
using SodaDen.Pacha;
using SodaDen.Pacha.UI;
using UnityEngine;

namespace CheatMod.Core;

public static class PachaUtils
{
    public static int NormalizeQty(int val)
    {
        return val switch
        {
            < 0 => 0,
            > 255 => 255,
            _ => val
        };
    }
}