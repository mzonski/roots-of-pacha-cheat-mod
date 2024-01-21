namespace CheatMod.Core.Utils;

public interface IModLogger
{
    void Log(string message);
    void Log(object obj);
}