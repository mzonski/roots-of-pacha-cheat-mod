using SodaDen.Pacha;
using HarmonyLib;
namespace RootsOfPachaCheatMod;

public partial class CheatMod
{
    [HarmonyPatch(typeof(PlayerStateController), "CheckIfAllowedAction")]
    private class InfiniteStaminaPatch
    {
        private static bool Prefix(PlayerStateController __instance)
        {
            if (_pachaManager.Config.IsInfiniteStaminaEnabled)
                __instance.PlayerEntity.Stats.SetStamina(__instance.PlayerEntity.Stats.MaxStamina);

            return true;
        }
    }
}