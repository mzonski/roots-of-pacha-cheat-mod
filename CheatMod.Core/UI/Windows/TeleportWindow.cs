using UnityEngine;

namespace CheatMod.Core.UI.Windows;

public class TeleportWindow : PachaCheatWindow
{
    private Rect _teleportWindowRect = new(220, 16, 200, 350);

    public TeleportWindow(PachaManager manager) : base(manager)
    {
    }

    private bool _isTpToCampfireClicked;

    private bool IsTpToCampfireClicked
    {
        set
        {
            if (_isTpToCampfireClicked == value) return;
            _isTpToCampfireClicked = value;
            if (value)
                Manager.PachaCheats.TeleportPlayer(11.75151f, 1.96547f);
        }
    }

    private bool _isTpToJungleClicked;

    private bool IsTpToJungleClicked
    {
        set
        {
            if (_isTpToJungleClicked == value) return;
            _isTpToJungleClicked = value;
            if (value)
                Manager.PachaCheats.TeleportPlayer(23997.19f, 16.04053f);
        }
    }

    private bool _isTpToCavesClicked;

    private bool IsTpToCavesClicked
    {
        set
        {
            if (_isTpToCavesClicked == value) return;
            _isTpToCavesClicked = value;
            if (value)
                Manager.PachaCheats.TeleportPlayer(30001.5f, 4.3f);
        }
    }

    protected override void DrawInternal(int windowId)
    {
        GUILayout.BeginVertical();


        IsTpToCampfireClicked = GUILayout.Button("Campfire");
        IsTpToJungleClicked = GUILayout.Button("Jungle");
        IsTpToCavesClicked = GUILayout.Button("Caves (Forest)");

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    public override void Draw()
    {
        if (!CheatOptions.DrawTeleportWindow) return;
        _teleportWindowRect = GUILayout.Window((int)CheatWindowType.Teleports, _teleportWindowRect, DrawInternal, "Teleports");
    }
}