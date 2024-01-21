using CheatMod.Core.Managers;

namespace CheatMod.Core.CheatCommands;

public abstract class CheatCommandExecutor<TCommand> : ICheatCommandExecutor<TCommand>
    where TCommand : ICheatCommand
{
    protected readonly PachaManager Manager;

    protected CheatCommandExecutor(PachaManager manager)
    {
        Manager = manager;
    }

    public abstract void Execute(TCommand command);
}