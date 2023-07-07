using UnityEngine;

namespace CheatMod.Core.UI.Windows;

public class TeleportWindow : PachaCheatWindow
{
    private Rect _teleportWindowRect = new(220, 16, 200, 290);

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
    
    private bool _isTpToSavannaClicked;

    private bool IsTpToSavannaClicked
    {
        set
        {
            if (_isTpToSavannaClicked == value) return;
            _isTpToSavannaClicked = value;
            if (value)
                Manager.PachaCheats.TeleportPlayer(11982.87f, -12.66215f);
        }
    }
    
    private bool _isTpToBeachClicked;

    private bool IsTpToBeachClicked
    {
        set
        {
            if (_isTpToBeachClicked == value) return;
            _isTpToBeachClicked = value;
            if (value)
                Manager.PachaCheats.TeleportPlayer(18002.43f, 19.39732f);
        }
    }
    
    private bool _isTpToTurtleTotemClicked;

    private bool IsTpToTurtleTotemClicked
    {
        set
        {
            if (_isTpToTurtleTotemClicked == value) return;
            _isTpToTurtleTotemClicked = value;
            if (value)
                Manager.PachaCheats.TeleportPlayer(29998.98f, 2002.019f);
        }
    }

    private bool _isTpToOwlTotemClicked;

    private bool IsTpToOwlTotemClicked
    {
        set
        {
            if (_isTpToOwlTotemClicked == value) return;
            _isTpToOwlTotemClicked = value;
            if (value)
                Manager.PachaCheats.TeleportPlayer(32002.05f, 2518.746f);
        }
    }

    private bool _isTpToMonkeyTotemClicked;

    private bool IsTpToMonkeyTotemClicked
    {
        set
        {
            if (_isTpToMonkeyTotemClicked == value) return;
            _isTpToMonkeyTotemClicked = value;
            if (value)
                Manager.PachaCheats.TeleportPlayer(31017.75f, 7021.971f);
        }
    }

    private bool _isTpToBearRoomClicked;

    private bool IsTpToBearRoomClicked
    {
        set
        {
            if (_isTpToBearRoomClicked == value) return;
            _isTpToBearRoomClicked = value;
            if (value)
                Manager.PachaCheats.TeleportPlayer(27491.32f, 10500.71f);
        }
    }

    private bool _isTpToHorseTotemClicked;

    private bool IsTpToHorseTotemClicked
    {
        set
        {
            if (_isTpToHorseTotemClicked == value) return;
            _isTpToHorseTotemClicked = value;
            if (value)
                Manager.PachaCheats.TeleportPlayer(29980.94f, 4528.404f);
        }
    }


    protected override void DrawInternal(int windowId)
    {
        GUILayout.BeginVertical();


        IsTpToCampfireClicked = GUILayout.Button("Campfire");
        IsTpToJungleClicked = GUILayout.Button("Jungle");
        IsTpToSavannaClicked = GUILayout.Button("Savanna");
        IsTpToBeachClicked = GUILayout.Button("Beach");
        GUILayout.Space(20);
        IsTpToCavesClicked = GUILayout.Button("Caves (Forest)");
        IsTpToHorseTotemClicked = GUILayout.Button("Caves (Horse)");
        IsTpToTurtleTotemClicked = GUILayout.Button("Caves (Turtle)");
        IsTpToOwlTotemClicked = GUILayout.Button("Caves (Owl)");
        IsTpToMonkeyTotemClicked = GUILayout.Button("Caves (Monkey)");
        IsTpToBearRoomClicked = GUILayout.Button("Caves (Bear)");

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    public override void Draw()
    {
        if (!CheatOptions.DrawTeleportWindow) return;
        _teleportWindowRect = GUILayout.Window((int)CheatWindowType.Teleports, _teleportWindowRect, DrawInternal, "Teleports");
    }
}