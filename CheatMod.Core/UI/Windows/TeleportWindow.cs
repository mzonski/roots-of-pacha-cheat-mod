using CheatMod.Core.CheatCommands.TeleportPlayer;
using CheatMod.Core.Managers;
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

    private void TeleportPlayer(Vector2 coordinates)
    {
        Manager.Mediator.Execute(new TeleportPlayerCommand { X = coordinates.x, Y = coordinates.y });
    }

    protected override void DrawWindow(int windowId)
    {
        GUILayout.BeginVertical();

        if (GUILayout.Button("Campfire"))
            TeleportPlayer(PachaTpLocation.Campfire);
        if (GUILayout.Button("Jungle"))
            TeleportPlayer(PachaTpLocation.Jungle);
        if (GUILayout.Button("Savanna"))
            TeleportPlayer(PachaTpLocation.Savanna);
        if (GUILayout.Button("Beach"))
            TeleportPlayer(PachaTpLocation.Beach);

        GUILayout.Space(20);

        if (GUILayout.Button("Caves (Forest)"))
            TeleportPlayer(PachaTpLocation.CavesForest);
        if (GUILayout.Button("Caves (Horse)"))
            TeleportPlayer(PachaTpLocation.CavesHorse);
        if (GUILayout.Button("Caves (Turtle)"))
            TeleportPlayer(PachaTpLocation.CavesTurtle);
        if (GUILayout.Button("Caves (Owl)"))
            TeleportPlayer(PachaTpLocation.CavesOwl);
        if (GUILayout.Button("Caves (Monkey)"))
            TeleportPlayer(PachaTpLocation.CavesMonkey);
        if (GUILayout.Button("Caves (Bear)"))
            TeleportPlayer(PachaTpLocation.CavesBear);

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    private struct PachaTpLocation
    {
        public static Vector2 Campfire = new(11.75151f, 1.96547f);
        public static Vector2 Jungle = new(23997.19f, 16.04053f);
        public static Vector2 Savanna = new(11982.87f, -12.66215f);
        public static Vector2 Beach = new(18002.43f, 19.39732f);
        public static Vector2 CavesForest = new(30001.5f, 4.3f);
        public static Vector2 CavesHorse = new(29980.94f, 4528.404f);
        public static Vector2 CavesTurtle = new(29998.98f, 2002.019f);
        public static Vector2 CavesOwl = new(32002.05f, 2518.746f);
        public static Vector2 CavesMonkey = new(31017.75f, 7021.971f);
        public static Vector2 CavesBear = new(27491.32f, 10500.71f);
    }
}