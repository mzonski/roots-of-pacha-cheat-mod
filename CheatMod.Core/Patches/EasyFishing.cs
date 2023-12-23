using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(FishingHitMode), "FocusRise", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool FishingFocusRisePatch(ref float __result)
    {
        if (!CheatOptions.Instance.IsEasyFishingEnabled.Value) return true;
        __result = 1000f;
        return false;
    }
    
    [HarmonyPatch(typeof(FishingMinigame), "FishSpawnBuff", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool FishingSpawnBuffPatch(ref float __result)
    {
        if (!CheatOptions.Instance.IsEasyFishingEnabled.Value) return true;
        __result = 1000f;
        return false;
    }
    [HarmonyPatch(typeof(FishingMinigame), "FishCatchBuff", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool FishingCatchBuffPatch(ref float __result)
    {
        if (!CheatOptions.Instance.IsEasyFishingEnabled.Value) return true;
        __result = 1000f;
        return false;
    }
    [HarmonyPatch(typeof(FishingMinigame), "FishRarityBuff", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool FishingRarityBuffPatch(ref float __result)
    {
        if (!CheatOptions.Instance.IsEasyFishingEnabled.Value) return true;
        __result = 1000f;
        return false;
    }
    [HarmonyPatch(typeof(FishingShoalController), "SpawnRate", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool FishingRarityBuffPatch(ref int __result)
    {
        if (!CheatOptions.Instance.IsEasyFishingEnabled.Value) return true;
        __result = 1;
        return false;
    }
}