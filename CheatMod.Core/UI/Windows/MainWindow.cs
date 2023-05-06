using System;
using UnityEngine;

namespace CheatMod.Core.UI.Windows;

public class MainWindow : PachaCheatWindow
{
    private Rect _windowRect = new(16, 16, 200, 350);

    private readonly Action<bool> _waterAllTilesClickHandler = isClicked =>
    {
        if (!isClicked) return;

        PachaCheats.WaterAllTilledTiles();
    };

    private readonly Action<bool> _growCropsAroundClickHandler = isClicked =>
    {
        if (!isClicked) return;

        PachaCheats.GrowCrops(9f);
    };

    public MainWindow(PachaManager manager) : base(manager)
    {
    }

    private void HandleOpenItemSpawnerClick()
    {
        if (Manager.Config.DrawItemSpawnerWindow)
        {
            Manager.Config.DrawItemSpawnerWindow = false;
            return;
        }

        Manager.ItemDb.Refresh();
        Manager.Config.DrawItemSpawnerWindow = true;
    }

    private void HandleOpenTimeManagerClick()
    {
        if (Manager.Config.DrawTimeManagerWindow)
        {
            Manager.Config.DrawTimeManagerWindow = false;
            return;
        }

        Manager.Config.DrawTimeManagerWindow = true;
    }

    protected override void DrawInternal(int windowId)
    {
        var config = Manager.Config;
        GUILayout.BeginVertical();

        config.IsEasyFishingEnabled =
            GUILayout.Toggle(config.IsEasyFishingEnabled, "Easy fishing", CheatUIStyles.Toggle);
        config.IsInfiniteFluteEnabled =
            GUILayout.Toggle(config.IsInfiniteFluteEnabled, "Infinite flute", CheatUIStyles.Toggle);
        config.IsInfiniteSeedsEnabled =
            GUILayout.Toggle(config.IsInfiniteSeedsEnabled, "Infinite seeds", CheatUIStyles.Toggle);
        config.IsInfiniteStaminaEnabled =
            GUILayout.Toggle(config.IsInfiniteStaminaEnabled, "Infinite stamina", CheatUIStyles.Toggle);
        config.IsInfiniteWaterToolEnabled = GUILayout.Toggle(config.IsInfiniteWaterToolEnabled, "Infinite water tool",
            CheatUIStyles.Toggle);
        config.IsMovementSpeedEnabled = GUILayout.Toggle(config.IsMovementSpeedEnabled, "Movement speedhack",
            CheatUIStyles.Toggle);

        GUILayout.Space(20);

        _waterAllTilesClickHandler.Debounce()(GUILayout.Button("Water all crops"));
        _growCropsAroundClickHandler.Debounce()(GUILayout.Button("Grow crops around"));

        GUILayout.FlexibleSpace();

        if (GUILayout.Button(!config.DrawItemSpawnerWindow ? "Open item spawner" : "Close item spawner"))
            HandleOpenItemSpawnerClick();

        if (GUILayout.Button(!config.DrawTimeManagerWindow ? "Open time manager" : "Close time manager"))
            HandleOpenTimeManagerClick();

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    public override void Draw()
    {
        _windowRect = GUILayout.Window((int)CheatWindowType.Main, _windowRect, DrawInternal, "Pacha Cheat");
    }
}