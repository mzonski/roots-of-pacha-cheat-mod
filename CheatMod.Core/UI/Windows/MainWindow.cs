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

        CheatOptions.IsEasyFishingEnabled =
            GUILayout.Toggle(CheatOptions.IsEasyFishingEnabled, "Easy fishing", CheatUIStyles.Toggle);
        CheatOptions.IsInfiniteFluteEnabled =
            GUILayout.Toggle(CheatOptions.IsInfiniteFluteEnabled, "Infinite flute", CheatUIStyles.Toggle);
        CheatOptions.IsEasyAnimalsEnabled =
            GUILayout.Toggle(CheatOptions.IsEasyAnimalsEnabled, "Easy animals", CheatUIStyles.Toggle);
        CheatOptions.IsInfiniteSeedsEnabled =
            GUILayout.Toggle(CheatOptions.IsInfiniteSeedsEnabled, "Infinite seeds", CheatUIStyles.Toggle);
        CheatOptions.IsInfiniteStaminaEnabled =
            GUILayout.Toggle(CheatOptions.IsInfiniteStaminaEnabled, "Infinite stamina", CheatUIStyles.Toggle);
        CheatOptions.IsInfiniteWaterToolEnabled = GUILayout.Toggle(CheatOptions.IsInfiniteWaterToolEnabled,
            "Infinite water tool", CheatUIStyles.Toggle);
        CheatOptions.IsMovementSpeedEnabled = GUILayout.Toggle(CheatOptions.IsMovementSpeedEnabled,
            "Movement speedhack", CheatUIStyles.Toggle);
        CheatOptions.IsInfiniteHarvestEnabled = GUILayout.Toggle(CheatOptions.IsInfiniteHarvestEnabled,
            "Infinite harvest", CheatUIStyles.Toggle);
        CheatOptions.IsFastProductionEnabled = GUILayout.Toggle(CheatOptions.IsFastProductionEnabled,
            "Fast production", CheatUIStyles.Toggle);

        GUILayout.Space(20);

        if (GUILayout.Button("Water all crops"))
            Manager.PachaCheats.WaterAllTilledTiles();

        if (GUILayout.Button("Grow crops around"))
            Manager.PachaCheats.GrowCrops(9f);

        if (GUILayout.Button("Grow trees around"))
            Manager.PachaCheats.GrowTrees(9f);

        GUILayout.Space(20);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button(!CheatOptions.DrawItemSpawnerWindow ? "Open item spawner" : "Close item spawner"))
            ToggleItemSpawnerWindow();

        if (GUILayout.Button(!CheatOptions.DrawTimeManagerWindow ? "Open time manager" : "Close time manager"))
            CheatOptions.DrawTimeManagerWindow = !CheatOptions.DrawTimeManagerWindow;

        if (GUILayout.Button(!CheatOptions.DrawTeleportWindow ? "Open teleports" : "Close teleports"))
            CheatOptions.DrawTeleportWindow = !CheatOptions.DrawTeleportWindow;

        if (GUILayout.Button(!CheatOptions.DrawAnimalShuffleWindow ? "Open animal shuffler" : "Close animal shuffler"))
            CheatOptions.DrawAnimalShuffleWindow = !CheatOptions.DrawAnimalShuffleWindow;

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    private void ToggleItemSpawnerWindow()
    {
        if (CheatOptions.DrawItemSpawnerWindow)
        {
            CheatOptions.DrawItemSpawnerWindow = false;
            return;
        }

        Manager.ItemDb.Refresh();
        CheatOptions.DrawItemSpawnerWindow = true;
    }
}