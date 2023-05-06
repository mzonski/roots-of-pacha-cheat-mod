using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.BepInEx5;

public partial class CheatMod
{
    [HarmonyPatch(typeof(PlayerStateController), "CheckIfAllowedAction")]
    [HarmonyPrefix]
    private static bool InfiniteStaminaPatch(PlayerStateController __instance)
    {
        if (PachaManager.Config.IsInfiniteStaminaEnabled)
            __instance.PlayerEntity.Stats.SetStamina(__instance.PlayerEntity.Stats.MaxStamina);

        return true;
    }
}