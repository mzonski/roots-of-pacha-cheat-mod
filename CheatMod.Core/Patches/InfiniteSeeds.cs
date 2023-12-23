using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(SeedInventoryEntity), "RemoveItem")]
    [HarmonyPrefix]
    private static bool InfiniteSeedsPatch()
    {
        return !CheatOptions.Instance.IsInfiniteSeedsEnabled.Value;
    }
}