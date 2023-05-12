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
        CheatOptions.IsInfiniteWaterToolEnabled = GUILayout.Toggle(CheatOptions.IsInfiniteWaterToolEnabled, "Infinite water tool",
            CheatUIStyles.Toggle);
        CheatOptions.IsMovementSpeedEnabled = GUILayout.Toggle(CheatOptions.IsMovementSpeedEnabled, "Movement speedhack",
            CheatUIStyles.Toggle);

        GUILayout.Space(20);

        _waterAllTilesClickHandler.Debounce()(GUILayout.Button("Water all crops"));
        _growCropsAroundClickHandler.Debounce()(GUILayout.Button("Grow crops around"));

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