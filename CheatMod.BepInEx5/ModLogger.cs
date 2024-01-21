using BepInEx.Logging;
using CheatMod.Core.Utils;

namespace CheatMod.BepInEx5;

public class ModLogger : IModLogger
{
    private readonly ManualLogSource _logSource;

    public ModLogger()
    {
        _logSource = Logger.CreateLogSource("CheatMod");
        Logger.Sources.Add(_logSource);
    }

    public void Log(string message)
    {
        _logSource.LogInfo(message);
    }

    public void Log(object obj)
    {
        _logSource.LogInfo(obj);
    }
}