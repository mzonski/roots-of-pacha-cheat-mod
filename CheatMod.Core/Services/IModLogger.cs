namespace CheatMod.Core.Services;

public interface IModLogger
{
    void Log(string message);
    void Log(object obj);
}