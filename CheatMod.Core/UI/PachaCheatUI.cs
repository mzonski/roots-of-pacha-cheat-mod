using System.Collections.Generic;
using CheatMod.Core.UI.Windows;

namespace CheatMod.Core.UI;

public class PachaCheatUI
{
    private readonly PachaManager _manager;

    private readonly Dictionary<CheatWindowType, PachaCheatWindow> _windows;

    public PachaCheatUI(PachaManager manager)
    {
        _manager = manager;
        _windows = new Dictionary<CheatWindowType, PachaCheatWindow>()
        {
            { CheatWindowType.Main, new MainWindow(manager) },
            { CheatWindowType.ItemSpawner, new ItemSpawnerWindow(manager) },
            { CheatWindowType.PlayerStats, new PlayerStatsWindow(manager) },
            { CheatWindowType.TimeManager, new TimeManagerWindow(manager) }
        };
    }

    public void Draw()
    {
        if (!_manager.Config.DrawUI)
            return;

        foreach (var window in _windows.Values)
        {
            window.Draw();
        }
    }
}