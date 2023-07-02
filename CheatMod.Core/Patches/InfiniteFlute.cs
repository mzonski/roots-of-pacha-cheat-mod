using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(FluteToolItem), "Use")]
    [HarmonyPrefix]
    private static bool InfiniteFlutePatch(FluteToolItem __instance, PlayerEntity player, InventoryEntity entity,
        float stamina)
    {
        if (CheatOptions.IsInfiniteFluteEnabled)
            __instance.ToolPropertyWithData(entity).Level = __instance.MaxLevel;

        return true;
    }
}