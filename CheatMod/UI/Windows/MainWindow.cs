using UnityEngine;

namespace RootsOfPachaCheatMod.UI.Windows;

public class MainWindow : PachaCheatWindow
{
    private Rect _windowRect = new(16, 16, 200, 300);

    public MainWindow(PachaManager manager) : base(manager)
    {
    }

    private void HandleOpenItemSpawnerClick()
    {
        if (Manager.Config.ItemSpawnerWindowOpen)
        {
            Manager.Config.ItemSpawnerWindowOpen = false;
            return;
        }

        Manager.ItemDb.Refresh();
        Manager.Config.ItemSpawnerWindowOpen = true;
    }

    public override void DrawInternal(int windowId)
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
        config.IsFreezeTimeEnabled = GUILayout.Toggle(config.IsFreezeTimeEnabled, "Freeze time",
            CheatUIStyles.Toggle);
        config.IsMovementSpeedEnabled = GUILayout.Toggle(config.IsMovementSpeedEnabled, "Enable player speedhack",
            CheatUIStyles.Toggle);

        GUILayout.Space(20);

        if (GUILayout.Button("Water all crops")) PachaCheats.WaterAllTilledTiles();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button(!config.ItemSpawnerWindowOpen ? "Open item spawner" : "Close item spawner"))
            HandleOpenItemSpawnerClick();

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    public override void Draw()
    {
        _windowRect = GUILayout.Window((int)CheatWindowType.Main, _windowRect, DrawInternal, "Pacha Cheat");
    }
}