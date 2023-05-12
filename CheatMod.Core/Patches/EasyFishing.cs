using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(FishingHitMode), "FocusRise", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool EasyFishingPatch(ref float __result)
    {
        if (!CheatOptions.IsEasyFishingEnabled) return true;
        __result = 1000f;
        return false;
    }
}