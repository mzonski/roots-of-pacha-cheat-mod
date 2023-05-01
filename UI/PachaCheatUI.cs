using System.Collections.Generic;
using RootsOfPachaCheatMod.UI.Windows;
using UnityEngine;

namespace RootsOfPachaCheatMod.UI;

public class PachaCheatUI
{
    private readonly PachaManager _manager;
    private Rect _windowRect = new(16, 16, 200, 300);
    private Rect _itemSpawnerWindow = new(220, 16, 400, 520);

    private readonly Dictionary<CheatWindowType, PachaCheatWindow> _windows;

    public PachaCheatUI(PachaManager manager)
    {
        _manager = manager;
        _windows = new Dictionary<CheatWindowType, PachaCheatWindow>()
        {
            { CheatWindowType.Main, new MainWindow(manager) },
            { CheatWindowType.ItemSpawner, new ItemSpawnerWindow(manager) }
        };
    }

    public void Draw()
    {
        if (!_manager.Config.DrawUI)
            return;

        _windowRect = GUILayout.Window((int)CheatWindowType.Main, _windowRect, _windows[CheatWindowType.Main].Draw,
            "Pacha Cheat");

        if (_manager.Config.ItemSpawnerWindowOpen)
            _itemSpawnerWindow = GUILayout.Window((int)CheatWindowType.ItemSpawner, _itemSpawnerWindow,
                _windows[CheatWindowType.ItemSpawner].Draw, "Pacha Item Spawner");
    }
}