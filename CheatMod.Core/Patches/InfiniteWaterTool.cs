using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(WaterToolItem), "Use")]
    [HarmonyPrefix]
    private static bool InfiniteWaterToolPatch(WaterToolItem __instance, InventoryEntity entity)
    {
        if (CheatOptions.Instance.IsInfiniteWaterToolEnabled.Value)
            __instance.ToolPropertyWithData(entity).Level = __instance.MaxLevel;

        return true;
    }
}