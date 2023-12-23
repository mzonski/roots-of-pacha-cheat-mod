using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(Session), "AdvanceTime")]
    [HarmonyPrefix]
    private static bool FreezeTimePatch()
    {
        return !CheatOptions.Instance.IsFreezeTimeEnabled.Value;
    }
}