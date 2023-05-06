using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.BepInEx5;

public partial class CheatMod
{
    [HarmonyPatch(typeof(PlayerStateController), "MaxSpeed", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool PlayerStateMaxSpeedPatch(ref float? __result)
    {
        if (!PachaManager.Config.IsMovementSpeedEnabled) return true;
        __result = 100f;
        return false;
    }


    [HarmonyPatch(typeof(PlayerStateController), "Speed", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool PlayerStateSpeedPatch(ref float __result)
    {
        if (!PachaManager.Config.IsMovementSpeedEnabled) return true;
        __result = PachaManager.Config.PlayerMovementSpeed;
        return false;
    }
}