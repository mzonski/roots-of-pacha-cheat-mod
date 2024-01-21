using CheatMod.Core.CheatCommands.DestroyHittableResources;
using CheatMod.Core.CheatCommands.GrowCrops;
using CheatMod.Core.CheatCommands.GrowTrees;
using CheatMod.Core.CheatCommands.ShuffleAnimalHerd;
using CheatMod.Core.Cheats;
using CheatMod.Core.Persistence;
using CheatMod.Core.UI;
using CheatMod.Core.Utils;
using UnityEngine;

namespace CheatMod.Core.Managers;

public class PachaManager
{
    private readonly PachaCheatUI _cheatUI;
    public readonly PachaItemDatabase ItemDatabase;
    public readonly CheatCommandMediator Mediator;
    public readonly IModLogger Logger;

    public PachaManager(IModLogger logger)
    {
        Logger = logger;
        ItemDatabase = new PachaItemDatabase();
        _cheatUI = new PachaCheatUI(this);
        Mediator = new CheatCommandMediator(this);

        CheatOptions.Load();

        var harmony = new CheatModHarmony();
        harmony.ApplyPatches();
    }

    public void CatchKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.F2)) CheatOptions.Instance.DrawUI.Value = !CheatOptions.Instance.DrawUI.Value;

        if (Input.GetKeyDown(KeyCode.F5)) Mediator.Execute(new GrowCropsCommand());

        if (Input.GetKeyDown(KeyCode.F6)) Mediator.Execute(new GrowTreesCommand());
        
        if (Input.GetKeyDown(KeyCode.F8)) Mediator.Execute(new DestroyHittableResourcesCommand());

        // if (Input.GetKeyDown(KeyCode.F9)) PachaCheats.GetPlayerCurrentCoords();

        // if (Input.GetKeyDown(KeyCode.F10)) PachaCheats.DumpEntitiesInRange();

        if (Input.GetKeyDown(KeyCode.F11)) Mediator.Execute(new ShuffleAnimalHerdCommand());
    }

    public void DrawGui()
    {
        _cheatUI.Draw();
    }
}