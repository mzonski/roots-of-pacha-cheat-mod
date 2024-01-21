using System;
using System.Linq;
using System.Reflection;
using CheatMod.Core.Managers;
using SodaDen.Pacha;

namespace CheatMod.Core.CheatCommands.GrowTrees;

public class GrowTreesCommandExecutor : CheatCommandExecutor<GrowTreesCommand>
{
    public GrowTreesCommandExecutor(PachaManager manager) : base(manager)
    {
    }

    private void PatchTreeEntity(TreeEntity treeEntity)
    {
        var ageField =
            typeof(TreeEntity).GetField("Age", BindingFlags.NonPublic | BindingFlags.Instance);
        var healthField =
            typeof(TreeEntity).GetField("Health", BindingFlags.NonPublic | BindingFlags.Instance);
        var treeRendererProperty =
            typeof(TreeEntity).GetProperty("TreeRenderer", BindingFlags.NonPublic | BindingFlags.Instance);
        var currentDayProperty =
            typeof(TreeEntity).GetProperty("CurrentDay", BindingFlags.NonPublic | BindingFlags.Instance);

        if (ageField == null || healthField == null || treeRendererProperty == null ||
            currentDayProperty == null)
        {
            throw new ArgumentNullException(nameof(treeEntity),
                "Tree entity data fields are initialized incorrectly");
        }

        var renderer = (TreeRenderer)treeRendererProperty.GetValue(treeEntity);
        var healthComponent = (HealthComponent)healthField.GetValue(treeEntity);
        var currentDay = (int)currentDayProperty.GetValue(treeEntity);

        healthComponent.IncreaseHealth(treeEntity.Tree.StumpAtHealth + 1);
        ageField.SetValue(treeEntity, (short)treeEntity.Tree.MatureAge);
        Manager.Logger.Log("Current age: " + ageField.GetValue(treeEntity));

        var currentSeason = new YearSeasonDay(currentDay).Season;
        renderer.UpdateFromData(treeEntity.Tree, treeEntity.Tree.StageIndexAt(treeEntity.Tree.MatureAge),
            currentSeason,
            CheatOptions.Instance.IsInfiniteHarvestEnabled.Value ? treeEntity.Tree.DaysInFlower + 1 : 0, false,
            true);

        treeEntity.StartFlowerIn = 0;
        treeEntity.LastFloweredIn = null;
    }

    public override void Execute(GrowTreesCommand command)
    {
        try
        {
            Manager.Logger.Log("Grow trees");

            var treesInRange = CommandHelper.GetEntitiesInRange<TreeEntity>(command.Range)
                .ToList();

            Manager.Logger.Log($"Found {treesInRange.Count} trees in range");

            foreach (var treeEntity in treesInRange)
            {
                PatchTreeEntity(treeEntity);
            }
        }
        catch (Exception ex)
        {
            Manager.Logger.Log("[GrowTrees] Failed: " + ex.Message);
            Manager.Logger.Log(ex.StackTrace);
        }
    }
}