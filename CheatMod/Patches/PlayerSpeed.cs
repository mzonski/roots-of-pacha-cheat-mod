using SodaDen.Pacha;
using HarmonyLib;
using UnityEngine;

namespace RootsOfPachaCheatMod;

public partial class CheatMod
{
    [HarmonyPatch(typeof(PlayerStateController), "MaxSpeed", MethodType.Getter)]
    private class PlayerStateMaxSpeedPatch
    {
        private static bool Prefix(PlayerStateController __instance, ref float? __result)
        {
            if (!_pachaManager.Config.IsMovementSpeedEnabled) return true;
            __result = 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerStateController), "Speed", MethodType.Getter)]
    private class PlayerStateSpeedPatch
    {
        private static bool Prefix(PlayerStateController __instance, ref float __result)
        {
            if (!_pachaManager.Config.IsMovementSpeedEnabled) return true;
            __result = _pachaManager.Config.PlayerMovementSpeed;
            return false;
        }
    }
}