using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheatMod.Core.CheatCommands;
using CheatMod.Core.CheatCommands.AddItemToInventory;
using CheatMod.Core.CheatCommands.DestroyHittableResources;
using CheatMod.Core.CheatCommands.GrowCrops;
using CheatMod.Core.CheatCommands.GrowTrees;
using CheatMod.Core.CheatCommands.RegenerateHittableResources;
using CheatMod.Core.CheatCommands.SetTime;
using CheatMod.Core.CheatCommands.ShuffleAnimalHerd;
using CheatMod.Core.CheatCommands.TeleportPlayer;
using CheatMod.Core.CheatCommands.WaterAllTiles;

namespace CheatMod.Core.Managers;

public class CheatCommandMediator
{
    private readonly Dictionary<object, object> _commandExecutors = new();

    public CheatCommandMediator(PachaManager manager)
    {
        RegisterCommandExecutor(new AddItemToInventoryCommandExecutor(manager));
        RegisterCommandExecutor(new DestroyHittableResourcesCommandExecutor(manager));
        RegisterCommandExecutor(new GrowCropsCommandExecutor(manager));
        RegisterCommandExecutor(new GrowTreesCommandExecutor(manager));
        RegisterCommandExecutor(new RegenerateHittableResourcesCommandExecutor(manager));
        RegisterCommandExecutor(new SetTimeCommandExecutor(manager));
        RegisterCommandExecutor(new ShuffleAnimalHerdCommandExecutor(manager));
        RegisterCommandExecutor(new TeleportPlayerCommandExecutor(manager));
        RegisterCommandExecutor(new WaterAllTilesCommandExecutor(manager));
    }

    private void RegisterCommandExecutor<T>(CheatCommandExecutor<T> executor) where T : ICheatCommand
    {
        _commandExecutors[typeof(T)] = executor;
    }

    public void Execute<TCheatCommand>(TCheatCommand command) where TCheatCommand : ICheatCommand
    {
        if (!_commandExecutors.TryGetValue(typeof(TCheatCommand), out var executorObj))
        {
            throw new ArgumentOutOfRangeException(nameof(command), "Handler for command not found");
        }

        if (executorObj is CheatCommandExecutor<TCheatCommand> executor)
        {
            new Task(() =>
            {
                executor.Execute(command);
            }).Start();
        }
        else
        {
            throw new InvalidOperationException("Invalid executor type");
        }
    }
}