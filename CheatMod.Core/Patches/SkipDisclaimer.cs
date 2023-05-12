using SodaDen.Pacha.UI;

namespace CheatMod.Core.Patches;
using HarmonyLib;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(Disclaimer), "Start")]
    [HarmonyPostfix]
    private static void SkipDisclaimerPatch()
    {
        PachaUtils.SkipDisclaimerIntros();
    }
}