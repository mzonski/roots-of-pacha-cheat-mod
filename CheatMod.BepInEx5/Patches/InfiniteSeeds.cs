using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.BepInEx5;

public partial class CheatMod
{
    [HarmonyPatch(typeof(SeedInventoryEntity), "RemoveItem")]
    [HarmonyPrefix]
    private static bool InfiniteSeedsPatch()
    {
        return !PachaManager.Config.IsInfiniteSeedsEnabled;
    }
}