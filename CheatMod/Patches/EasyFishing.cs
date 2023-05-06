using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.MelonLoader;

public partial class CheatMod
{
    [HarmonyPatch(typeof(FishingHitMode), "FocusRise", MethodType.Getter)]
    private class EasyFishingPatch
    {
        private static bool Prefix(ref float __result)
        {
            if (!_pachaManager.Config.IsEasyFishingEnabled) return true;
            __result = 1000f;
            return false;
        }
    }
}