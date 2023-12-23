using CheatMod.Core.Utils;
using UnityEngine;

namespace CheatMod.Core.UI.Windows;

public class TeleportWindow : PachaCheatWindow
{
    private Rect _teleportWindowRect = new(220, 16, 200, 290);

    public TeleportWindow(PachaManager manager) : base(manager)
    {
    }
    
    public override void Draw()
    {
        if (!CheatOptions.Instance.DrawTeleportWindow.Value) return;
        _teleportWindowRect = GUILayout.Window(CheatWindowType.Teleports, _teleportWindowRect, DrawWindow, "Teleports");
    }

    protected override void DrawWindow(int windowId)
    {
        GUILayout.BeginVertical();
        
        if (GUILayout.Button("Campfire"))
            Manager.PachaCheats.TeleportPlayer(PachaTpLocation.Campfire);
        if (GUILayout.Button("Jungle"))
            Manager.PachaCheats.TeleportPlayer(PachaTpLocation.Jungle);
        if (GUILayout.Button("Savanna"))
            Manager.PachaCheats.TeleportPlayer(PachaTpLocation.Savanna);
        if (GUILayout.Button("Beach"))
            Manager.PachaCheats.TeleportPlayer(PachaTpLocation.Beach);
        
        GUILayout.Space(20);

        if (GUILayout.Button("Caves (Forest)"))
            Manager.PachaCheats.TeleportPlayer(PachaTpLocation.CavesForest);
        if (GUILayout.Button("Caves (Horse)"))
            Manager.PachaCheats.TeleportPlayer(PachaTpLocation.CavesHorse);
        if (GUILayout.Button("Caves (Turtle)"))
            Manager.PachaCheats.TeleportPlayer(PachaTpLocation.CavesTurtle);
        if (GUILayout.Button("Caves (Owl)"))
            Manager.PachaCheats.TeleportPlayer(PachaTpLocation.CavesOwl);
        if (GUILayout.Button("Caves (Monkey)"))
            Manager.PachaCheats.TeleportPlayer(PachaTpLocation.CavesMonkey);
        if (GUILayout.Button("Caves (Bear)"))
            Manager.PachaCheats.TeleportPlayer(PachaTpLocation.CavesBear);

        GUILayout.EndVertical();

        GUI.DragWindow();
    }
    
    private struct PachaTpLocation
    {
        public static TeleportCoordinates Campfire = new(11.75151f, 1.96547f);
        public static TeleportCoordinates Jungle = new(23997.19f, 16.04053f);
        public static TeleportCoordinates Savanna = new(11982.87f, -12.66215f);
        public static TeleportCoordinates Beach = new(18002.43f, 19.39732f);
        public static TeleportCoordinates CavesForest = new(30001.5f, 4.3f);
        public static TeleportCoordinates CavesHorse = new(29980.94f, 4528.404f);
        public static TeleportCoordinates CavesTurtle = new(29998.98f, 2002.019f);
        public static TeleportCoordinates CavesOwl = new(32002.05f, 2518.746f);
        public static TeleportCoordinates CavesMonkey = new(31017.75f, 7021.971f);
        public static TeleportCoordinates CavesBear = new(27491.32f, 10500.71f);
    }
}