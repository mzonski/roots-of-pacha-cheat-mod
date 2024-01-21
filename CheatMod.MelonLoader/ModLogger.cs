using CheatMod.Core.Utils;
using MelonLoader;

namespace CheatMod.MelonLoader;

public class ModLogger : IModLogger
{
    public void Log(string message)
    {
        MelonLogger.Msg(message);
    }

    public void Log(object obj)
    {
        MelonLogger.Msg(obj);
    }
}