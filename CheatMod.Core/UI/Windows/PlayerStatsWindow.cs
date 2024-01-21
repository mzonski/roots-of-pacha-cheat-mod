using CheatMod.Core.Managers;
using UnityEngine;

namespace CheatMod.Core.UI.Windows;

public class PlayerStatsWindow : PachaCheatWindow
{
    private Rect _statsWindow = new(16, 480, 300, 60);

    public PlayerStatsWindow(PachaManager manager) : base(manager)
    {
    }

    public override void Draw()
    {
        if (CheatOptions.Instance.IsMovementSpeedEnabled.Value)
            _statsWindow = GUILayout.Window(CheatWindowType.PlayerStats, _statsWindow, DrawWindow,
                "Pacha Player Stats");
    }

    protected override void DrawWindow(int windowId)
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Movement speed");
        CheatOptions.Instance.PlayerMovementSpeed.Value =
            (int)GUI.HorizontalSlider(new Rect(126, 26, 180, 20), CheatOptions.Instance.PlayerMovementSpeed.Value, 1,
                10);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUI.DragWindow();
    }
}