using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.BepInEx5;

public partial class CheatMod
{
    [HarmonyPatch(typeof(WaterToolItem), "Use")]
    [HarmonyPrefix]
    private static bool InfiniteWaterToolPatch(WaterToolItem __instance, InventoryEntity entity)
    {
        if (PachaManager.Config.IsInfiniteWaterToolEnabled)
            __instance.ToolPropertyWithData(entity).Level = __instance.MaxLevel;

        return true;
    }
}