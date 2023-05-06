using UnityEngine;

namespace CheatMod.Core.UI.Windows;

public class PlayerStatsWindow : PachaCheatWindow
{
    private Rect _statsWindow = new(16, 370, 300, 60);

    public PlayerStatsWindow(PachaManager manager) : base(manager)
    {
    }

    protected override void DrawInternal(int windowId)
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Movement speed");
        Manager.Config.PlayerMovementSpeed =
            (int)GUI.HorizontalSlider(new Rect(126, 26, 180, 20), Manager.Config.PlayerMovementSpeed, 1, 10);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    public override void Draw()
    {
        if (Manager.Config.IsMovementSpeedEnabled)
            _statsWindow = GUILayout.Window((int)CheatWindowType.PlayerStats, _statsWindow, DrawInternal, "Pacha Player Stats");
    }
}