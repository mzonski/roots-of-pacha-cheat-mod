using System;

namespace CheatMod.Core.CheatCommands.SetTime;

public class SetTimeCommand : ICheatCommand
{
    public TimeSpan Time { get; set; }
}