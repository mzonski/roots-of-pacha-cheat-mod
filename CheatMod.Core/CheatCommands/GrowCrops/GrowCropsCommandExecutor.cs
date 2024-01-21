using System;
using System.Linq;
using System.Reflection;
using CheatMod.Core.Managers;
using SodaDen.Pacha;

namespace CheatMod.Core.CheatCommands.GrowCrops;

public class GrowCropsCommandExecutor : CheatCommandExecutor<GrowCropsCommand>
{
    public GrowCropsCommandExecutor(PachaManager manager) : base(manager)
    {
    }

    private static void PatchPlantEntity(PlantEntity plantEntity)
    {
        var lastHarvestedProperty =
            typeof(PlantEntity).GetProperty("LastHarvested", BindingFlags.NonPublic | BindingFlags.Instance);
        var plantAgeProperty =
            typeof(PlantEntity).GetProperty("Age", BindingFlags.NonPublic | BindingFlags.Instance);
        var updateRendererMethod =
            typeof(PlantEntity).GetMethod("UpdateRenderer", BindingFlags.NonPublic | BindingFlags.Instance);
        var levelProperty =
            typeof(PlantEntity).GetProperty("LevelWhenSpawnedOrLastHarvested",
                BindingFlags.NonPublic | BindingFlags.Instance);

        if (lastHarvestedProperty == null || plantAgeProperty == null || updateRendererMethod == null ||
            levelProperty == null)
            throw new ArgumentNullException(nameof(PlantEntity),
                "Plant entity data fields are initialized incorrectly");

        levelProperty.SetValue(plantEntity, 2);
        plantEntity.Withered = false;
        if (CheatOptions.Instance.IsInfiniteHarvestEnabled.Value)
            lastHarvestedProperty.SetValue(plantEntity,
                null);

        plantAgeProperty.SetValue(plantEntity, plantEntity.IsRepeating ? plantEntity.Plant.RepeatsEvery : 30f);
        
        updateRendererMethod.Invoke(plantEntity, null);
    }


    public override void Execute(GrowCropsCommand command)
    {
        try
        {
            Manager.Logger.Log("Grow crops");

            var plantsInRange = CommandHelper.GetEntitiesInRange<PlantEntity>(command.Range)
                .ToList();

            Manager.Logger.Log($"[GrowCrops] Found {plantsInRange.Count} plants in range");

            foreach (var plantEntity in plantsInRange) PatchPlantEntity(plantEntity);
        }
        catch (Exception ex)
        {
            Manager.Logger.Log("[GrowCrops] Failed: " + ex.Message);
            Manager.Logger.Log(ex.StackTrace);
        }
    }
}