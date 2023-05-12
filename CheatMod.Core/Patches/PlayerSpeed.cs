using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(PlayerStateController), "MaxSpeed", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool PlayerStateMaxSpeedPatch(ref float? __result)
    {
        if (!CheatOptions.IsMovementSpeedEnabled) return true;
        __result = 100f;
        return false;
    }


    [HarmonyPatch(typeof(PlayerStateController), "Speed", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool PlayerStateSpeedPatch(ref float __result)
    {
        if (!CheatOptions.IsMovementSpeedEnabled) return true;
        __result = CheatOptions.PlayerMovementSpeed;
        return false;
    }
}