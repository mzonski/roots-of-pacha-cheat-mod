using SodaDen.Pacha;
using HarmonyLib;
namespace CheatMod.MelonLoader;

public partial class CheatMod
{
    [HarmonyPatch(typeof(WaterToolItem), "Use")]
    private class InfiniteWaterToolPatch
    {
        private static bool Prefix(WaterToolItem __instance, InventoryEntity entity)
        {
            if (_pachaManager.Config.IsInfiniteWaterToolEnabled)
                __instance.ToolPropertyWithData(entity).Level = __instance.MaxLevel;

            return true;
        }
    }
}