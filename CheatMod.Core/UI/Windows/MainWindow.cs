using UnityEngine;

namespace CheatMod.Core.UI.Windows;


public class MainWindow : PachaCheatWindow
{
    private bool _isGrowCropsAroundClicked;

    private bool _isWaterAllTilesClicked;
    private Rect _windowRect = new(16, 16, 200, 350);


    public MainWindow(PachaManager manager) : base(manager)
    {
    }

    private bool IsWaterAllTilesClicked
    {
        get => _isWaterAllTilesClicked;
        set
        {
            if (_isWaterAllTilesClicked == value) return;
            _isWaterAllTilesClicked = value;
            if (value)
                Manager.PachaCheats.WaterAllTilledTiles();
        }
    }

    private bool IsGrowCropsAroundClicked
    {
        get => _isGrowCropsAroundClicked;
        set
        {
            if (_isGrowCropsAroundClicked == value) return;
            _isGrowCropsAroundClicked = value;
            if (value)
                Manager.PachaCheats.GrowCrops(9f);
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

    protected override void DrawInternal(int windowId)
    {
        GUILayout.BeginVertical();

        CheatOptions.IsEasyFishingEnabled =
            GUILayout.Toggle(CheatOptions.IsEasyFishingEnabled, "Easy fishing", CheatUIStyles.Toggle);
        CheatOptions.IsInfiniteFluteEnabled =
            GUILayout.Toggle(CheatOptions.IsInfiniteFluteEnabled, "Infinite flute", CheatUIStyles.Toggle);
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

        GUILayout.Space(20);

        IsWaterAllTilesClicked = GUILayout.Button("Water all crops");
        IsGrowCropsAroundClicked = GUILayout.Button("Grow crops around");

        GUILayout.FlexibleSpace();

        if (GUILayout.Button(!CheatOptions.DrawItemSpawnerWindow ? "Open item spawner" : "Close item spawner"))
            HandleOpenItemSpawnerClick();

        if (GUILayout.Button(!CheatOptions.DrawTimeManagerWindow ? "Open time manager" : "Close time manager"))
            HandleOpenTimeManagerClick();

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    public override void Draw()
    {
        _windowRect = GUILayout.Window((int)CheatWindowType.Main, _windowRect, DrawInternal, "Pacha Cheat");
    }

}