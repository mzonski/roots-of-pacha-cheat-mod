using System.Threading;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using SodaDen.Pacha;

namespace CheatMod.Core.Patches;

public partial class CheatModPatches
{
    [HarmonyPatch(typeof(FluteToolItem), "Use")]
    [HarmonyPrefix]
    private static bool InfiniteFlutePatch(FluteToolItem __instance, PlayerEntity player, InventoryEntity entity,
        float stamina)
    {
        if (CheatOptions.IsInfiniteFluteEnabled)
            __instance.ToolPropertyWithData(entity).Level = __instance.MaxLevel;

        return true;
    }
    
    [HarmonyPatch(typeof(AnimalAttuneMinigame), "StartMinigame", typeof(Animal), typeof(AnimalVariant), typeof(bool), typeof(CancellationToken))]
    [HarmonyPrefix]
    private static bool SkipAttuneMinigamePatch(ref UniTask<int> __result)
    {
        if (!CheatOptions.IsSkipAttuneMinigameEnabled)
            return true;
        
        __result = UniTask.FromResult(4);
        return false;
    }
}