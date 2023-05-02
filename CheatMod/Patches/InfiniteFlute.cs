using SodaDen.Pacha;
using HarmonyLib;

namespace RootsOfPachaCheatMod;

public partial class CheatMod
{
    [HarmonyPatch(typeof(FluteToolItem), "Use")]
    private class InfiniteFlutePatch
    {
        private static bool Prefix(FluteToolItem __instance, PlayerEntity player, InventoryEntity entity,
            float stamina)
        {
            if (_pachaManager.Config.IsInfiniteFluteEnabled)
                __instance.ToolPropertyWithData(entity).Level = __instance.MaxLevel;

            return true;
        }
    }
}