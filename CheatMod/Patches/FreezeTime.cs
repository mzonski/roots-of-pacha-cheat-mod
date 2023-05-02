using HarmonyLib;
using SodaDen.Pacha;

namespace RootsOfPachaCheatMod;

public partial class CheatMod
{
    [HarmonyPatch(typeof(Session), "AdvanceTime")]
    private class FreezeTimePatch
    {
        private static bool Prefix()
        {
            return !_pachaManager.Config.IsFreezeTimeEnabled;
        }
    }
}