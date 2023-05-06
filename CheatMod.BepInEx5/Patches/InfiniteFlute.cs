using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.BepInEx5;

public partial class CheatMod
{
    [HarmonyPatch(typeof(FluteToolItem), "Use")]
    [HarmonyPrefix]
    private static bool InfiniteFlutePatch(FluteToolItem __instance, PlayerEntity player, InventoryEntity entity,
        float stamina)
    {
        if (PachaManager.Config.IsInfiniteFluteEnabled)
            __instance.ToolPropertyWithData(entity).Level = __instance.MaxLevel;

        return true;
    }
}