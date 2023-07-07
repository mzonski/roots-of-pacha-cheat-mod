using UnityEngine;

namespace CheatMod.Core.UI.Windows;


public class MainWindow : PachaCheatWindow
{
    private Rect _mainWindow = new(16, 16, 200, 450);

    public MainWindow(PachaManager manager) : base(manager)
    {
    }

    private bool _isWaterAllTilesClicked;
    private bool IsWaterAllTilesClicked
    {
        set
        {
            if (_isWaterAllTilesClicked == value) return;
            _isWaterAllTilesClicked = value;
            if (value)
                Manager.PachaCheats.WaterAllTilledTiles();
        }
    }

    private bool _isGrowCropsAroundClicked;
    private bool IsGrowCropsAroundClicked
    {
        set
        {
            if (_isGrowCropsAroundClicked == value) return;
            _isGrowCropsAroundClicked = value;
            if (value)
                Manager.PachaCheats.GrowCrops(9f);
        }
    }
    
    private bool _isGrowTreesAroundClicked;
    private bool IsGrowTreesAroundClicked
    {
        set
        {
            if (_isGrowTreesAroundClicked == value) return;
            _isGrowTreesAroundClicked = value;
            if (value)
                Manager.PachaCheats.GrowTrees(9f);
        }
    }

    private void HandleOpenItemSpawnerClick()
    {
        if (CheatOptions.DrawItemSpawnerWindow)
        {
            CheatOptions.DrawItemSpawnerWindow = false;
            return;
        }

        Manager.ItemDb.Refresh();
        CheatOptions.DrawItemSpawnerWindow = true;
    }

    private void HandleOpenTimeManagerClick()
    {
        if (CheatOptions.DrawTimeManagerWindow)
        {
            CheatOptions.DrawTimeManagerWindow = false;
            return;
        }

        CheatOptions.DrawTimeManagerWindow = true;
    }

    private void HandleOpenTeleportsClick()
    {
        if (CheatOptions.DrawTeleportWindow)
        {
            CheatOptions.DrawTeleportWindow = false;
            return;
        }

        CheatOptions.DrawTeleportWindow = true;
    }

    private void HandleOpenAnimalShuffleWindow()
    {
        if (CheatOptions.DrawAnimalShuffleWindow)
        {
            CheatOptions.DrawAnimalShuffleWindow = false;
            return;
        }

        CheatOptions.DrawAnimalShuffleWindow = true;
    }

    protected override void DrawInternal(int windowId)
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

        IsWaterAllTilesClicked = GUILayout.Button("Water all crops");
        IsGrowCropsAroundClicked = GUILayout.Button("Grow crops around");
        IsGrowTreesAroundClicked = GUILayout.Button("Grow trees around");
        
        GUILayout.Space(20);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button(!CheatOptions.DrawItemSpawnerWindow ? "Open item spawner" : "Close item spawner"))
            HandleOpenItemSpawnerClick();

        if (GUILayout.Button(!CheatOptions.DrawTimeManagerWindow ? "Open time manager" : "Close time manager"))
            HandleOpenTimeManagerClick();

        if (GUILayout.Button(!CheatOptions.DrawTeleportWindow ? "Open teleports" : "Close teleports"))
            HandleOpenTeleportsClick();
        
        if (GUILayout.Button(!CheatOptions.DrawAnimalShuffleWindow ? "Open animal shuffler" : "Close animal shuffler"))
            HandleOpenAnimalShuffleWindow();

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    public override void Draw()
    {
        _mainWindow = GUILayout.Window((int)CheatWindowType.Main, _mainWindow, DrawInternal, "Pacha Cheat");
    }

}