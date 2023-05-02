using HarmonyLib;
using SodaDen.Pacha;

namespace RootsOfPachaCheatMod;

public partial class CheatMod
{
    [HarmonyPatch(typeof(Session), "AdvanceTime")]
    class FreezeTimePatch
    {
        static bool Prefix()
        {
            return !_pachaManager.Config.IsFreezeTimeEnabled;
        }
    }
}