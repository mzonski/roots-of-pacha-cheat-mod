using System.Linq;
using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(PlayerManualProducerState), "ProductTimeout", MethodType.Getter)]
    [HarmonyPrefix]
    private static bool ManualProducerProductTimeout(ref float __result)
    {
        if (!CheatOptions.Instance.IsFastProductionEnabled.Value) return true;
        __result = 0.2f;
        return false;
    }
    
    [HarmonyPatch(typeof(PlayerManualProducerState), "Timeout", MethodType.Getter)]
    [HarmonyPostfix]
    private static float ManualProducerTimeout(float value)
    {
        if (!CheatOptions.Instance.IsFastProductionEnabled.Value) return value;
        if (value > 0.2f)
        {
            return 0.2f;
        }
    
        return value;
    }
    
    [HarmonyPatch(typeof(AutoProducerEntity), "UpdateStatusAndScheduleNextCheck", MethodType.Normal)]
    [HarmonyPrefix]
    private static void AutoProducerHasFuel(ref AutoProducerEntity __instance)
    {
        if (!CheatOptions.Instance.IsFastProductionEnabled.Value) return;
        foreach (var currentConversion in __instance.CurrentConversions.Where(c => c is not null))
        {
            currentConversion.StartedOnDayTime = 1.0f;
        }
    }
}