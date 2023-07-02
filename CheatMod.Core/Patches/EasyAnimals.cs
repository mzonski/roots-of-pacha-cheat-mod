using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(AnimalAttuneMinigame), "StartMinigame", typeof(Animal), typeof(AnimalVariant), typeof(bool), typeof(CancellationToken))]
    [HarmonyPrefix]
    private static bool SkipAttuneMinigamePatch(ref UniTask<int> __result)
    {
        if (!CheatOptions.IsEasyAnimalsEnabled)
            return true;
        
        __result = UniTask.FromResult(4);
        return false;
    }
    
    [HarmonyPatch(typeof(AnimalEntity), "OnNextDayLocal")]
    [HarmonyPostfix]
    private static void AnimalEntityAutoFeed(AnimalEntity __instance)
    {
        if (!CheatOptions.IsEasyAnimalsEnabled) return;

        if (__instance.IsTamed || __instance.IsPet)
        {
            var feedMethodInfo = __instance.GetType().GetMethod("Feed", BindingFlags.Instance | BindingFlags.NonPublic);
            feedMethodInfo?.Invoke(__instance, new object[] { __instance.CurrentDay });

            if (__instance.Hunger != AnimalHunger.WellFed)
            {
                feedMethodInfo?.Invoke(__instance, new object[] { __instance.CurrentDay - 1 });
                feedMethodInfo?.Invoke(__instance, new object[] { __instance.CurrentDay - 2 });
                feedMethodInfo?.Invoke(__instance, new object[] { __instance.CurrentDay - 3 });
            }
            
            __instance.CureAllSickness();
        }
    }
}