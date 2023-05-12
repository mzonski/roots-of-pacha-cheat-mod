namespace CheatMod.Core.Services;

public interface ILogger
{
    void Log(string message);
    ILogger Instance { get; internal set; }
}