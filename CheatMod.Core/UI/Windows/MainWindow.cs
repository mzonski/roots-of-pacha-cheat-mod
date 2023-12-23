using UnityEngine;

namespace CheatMod.Core.UI.Windows;

public class MainWindow : PachaCheatWindow
{
    private Rect _mainWindow = new(16, 16, 200, 450);

    public MainWindow(PachaManager manager) : base(manager)
    {
    }

    public override void Draw()
    {
        _mainWindow = GUILayout.Window(CheatWindowType.Main, _mainWindow, DrawWindow, "Pacha Cheat");
    }

    protected override void DrawWindow(int windowId)
    {
        GUILayout.BeginVertical();

        CheatOptions.Instance.IsEasyFishingEnabled.Value = GUILayout.Toggle(
            CheatOptions.Instance.IsEasyFishingEnabled.Value, "Easy fishing", CheatUIStyles.Toggle);
        CheatOptions.Instance.IsInfiniteFluteEnabled.Value = GUILayout.Toggle(
            CheatOptions.Instance.IsInfiniteFluteEnabled.Value, "Infinite flute", CheatUIStyles.Toggle);
        CheatOptions.Instance.IsEasyAnimalsEnabled.Value = GUILayout.Toggle(
            CheatOptions.Instance.IsEasyAnimalsEnabled.Value, "Easy animals", CheatUIStyles.Toggle);
        CheatOptions.Instance.IsInfiniteSeedsEnabled.Value = GUILayout.Toggle(
            CheatOptions.Instance.IsInfiniteSeedsEnabled.Value, "Infinite seeds", CheatUIStyles.Toggle);
        CheatOptions.Instance.IsInfiniteStaminaEnabled.Value = GUILayout.Toggle(
            CheatOptions.Instance.IsInfiniteStaminaEnabled.Value, "Infinite stamina", CheatUIStyles.Toggle);
        CheatOptions.Instance.IsInfiniteWaterToolEnabled.Value = GUILayout.Toggle(
            CheatOptions.Instance.IsInfiniteWaterToolEnabled.Value, "Infinite water tool", CheatUIStyles.Toggle);
        CheatOptions.Instance.IsMovementSpeedEnabled.Value = GUILayout.Toggle(
            CheatOptions.Instance.IsMovementSpeedEnabled.Value, "Movement speedhack", CheatUIStyles.Toggle);
        CheatOptions.Instance.IsInfiniteHarvestEnabled.Value = GUILayout.Toggle(
            CheatOptions.Instance.IsInfiniteHarvestEnabled.Value, "Infinite harvest", CheatUIStyles.Toggle);
        CheatOptions.Instance.IsFastProductionEnabled.Value = GUILayout.Toggle(
            CheatOptions.Instance.IsFastProductionEnabled.Value, "Fast production", CheatUIStyles.Toggle);

        GUILayout.Space(20);

        if (GUILayout.Button("Water all crops"))
            Manager.PachaCheats.WaterAllTilledTiles();

        if (GUILayout.Button("Grow crops around"))
            Manager.PachaCheats.GrowCrops(9f);

        if (GUILayout.Button("Grow trees around"))
            Manager.PachaCheats.GrowTrees(9f);

        GUILayout.Space(20);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button(
                !CheatOptions.Instance.DrawItemSpawnerWindow.Value ? "Open item spawner" : "Close item spawner"))
            ToggleItemSpawnerWindow();

        if (GUILayout.Button(
                !CheatOptions.Instance.DrawTimeManagerWindow.Value ? "Open time manager" : "Close time manager"))
            CheatOptions.Instance.DrawTimeManagerWindow.Value = !CheatOptions.Instance.DrawTimeManagerWindow.Value;

        if (GUILayout.Button(!CheatOptions.Instance.DrawTeleportWindow.Value ? "Open teleports" : "Close teleports"))
            CheatOptions.Instance.DrawTeleportWindow.Value = !CheatOptions.Instance.DrawTeleportWindow.Value;

        if (GUILayout.Button(!CheatOptions.Instance.DrawAnimalShuffleWindow.Value
                ? "Open animal shuffler"
                : "Close animal shuffler"))
            CheatOptions.Instance.DrawAnimalShuffleWindow.Value = !CheatOptions.Instance.DrawAnimalShuffleWindow.Value;

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    private void ToggleItemSpawnerWindow()
    {
        if (CheatOptions.Instance.DrawItemSpawnerWindow.Value)
        {
            CheatOptions.Instance.DrawItemSpawnerWindow.Value = false;
            return;
        }

        Manager.ItemDb.Refresh();
        CheatOptions.Instance.DrawItemSpawnerWindow.Value = true;
    }
}