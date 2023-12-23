using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(PlayerStateController), "CheckIfAllowedAction")]
    [HarmonyPrefix]
    private static bool InfiniteStaminaPatch(PlayerStateController __instance)
    {
        if (CheatOptions.Instance.IsInfiniteStaminaEnabled.Value)
            __instance.PlayerEntity.Stats.SetStamina(__instance.PlayerEntity.Stats.MaxStamina);

        return true;
    }
}