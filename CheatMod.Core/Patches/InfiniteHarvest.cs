using HarmonyLib;
using SodaDen.Pacha;
using UnityEngine;

namespace CheatMod.Core.Patches;


public partial class CheatModPatches
{
    [HarmonyPatch(typeof(PlantEntity), "HarvestID")]
    [HarmonyPrefix]
    private static bool InfinitePlantHarvestPatch(PlantEntity __instance, ref string __result)
    {
        if (!CheatOptions.IsInfiniteHarvestEnabled)
            return true;
        
        __result = $"{__instance.ID}-{__instance.CurrentDay.Value}-${Random.Range(1, int.MaxValue)}";
        return false;
    }
    
    
/*
 *             fruits.Add(new MagneticItemData()
            {
              ID = string.Format("{0}-{1}-{2}", (object) this.Entity.ID, (object) this.StartFlowerIn.Value, (object) index),
              ItemWithProperties = itemWithProperties,
              FromPosition = (NullableVector2) fromPosition.Value,
              ToPosition = (NullableVector2) distanceForBounce,
              SpawnBehavior = SpawnBehavior.FallInYThenShortBounceToX,
              Source = ItemObtainedSource.Harvested
            });
          }
        }));
        return fruits.ToArray();
 */
    
    [HarmonyPatch(typeof(TreeEntity), "FruitToDrop", MethodType.Getter)]
    [HarmonyPostfix]
    private static void InfiniteTreeHarvestPatch(TreeEntity __instance, ref MagneticItemData[] __result)
    {
        if (!CheatOptions.IsInfiniteHarvestEnabled)
            return;

        for (var i = 0; i < __result.Length; i++)
        {
            __result[i].ID =  $"{__result[i].ID}-${Random.Range(1, int.MaxValue)}";
        }
    }
}