namespace CheatMod.Core.CheatCommands;

public interface ICheatCommandExecutor<in TCommand> where TCommand : ICheatCommand
{
    void Execute(TCommand command);
}