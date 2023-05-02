using SodaDen.Pacha;
using HarmonyLib;
namespace RootsOfPachaCheatMod;

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