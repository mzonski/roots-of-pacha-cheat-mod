using SodaDen.Pacha;
using HarmonyLib;
namespace CheatMod.MelonLoader;

public partial class CheatMod
{
    [HarmonyPatch(typeof(SeedInventoryEntity), "RemoveItem")]
    private class InfiniteSeedsPatch
    {
        private static bool Prefix()
        {
            return !_pachaManager.Config.IsInfiniteSeedsEnabled;
        }
    }
}