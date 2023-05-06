using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.BepInEx5;

public partial class CheatMod
{
    [HarmonyPatch(typeof(FishingHitMode), "FocusRise", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool EasyFishingPatch(ref float __result)
    {
        if (!PachaManager.Config.IsEasyFishingEnabled) return true;
        __result = 1000f;
        return false;
    }
}