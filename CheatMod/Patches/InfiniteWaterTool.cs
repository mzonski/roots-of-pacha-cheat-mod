using SodaDen.Pacha;
using HarmonyLib;
namespace RootsOfPachaCheatMod;

public partial class CheatMod
{
    [HarmonyPatch(typeof(WaterToolItem), "Use")]
    private class InfiniteWaterToolPatch
    {
        private static bool Prefix(WaterToolItem __instance, PlayerEntity player, InventoryEntity entity,
            float stamina)
        {
            if (_pachaManager.Config.IsInfiniteWaterToolEnabled)
                __instance.ToolPropertyWithData(entity).Level = __instance.MaxLevel;

            return true;
        }
    }
}